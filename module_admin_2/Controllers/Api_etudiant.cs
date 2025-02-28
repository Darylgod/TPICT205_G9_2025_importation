using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;

namespace module_admin1.Controllers
{
    
   
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    using module_admin_2.Models;
    using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
    [ApiController]
    public class Api_etudiant : ControllerBase
    {
        private readonly MyDbContext2 _context;

        public Api_etudiant(MyDbContext2 context)
        {
            _context = context;
        }

        // GET: api/etudiants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etudiant>>> GetEtudiants()
        {
            return await _context.Etudiants.ToListAsync();
        }

        // GET: api/etudiants/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Etudiant>> GetEtudiant(int id)
        {
            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant == null)
            {
                return NotFound();
            }
            return etudiant;
        }

        // POST: api/etudiants
        [HttpPost]
        public async Task<ActionResult<Etudiant>> PostEtudiant(Etudiant etudiant)
        {
            _context.Etudiants.Add(etudiant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEtudiant), new { id = etudiant.IdEtudiant }, etudiant);
        }

        // PUT: api/etudiants/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtudiant(int id, Etudiant etudiant)
        {
            if (id != etudiant.IdEtudiant)
            {
                return BadRequest();
            }
            _context.Entry(etudiant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/etudiants/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEtudiant(int id)
        {
            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant == null)
            {
                return NotFound();
            }
            _context.Etudiants.Remove(etudiant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
