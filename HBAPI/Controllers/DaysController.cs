using HBAPI.Data;
using HBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HBAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DaysController : ControllerBase
{
    private readonly HbDbContext _context;

    public DaysController(HbDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Day>>> GetDays()
    {
        return await _context.Days.ToListAsync();
    } 
}