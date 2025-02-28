using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_user.Models;

namespace module_user.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class Api_role:ControllerBase
    {
        private readonly BonitaContext _context;

        public Api_role(BonitaContext context)
        {
            _context = context;

        }
    



        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Données invalides.");

                // 🔹 Vérifier s'il existe déjà un tenant
                var existingTenant = await _context.Tenants.FirstOrDefaultAsync();
                if (existingTenant == null)
                {
                    Console.WriteLine("il y a pas de tenanat");

                    await _context.SaveChangesAsync();
                }
                // 🔹 Assigner automatiquement le tenant_id
                user.TenantId = existingTenant.Id;

                // 🔹 Gérer createBy, createDate et lastUpdate
                
                user.CreateDate = DateTime.UtcNow;
                user.LastUpdate = DateTime.UtcNow;

                _context.Roles.Add(user);
                await _context.SaveChangesAsync();

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }


        // ✅ GET : Récupérer tous les utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // 📌 GET: Récupérer un utilisateur par ID et tenant_id
        [HttpGet("{tenantId}/{id}")]
        public async Task<ActionResult<Role>> GetRolesById(int tenantId, int id)
        {
            var user = await _context.Roles.FindAsync(tenantId, id);
            if (user == null)
                return NotFound("role  non trouvé.");

            return user;
        }

        // 📌 PUT: Modifier un utilisateur
        [HttpPut("{tenantId}/{id}")]
        public async Task<IActionResult> PutRole(int tenantId, int id, Role user)
        {
            if (tenantId != user.TenantId || id != user.Id)
                return BadRequest("Données incohérentes.");

            var existingUser = await _context.Roles.FindAsync(tenantId, id);
            if (existingUser == null)
                return NotFound("role non trouvé.");

            existingUser.Name = user.Name;
            existingUser.Displayname = user.Displayname;
            existingUser.CreateDate = DateTime.UtcNow;
            existingUser.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // 📌 DELETE: Supprimer un utilisateur
        [HttpDelete("{tenantId}/{id}")]
         // Seulement les utilisateurs avec le rôle "Admin" auront accès à cette API  , apparement juste ca suffit pour restreintre lacces 
        public async Task<IActionResult> DeleteRole(int tenantId, int id)
        {
            var user = await _context.Roles.FindAsync(tenantId, id);
            if (user == null)
                return NotFound("Utilisateur non trouvé.");

            _context.Roles.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }


}
