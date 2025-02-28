using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_grade : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_grade(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return grade;
        }

        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGrade), new { id = grade.IdGrade }, grade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, Grade grade)
        {
            if (id != grade.IdGrade)
            {
                return BadRequest();
            }
            _context.Entry(grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
