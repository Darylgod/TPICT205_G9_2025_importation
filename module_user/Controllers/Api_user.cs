using global::module_user.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace module_user.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_user : ControllerBase
    {
        private readonly BonitaContext _context;

        public Api_user(BonitaContext context)
        {
            _context = context;

        }
            [HttpPost]
            public async Task<ActionResult<User>> PostUser(User user)
            {
                try
                {
                    if (user == null)
                        return BadRequest("Données invalides.");

                    // 🔹 Vérifier s'il existe déjà un tenant
                    var existingTenant = await _context.Tenants.FirstOrDefaultAsync();
                    if (existingTenant == null)
                    {
                        // 🔹 Créer un tenant automatiquement
                        existingTenant = new Tenant
                        {
                            Description = "Tenant automatique",
                            CreateBy = "System",
                            Created = DateTime.UtcNow
                        };
                        _context.Tenants.Add(existingTenant);
                        await _context.SaveChangesAsync();
                    }

                    // 🔹 Assigner automatiquement le tenant_id
                    user.TenantId = existingTenant.Id;
                   // 🔹 Hacher le mot de passe avant de l'enregistrer
                   user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    // 🔹 Gérer createBy, createDate et lastUpdate
                    user.CreateBy = user.Username;
                    user.CreateDate = DateTime.UtcNow;
                    user.LastUpdate = DateTime.UtcNow;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                //
                // ✅ 🔹 Vérifier si le rôle "Visiteur" existe déjà
                var visiteurRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name == "Visiteur" && r.TenantId == user.TenantId);

                if (visiteurRole == null)
                {
                    return BadRequest("Le rôle 'Visiteur' n'existe pas. Veuillez le créer d'abord.");
                }

                // 🔹 Appeler l'API `AssignUserRole`
                var userMembership = new UserMembership
                {
                    Userid = user.Id,
                    Roleid = visiteurRole.Id,
                    TenantId = user.TenantId,
                    AssignDate = DateTime.UtcNow,
                    AssignBy = "System" // Peut être remplacé par un admin connecté
                };

                _context.UserMemberships.Add(userMembership);
                await _context.SaveChangesAsync();




                //
                return Ok(user);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erreur : {ex.Message}");
                }
            }


        // ✅ GET : Récupérer tous les utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // 📌 GET: Récupérer un utilisateur par ID et tenant_id
        [HttpGet("{tenantId}/{id}")]
        public async Task<ActionResult<User>> GetUserById(int tenantId, int id)
        {
            var user = await _context.Users.FindAsync(tenantId, id);
            if (user == null)
                return NotFound("Utilisateur non trouvé.");

            return user;
        }

        // 📌 PUT: Modifier un utilisateur
        [HttpPut("{tenantId}/{id}")]
        public async Task<IActionResult> PutUser(int tenantId, int id, User user)
        {
            if (tenantId != user.TenantId || id != user.Id)
                return BadRequest("Données incohérentes.");

            var existingUser = await _context.Users.FindAsync(tenantId, id);
            if (existingUser == null)
                return NotFound("Utilisateur non trouvé.");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Title = user.Title;
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.LastUpdate = DateTime.UtcNow;
            // 🔹 Hacher le mot de passe uniquement s'il est modifié
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // 📌 DELETE: Supprimer un utilisateur
        [HttpDelete("{tenantId}/{id}")]
        [Authorize(Roles = "Admin")] // Seulement les utilisateurs avec le rôle "Admin" auront accès à cette API  , apparement juste ca suffit pour restreintre lacces 
        public async Task<IActionResult> DeleteUser(int tenantId, int id)
        {
            var user = await _context.Users.FindAsync(tenantId, id);
            if (user == null)
                return NotFound("Utilisateur non trouvé.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
