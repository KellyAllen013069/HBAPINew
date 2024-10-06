using System.Security.Claims;
using HBAPI.Data;
using HBAPI.DTOs;
using HBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly HbDbContext _context;

        public CartController(HbDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItem(CartItemDto? cartItemDto)
        {
            if (cartItemDto == null) return BadRequest("Cart item data is required");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            Console.WriteLine($"user is is ${userId}");
            
            var cartItem = new CartItem
            {
                UserId = userId,
                ClassId = cartItemDto.ClassId,
                ClassName = cartItemDto.ClassName,
                Day = cartItemDto.Day,
                Price = cartItemDto.Price,
                CouponCode = cartItemDto.CouponCode,
                // Assuming Dates is stored as a JSON array in the database
                Dates = JsonConvert.SerializeObject(cartItemDto.Dates),
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
            
            return Ok();
        }
    }
}