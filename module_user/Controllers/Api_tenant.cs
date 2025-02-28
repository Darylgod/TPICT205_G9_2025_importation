using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_user.Models;

namespace module_user.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Api_tenant : ControllerBase
    {
        private readonly BonitaContext _context;

        public Api_tenant(BonitaContext context)
        {
            _context = context;

        }
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(Tenant user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Données invalides.");



                // 🔹 Gérer createBy, createDate et lastUpdate
                user.CreateBy = "System";
                user.Created = DateTime.UtcNow;
               

                _context.Tenants.Add(user);
                await _context.SaveChangesAsync();

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        // ✅ GET : Récupérer tous les tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        // 📌 GET: Récupérer un Tenant par ID 
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenantById( int id)
        {
            var Tenant = await _context.Tenants.FindAsync( id);
            if (Tenant == null)
                return NotFound("tenant non trouvé.");

            return Tenant;
        }


        // 📌 PUT: Modifier un tenant
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser( int id, Tenant user)
        {
            if ( id != user.Id)
                return BadRequest("Données incohérentes.");

            var existingTenant = await _context.Tenants.FindAsync( id);
            if (existingTenant == null)
                return NotFound("Utilisateur non trouvé.");

            existingTenant.Description= user.Description;

            existingTenant.Created = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // 📌 DELETE: Supprimer un tenant
        [HttpDelete("{id}")]
       // Seulement les utilisateurs avec le rôle "Admin" auront accès à cette API  , apparement juste ca suffit pour restreintre lacces 
        public async Task<IActionResult> DeleteTenant ( int id)
        {
            var tenant = await _context.Tenants.FindAsync( id);
            if (tenant == null)
                return NotFound("Utilisateur non trouvé.");

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
