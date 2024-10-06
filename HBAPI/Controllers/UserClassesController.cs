using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HBAPI.Data;
using HBAPI.DTOs;
using HBAPI.Models;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClassesController : ControllerBase
    {
        private readonly HbDbContext _context;

        public UserClassesController(HbDbContext context)
        {
            _context = context;
        }

        // GET: api/UserClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserClassesDto>>> GetUserClasses()
        {
            var userClasses = await _context.UserClasses
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.DanceClass)
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.Day)
                .Select(uc => new UserClassesDto
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClassesSessionsId = uc.ClassesSessionsId,
                    Coupon = uc.Coupon,
                    CreatedAt = uc.CreatedAt,
                    ClassName = uc.ClassesSessions.DanceClass.Name,
                    Day = uc.ClassesSessions.Day.DayName,
                    StartTime = uc.ClassesSessions.DanceClass.StartTime,
                    EndTime = uc.ClassesSessions.DanceClass.EndTime,
                    Month = uc.ClassesSessions.DanceClass.StartDate.Month
                })
                .ToListAsync();

            return Ok(userClasses);
        }

        // GET: api/UserClasses/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<UserClassesDto>>> GetUserClassesByUserId(string userId)
        {
            var userClasses = await _context.UserClasses
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.DanceClass)
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.Day)
                .Where(uc => uc.UserId == userId)
                .Select(uc => new UserClassesDto
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClassesSessionsId = uc.ClassesSessionsId,
                    Coupon = uc.Coupon,
                    CreatedAt = uc.CreatedAt,
                    ClassName = uc.ClassesSessions.DanceClass.Name,
                    Day = uc.ClassesSessions.Day.DayName,
                    StartTime = uc.ClassesSessions.DanceClass.StartTime,
                    EndTime = uc.ClassesSessions.DanceClass.EndTime,
                    Month = uc.ClassesSessions.DanceClass.StartDate.Month
                })
                .ToListAsync();

            if (userClasses == null || !userClasses.Any())
            {
                return NotFound("No classes found for this user.");
            }

            return Ok(userClasses);
        }

        // GET: api/UserClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserClassesDto>> GetUserClass(int id)
        {
            var userClass = await _context.UserClasses
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.DanceClass)
                .Include(uc => uc.ClassesSessions)
                    .ThenInclude(cs => cs.Day)
                .Where(uc => uc.Id == id)
                .Select(uc => new UserClassesDto
                {
                    Id = uc.Id,
                    UserId = uc.UserId,
                    ClassesSessionsId = uc.ClassesSessionsId,
                    Coupon = uc.Coupon,
                    CreatedAt = uc.CreatedAt,
                    ClassName = uc.ClassesSessions.DanceClass.Name,
                    Day = uc.ClassesSessions.Day.DayName,
                    StartTime = uc.ClassesSessions.DanceClass.StartTime,
                    EndTime = uc.ClassesSessions.DanceClass.EndTime,
                    Month = uc.ClassesSessions.DanceClass.StartDate.Month
                })
                .FirstOrDefaultAsync();

            if (userClass == null)
            {
                return NotFound();
            }

            return Ok(userClass);
        }

        // PUT: api/UserClasses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserClass(int id, UserClassesDto userClassesDto)
        {
            if (id != userClassesDto.Id)
            {
                return BadRequest();
            }

            var userClass = await _context.UserClasses.FindAsync(id);
            if (userClass == null)
            {
                return NotFound();
            }

            // Map the DTO fields back to the entity
            userClass.Coupon = userClassesDto.Coupon;

            _context.Entry(userClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserClasses
        [HttpPost]
        public async Task<ActionResult<UserClasses>> CreateUserClass(UserClasses userClasses)
        {
            _context.UserClasses.Add(userClasses);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserClass), new { id = userClasses.Id }, userClasses);
        }

        // DELETE: api/UserClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserClass(int id)
        {
            var userClass = await _context.UserClasses.FindAsync(id);
            if (userClass == null)
            {
                return NotFound();
            }

            _context.UserClasses.Remove(userClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserClassExists(int id)
        {
            return _context.UserClasses.Any(e => e.Id == id);
        }
    }
}
