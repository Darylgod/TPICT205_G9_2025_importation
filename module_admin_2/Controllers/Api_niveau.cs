using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin1.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class Api_niveau : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_niveau(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Niveau>>> GetNiveaux()
        {
            return await _context.Niveaus.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Niveau>> GetNiveau(int id)
        {
            var niveau = await _context.Niveaus.FindAsync(id);
            if (niveau == null)
            {
                return NotFound();
            }
            return niveau;
        }

        [HttpPost]
        public async Task<ActionResult<Niveau>> PostNiveau(Niveau niveau)
        {
            _context.Niveaus.Add(niveau);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNiveau), new { id = niveau.IdNiveau }, niveau);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNiveau(int id, Niveau niveau)
        {
            if (id != niveau.IdNiveau)
            {
                return BadRequest();
            }
            _context.Entry(niveau).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNiveau(int id)
        {
            var niveau = await _context.Niveaus.FindAsync(id);
            if (niveau == null)
            {
                return NotFound();
            }
            _context.Niveaus.Remove(niveau);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
