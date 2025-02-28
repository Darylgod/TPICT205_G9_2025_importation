using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using module_user.Models;

namespace module_user.Controllers
{
    [Route("api/usercontactinfo")]
    [ApiController]
    public class UserContactInfoController : ControllerBase
    {
        private readonly BonitaContext _context;

        public UserContactInfoController(BonitaContext context)
        {
            _context = context;
        }

        // 🔹 1. Récupérer tous les contacts d'un tenant
        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetAll(int tenantId)
        {
            var contacts = await _context.UserContactinfos
                .Where(c => c.TenantId == tenantId)
                .ToListAsync();

            return Ok(contacts);
        }

        // 🔹 2. Récupérer les contacts d'un utilisateur spécifique
        [HttpGet("{tenantId}/{userId}")]
        public async Task<IActionResult> GetByUser(int tenantId, int userId)
        {
            var contact = await _context.UserContactinfos
                .Where(c => c.TenantId == tenantId && c.Userid == userId)
                .FirstOrDefaultAsync();

            if (contact == null)
                return NotFound("Aucun contact trouvé pour cet utilisateur.");

            return Ok(contact);
        }

        // 🔹 3. Ajouter un contact
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserContactinfo contactInfo)
        {
            if (contactInfo == null)
                return BadRequest("Données invalides.");

            _context.UserContactinfos.Add(contactInfo);
            await _context.SaveChangesAsync();
            return Ok("Contact ajouté avec succès.");
        }

        // 🔹 4. Modifier un contact
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserContactinfo contactInfo)
        {
            var existingContact = await _context.UserContactinfos.FindAsync(id);
            if (existingContact == null)
                return NotFound("Contact non trouvé.");

            existingContact.Email = contactInfo.Email;
            existingContact.Phone = contactInfo.Phone;
            existingContact.WhatsApp = contactInfo.WhatsApp;
            existingContact.Address = contactInfo.Address;
            existingContact.City = contactInfo.City;
            existingContact.State = contactInfo.State;
            existingContact.Country = contactInfo.Country;

            await _context.SaveChangesAsync();
            return Ok("Contact mis à jour.");
        }

        // 🔹 5. Supprimer un contact
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.UserContactinfos.FindAsync(id);
            if (contact == null)
                return NotFound("Contact non trouvé.");

            _context.UserContactinfos.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok("Contact supprimé.");
        }
    }

}
