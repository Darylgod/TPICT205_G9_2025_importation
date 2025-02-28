using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_admin_2.Models;

using System;

namespace module_admin2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class Api_classe : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_classe(MyDbContext2 context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classe>>> GetClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Classe>> GetClasse(int id)
        {
            var classe = await _context.Classes.FindAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            return classe;
        }

        [HttpPost]
        public async Task<ActionResult<Classe>> PostClasse(Classe classe)
        {
            _context.Classes.Add(classe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClasse), new { id = classe.IdClasse }, classe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClasse(int id, Classe classe)
        {
            if (id != classe.IdClasse)
            {
                return BadRequest();
            }
            _context.Entry(classe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClasse(int id)
        {
            var classe = await _context.Classes.FindAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            _context.Classes.Remove(classe);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
