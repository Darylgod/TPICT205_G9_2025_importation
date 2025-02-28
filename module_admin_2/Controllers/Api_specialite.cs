using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin1.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class Api_specialite : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_specialite(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialite>>> GetSpecialites()
        {
            return await _context.Specialites.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialite>> GetSpecialite(int id)
        {
            var specialite = await _context.Specialites.FindAsync(id);
            if (specialite == null)
            {
                return NotFound();
            }
            return specialite;
        }

        [HttpPost]
        public async Task<ActionResult<Specialite>> PostSpecialite(Specialite specialite)
        {
            _context.Specialites.Add(specialite);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSpecialite), new { id = specialite.IdSpecialite }, specialite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialite(int id, Specialite specialite)
        {
            if (id != specialite.IdSpecialite)
            {
                return BadRequest();
            }
            _context.Entry(specialite).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialite(int id)
        {
            var specialite = await _context.Specialites.FindAsync(id);
            if (specialite == null)
            {
                return NotFound();
            }
            _context.Specialites.Remove(specialite);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
