using HBAPI.Data;
using HBAPI.Models;
using HBAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesSessionsController : ControllerBase
    {
        private readonly HbDbContext _context;

        public ClassesSessionsController(HbDbContext context)
        {
            _context = context;
        }

        // GET: api/ClassesSessions/byClassId/{classId}
        [HttpGet("byClassId/{classId}")]
        public async Task<ActionResult<IEnumerable<ClassesSessionsDto>>> GetClassesSessionsByClassId(int classId)
        {
            var classSessions = await _context.ClassesSessions
                .Include(cs => cs.Day)  // Include the Day info
                .Include(cs => cs.DanceClass)  // Include the Class info
                .Where(cs => cs.ClassId == classId)
                .ToListAsync();

            if (classSessions == null || classSessions.Count == 0)
            {
                return NotFound(new { message = "No class sessions found for this class." });
            }

            var classSessionsDto = classSessions.Select(cs => new ClassesSessionsDto
            {
                Id = cs.Id,
                ClassId = cs.ClassId,
                DayId = cs.DayId,
                DayName = cs.Day.DayName,
                ClassName = cs.DanceClass.Name  
            }).ToList();

            return Ok(classSessionsDto);
        }

        // GET: api/ClassesSessions/byMonth?month={month}&year={year}
        [HttpGet("byMonth")]
        public async Task<ActionResult<IEnumerable<ClassesSessionsDto>>> GetClassesByMonth(int month, int year)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var classSessions = await _context.ClassesSessions
                .Include(cs => cs.DanceClass)  // Make sure to include DanceClass details
                .ThenInclude(dc => dc.ClassLevels) // Include class levels
                .ThenInclude(cl => cl.Level) // Include level name
                .Include(cs => cs.Day) // Include the day information
                .Where(cs => (cs.DanceClass.StartDate >= firstDayOfMonth && cs.DanceClass.StartDate <= lastDayOfMonth) ||
                             (cs.DanceClass.EndDate >= firstDayOfMonth && cs.DanceClass.EndDate <= lastDayOfMonth) ||
                             (cs.DanceClass.StartDate <= firstDayOfMonth && cs.DanceClass.EndDate >= lastDayOfMonth))
                .Select(cs => new ClassesSessionsDto
                {
                    Id = cs.Id,
                    ClassId = cs.ClassId,
                    ClassName = cs.DanceClass.Name,
                    Description = cs.DanceClass.Description,
                    Instructor = cs.DanceClass.Instructor,
                    StartTime = cs.DanceClass.StartTime,
                    EndTime = cs.DanceClass.EndTime,
                    DayId = cs.DayId,
                    DayName = cs.Day.DayName,
                    Price = cs.DanceClass.Price,
                    Levels = cs.DanceClass.ClassLevels.Select(cl => cl.Level.LevelName.ToString()).ToList() // Correct conversion
                })
                .ToListAsync();  // Remove the <ClassesSessionsDto>

            if (classSessions.Count == 0)
            {
                return NotFound();
            }

            return Ok(classSessions);
        }

        // POST: api/ClassesSessions
        [HttpPost]
        public async Task<ActionResult<ClassesSessionsDto>> AddClassesSessions(ClassesSessionsDto classesSessionsDto)
        {
            var classesSession = new ClassesSessions
            {
                ClassId = classesSessionsDto.ClassId,
                DayId = classesSessionsDto.DayId
            };

            _context.ClassesSessions.Add(classesSession);
            await _context.SaveChangesAsync();

            classesSessionsDto.Id = classesSession.Id;  
            return CreatedAtAction(nameof(GetClassesSessionsByClassId), new { classId = classesSession.ClassId }, classesSessionsDto);
        }
    }
}
