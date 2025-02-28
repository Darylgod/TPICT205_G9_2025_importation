using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_programme : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_programme(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Programme>>> GetProgrammes()
        {
            return await _context.Programmes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Programme>> GetProgramme(int id)
        {
            var programme = await _context.Programmes.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }
            return programme;
        }

        [HttpPost]
        public async Task<ActionResult<Programme>> PostProgramme(Programme programme)
        {
            _context.Programmes.Add(programme);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProgramme), new { id = programme.IdProgramme }, programme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgramme(int id, Programme programme)
        {
            if (id != programme.IdProgramme)
            {
                return BadRequest();
            }
            _context.Entry(programme).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgramme(int id)
        {
            var programme = await _context.Programmes.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }
            _context.Programmes.Remove(programme);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
