using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HBAPI.Data;
using HBAPI.Models;
using HBAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly HbDbContext _context;

        public ClassesController(HbDbContext context)
        {
            _context = context;
        }

        // POST: api/Classes
        [HttpPost]
        public async Task<IActionResult> PostClass([FromBody] CreateDanceClassRequest request)
        {
            var startTime = TimeSpan.Parse(request.StartTime);
            var endTime = TimeSpan.Parse(request.EndTime);
            
            var danceClass = new DanceClass
            {
                Name = request.Name,
                Description = request.Description,
                Instructor = request.Instructor,
                StartTime = startTime,
                EndTime = endTime,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Price = request.Price,
                Coupon = request.Coupon,
                MonthlyEligible = request.MonthlyEligible,
                ClassDays = request.ClassDays.Select(cd => new ClassDay
                {
                    DayId = cd.Id
                }).ToList(),
                ClassLevels = request.ClassLevels.Select(cl => new ClassLevel
                {
                    LevelId = cl.Id
                }).ToList()
            };

            _context.DanceClasses.Add(danceClass);
            await _context.SaveChangesAsync();

            return Ok(danceClass);
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanceClassDto>>> GetClasses()
        {
            var classes = await _context.DanceClasses
                .Include(c => c.ClassDays)
                    .ThenInclude(cd => cd.Day)
                .Include(c => c.ClassLevels)
                    .ThenInclude(cl => cl.Level)
                .Select(c => new DanceClassDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Instructor = c.Instructor,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Price = c.Price,
                    Coupon = c.Coupon,
                    MonthlyEligible = c.MonthlyEligible,
                    ClassDays = c.ClassDays.Select(cd => new ClassDayDto
                    {
                        Id = cd.Id,
                        DayName = cd.Day.DayName ?? string.Empty // Ensure correct DayName
                    }).ToList(),
                    ClassLevels = c.ClassLevels.Select(cl => new ClassLevelDto
                    {
                        Id = cl.Id,
                        LevelName = cl.Level.LevelName.ToString() // Convert LevelName to string
                    }).ToList()
                })
                .ToListAsync();

            if (classes.Count == 0)
            {
                return NotFound();
            }

            return Ok(classes);
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanceClassDto>> GetClass(int id)
        {
            var danceClass = await _context.DanceClasses
                .Include(c => c.ClassDays)
                    .ThenInclude(cd => cd.Day)
                .Include(c => c.ClassLevels)
                    .ThenInclude(cl => cl.Level)
                .Where(c => c.Id == id)
                .Select(c => new DanceClassDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Instructor = c.Instructor,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Price = c.Price,
                    Coupon = c.Coupon,
                    MonthlyEligible = c.MonthlyEligible,
                    ClassDays = c.ClassDays.Select(cd => new ClassDayDto
                    {
                        Id = cd.Id,
                        DayName = cd.Day.DayName ?? string.Empty
                    }).ToList(),
                    ClassLevels = c.ClassLevels.Select(cl => new ClassLevelDto
                    {
                        Id = cl.Id,
                        LevelName = cl.Level.LevelName.ToString()
                    }).ToList()
                })
                .SingleOrDefaultAsync();

            if (danceClass == null)
            {
                return NotFound();
            }

            return Ok(danceClass);
        }

        // GET: api/Classes/byMonth
        [HttpGet("byMonth")]
        public async Task<ActionResult<IEnumerable<DanceClassDto>>> GetClassesByMonth(int month, int year)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var classes = await _context.DanceClasses
                .Include(c => c.ClassDays)
                    .ThenInclude(cd => cd.Day)
                .Include(c => c.ClassLevels)
                    .ThenInclude(cl => cl.Level)
                .Where(c => (c.StartDate >= firstDayOfMonth && c.StartDate <= lastDayOfMonth) ||
                            (c.EndDate >= firstDayOfMonth && c.EndDate <= lastDayOfMonth) ||
                            (c.StartDate <= firstDayOfMonth && c.EndDate >= lastDayOfMonth))
                .OrderBy(c => c.ClassDays.Min(cd => cd.Day.Id))
                .Select(c => new DanceClassDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Instructor = c.Instructor,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Price = c.Price,
                    Coupon = c.Coupon,
                    MonthlyEligible = c.MonthlyEligible,
                    ClassDays = c.ClassDays.Select(cd => new ClassDayDto
                    {
                        Id = cd.Day.Id,
                        DayName = cd.Day.DayName
                    }).OrderBy(cd => cd.Id).ToList(),
                    ClassLevels = c.ClassLevels.Select(cl => new ClassLevelDto
                    {
                        Id = cl.Level.Id,
                        LevelName = cl.Level.LevelName.ToString()
                    }).ToList()
                })
                .ToListAsync();

            if (classes.Count == 0)
            {
                return NotFound();
            }

            return Ok(classes);
        }

        // PUT: api/Classes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, [FromBody] CreateDanceClassRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Class ID mismatch.");
            }

            var startTime = TimeSpan.Parse(request.StartTime);
            var endTime = TimeSpan.Parse(request.EndTime);

            var danceClass = await _context.DanceClasses
                .Include(c => c.ClassDays)
                .Include(c => c.ClassLevels)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (danceClass == null)
            {
                return NotFound();
            }

            // Update the dance class properties
            danceClass.Name = request.Name;
            danceClass.Description = request.Description;
            danceClass.Instructor = request.Instructor;
            danceClass.StartTime = startTime;
            danceClass.EndTime = endTime;
            danceClass.StartDate = request.StartDate;
            danceClass.EndDate = request.EndDate;
            danceClass.Price = request.Price;
            danceClass.Coupon = request.Coupon;
            danceClass.MonthlyEligible = request.MonthlyEligible;

            // Clear the existing ClassDays and ClassLevels
            danceClass.ClassDays.Clear();
            danceClass.ClassLevels.Clear();

            // Update ClassDays
            foreach (var classDay in request.ClassDays)
            {
                danceClass.ClassDays.Add(new ClassDay { ClassId = id, DayId = classDay.Id });
            }

            // Update ClassLevels
            foreach (var classLevel in request.ClassLevels)
            {
                danceClass.ClassLevels.Add(new ClassLevel { ClassId = id, LevelId = classLevel.Id });
            }

            _context.Entry(danceClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanceClassExists(id))
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

        private bool DanceClassExists(int id)
        {
            return _context.DanceClasses.Any(e => e.Id == id);
        }
    }
}
