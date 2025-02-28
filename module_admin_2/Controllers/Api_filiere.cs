using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_filiere : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_filiere(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filiere>>> GetFilieres()
        {
            return await _context.Filieres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Filiere>> GetFiliere(int id)
        {
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound();
            }
            return filiere;
        }

        [HttpPost]
        public async Task<ActionResult<Filiere>> PostFiliere(Filiere filiere)
        {
            _context.Filieres.Add(filiere);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFiliere), new { id = filiere.IdFiliere }, filiere);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFiliere(int id, Filiere filiere)
        {
            if (id != filiere.IdFiliere)
            {
                return BadRequest();
            }
            _context.Entry(filiere).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFiliere(int id)
        {
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound();
            }
            _context.Filieres.Remove(filiere);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
