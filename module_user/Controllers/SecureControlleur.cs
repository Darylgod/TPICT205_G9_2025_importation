using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace module_user.Controllers
{
    
  

[Route("api/secure")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok("Données accessibles uniquement aux Admins !");
        }

        [HttpGet("user")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetUserData()
        {
            return Ok("Données accessibles aux utilisateurs et aux admins !");
        }
    }

}
