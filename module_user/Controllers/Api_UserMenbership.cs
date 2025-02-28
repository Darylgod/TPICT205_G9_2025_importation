using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_user.Models;

namespace module_user.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserMembershipController : ControllerBase
    {
        private readonly BonitaContext _context;

        public UserMembershipController(BonitaContext context)
        {
            _context = context;
        }


        // 1️⃣ Liste toutes les associations utilisateur-rôle
        [HttpGet]
        public async Task<IActionResult> GetAllUserMemberships()
        {
            var memberships = await _context.UserMemberships.ToListAsync();
            return Ok(memberships);
        }

        // 2️⃣ Récupère une association par ID
        [HttpGet("{tenant_id}/{id}")]
        public async Task<IActionResult> GetUserMembership(int id)
        {
            var membership = await _context.UserMemberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound(new { message = "Aucune association trouvée" });
            }
            return Ok(membership);
        }

        // 3️⃣ Ajoute un utilisateur à un rôle
        [HttpPost]
        public async Task<IActionResult> AssignUserRole([FromBody] UserMembership userMembership)
        {
            if (userMembership == null)
            {
                return BadRequest(new { message = "Données invalides" });
            }

            // 🔹 Vérifier si le tenant existe
            var existingTenant = await _context.Tenants.FirstOrDefaultAsync();
            if (existingTenant == null)
            {
                return BadRequest(new { message = "Aucun tenant trouvé" });
            }

            // 🔹 Assigner automatiquement le tenant_id
            userMembership.TenantId = existingTenant.Id;

            // 🔹 Vérifier si l'utilisateur appartient bien à ce tenant
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userMembership.Userid && u.TenantId == userMembership.TenantId);

            if (existingUser == null)
            {
                return BadRequest(new { message = "Utilisateur introuvable pour ce tenant" });
            }

            // 🔹 Vérifier si le rôle appartient bien à ce tenant
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == userMembership.Roleid && r.TenantId == userMembership.TenantId);

            if (existingRole == null)
            {
                return BadRequest(new { message = "Rôle introuvable pour ce tenant" });
            }

            // 🔹 Ajouter l'association
            userMembership.AssignDate = DateTime.Now;
            userMembership.AssignBy = "System"; // À remplacer par l'ID de l'admin connecté

            _context.UserMemberships.Add(userMembership);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserMembership), new { id = userMembership.Id }, userMembership);
        }

        [HttpPut("{tenantId}/{id}")]
        public async Task<IActionResult> UpdateUserMembership(int tenantId, int id, [FromBody] UserMembership userMembership)
        {
            if (id != userMembership.Id)
            {
                Console.WriteLine($"ID dans l'URL: {id}, ID dans le corps: {userMembership.Id}");
                return BadRequest(new { message = "L'ID ne correspond pas" });

            }

            var existingMembership = await _context.UserMemberships.FindAsync(tenantId, id);
            if (existingMembership == null)
            {
                return NotFound(new { message = "Aucune association trouvée" });
            }

            existingMembership.Roleid = userMembership.Roleid;
            existingMembership.AssignDate = DateTime.Now; // Mise à jour de la date d'attribution
            existingMembership.AssignBy = "System"; // Mettre ici l'ID de l'utilisateur qui modifie

            _context.UserMemberships.Update(existingMembership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{tenantId}/{id}")]
        public async Task<IActionResult> DeleteUserMembership(int tenantId, int id)
        {
            var membership = await _context.UserMemberships.FindAsync(tenantId, id);
            if (membership == null)
            {
                return NotFound(new { message = "Aucune association trouvée" });
            }

            _context.UserMemberships.Remove(membership);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
