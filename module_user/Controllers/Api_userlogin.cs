using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using module_user.Models;

namespace module_user.Controllers
{
   
    using global::module_user.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace module_user.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class Api_userlogin : ControllerBase
        {
            private readonly BonitaContext _context;

            public Api_userlogin(BonitaContext context)
            {
                _context = context;

            }


            // ✅ GET : Récupérer tous les utilisateurs
            [HttpGet]
            public async Task<ActionResult<IEnumerable<UserLogin>>> GetUsers()
            {
                return await _context.UserLogins.ToListAsync();
            }

        }
    }

}
