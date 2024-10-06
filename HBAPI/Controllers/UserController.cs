using Microsoft.AspNetCore.Mvc;
using HBAPI.Models;
using HBAPI.DTOs;
using HBAPI.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HbDbContext _context;

        public UserController(HbDbContext context)
        {
            _context = context;
        }

        // GET: api/User/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<UsersDto>> GetUser(string userId) // UserId from Cognito is a string
        {
            // Find user by UserId (which is a string in Cognito)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            // Map User to UserDto
            var userDto = new UsersDto
            {
                Id = user.Id, // This remains an int (database primary key)
                UserId = user.UserId, // Cognito UserId, which is a string
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredName = user.PreferredName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };

            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UsersDto>> AddUser(UsersDto userDto)
        {
            // Check if a user with the same email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "Email already in use." });
            }

            // Create a new User from the provided UserDto
            var user = new Users
            {
                UserId = userDto.UserId,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PreferredName = userDto.PreferredName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Role = userDto.Role
            };

            // Add user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return the created user with the generated UserId
            userDto.UserId = user.UserId;

            return CreatedAtAction(nameof(GetUser), new { userId = user.UserId }, userDto);
        }

        // GET: api/User/byEmail/{email}
        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Map the found User to a DTO
            var userDto = new UsersDto
            {
                Id = user.Id,
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredName = user.PreferredName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };

            return Ok(userDto);
        }
    }
}
