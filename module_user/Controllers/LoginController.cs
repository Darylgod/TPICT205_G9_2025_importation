using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using module_user.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace module_user.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly BonitaContext _context;
        private readonly IConfiguration _config;

        public LoginController(BonitaContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        //pour gérer le login et générer un token
        //Quand un utilisateur envoie une requête de connexion (par exemple, via une méthode POST /login), l'API vérifie ses informations d'identification, et si elles sont valides, elle génère un token JWT avec une durée d'expiration.

        //elle generre le token via la methode   GenerateJwtToken
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

              var user = await _context.Users
             .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
            }
            /* var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

             if (user == null)
             {
                 return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
             }*/

            // 🔹 Récupérer le rôle de l'utilisateur
            // 🔹 Récupérer le rôle de l'utilisateur
            var role = await _context.UserMemberships
                .Where(um => um.Userid == user.Id)
                   .Select(um => new { um.Roleid, um.TenantId })
                    .FirstOrDefaultAsync();

            if (role == null)
            {
                return Unauthorized("Aucun rôle assigné à cet utilisateur.");
            }

            // 🔹 Récupérer le nom du rôle séparément
            var roleName = await _context.Roles
                .Where(r => r.Id == role.Roleid && r.TenantId == role.TenantId)  // 🔥 Vérifie le TenantId
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(roleName))
            {
                return Unauthorized("Rôle introuvable.");
            }
            // 🔹 Enregistrer la connexion dans `user_login`
            var userLogin = new UserLogin
            {
                TenantId = role.TenantId,
                Name = user.Username,
                Displayname = $"{user.FirstName} {user.LastName}",
                CreateBy = "System"
            };

            _context.UserLogins.Add(userLogin);
            await _context.SaveChangesAsync();
            var expiration = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("JwtSettings:ExpirationInMinutes"));
            // 🔹 Générer le token JWT
            var token = GenerateJwtToken(user, roleName);
            return Ok(new { Token = token, Role = roleName, Expiration = expiration });


        }

        private string GenerateJwtToken(User user, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            // Récupère la durée d'expiration du token depuis les paramètres
            var expirationInMinutes = _config.GetValue<int>("JwtSettings:ExpirationInMinutes");
            var expiration = DateTime.UtcNow.AddMinutes(expirationInMinutes);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        //Génère un nouveau token si l’ancien est expiré.
        //Si tu utilises un refresh token, tu peux permettre à l'utilisateur de récupérer un nouveau token sans avoir à se reconnecter. Le refresh token a une durée de vie plus longue et peut être utilisé pour obtenir un nouveau token d'accès lorsque celui-ci expire.
        //vu que bje veux que quand le token  expire il doit encore se reconnecter je ne dois pas utilise  refresh_token
        //Gestion du token expiré côté client :
        //Dans ton front-end(Blazor, par exemple), lorsque tu reçois une erreur 401 Unauthorized en raison de l'expiration du token, tu peux rediriger l'utilisateur vers la page de connexion ou déclencher une nouvelle tentative de récupération d'un nouveau token via un refresh token (si tu l'implémentes).

       


        //1️⃣ L'utilisateur clique sur "Se déconnecter".
        // 2️⃣ Le front-end envoie une requête POST à logout avec son username.
        // 3️⃣ L'API logout supprime l'utilisateur de la table user_login.
        // 4️⃣ Le front-end supprime aussi le token du stockage local (localStorage ou sessionStorage).
        //5️⃣ L'utilisateur est totalement déconnecté. ✅
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            Console.WriteLine($"Requête de déconnexion reçue pour : {request.Username}");

            var userLogin = await _context.UserLogins
                .Where(ul => ul.Name == request.Username)
                .FirstOrDefaultAsync();

            if (userLogin == null)
            {
                return NotFound("Utilisateur non trouvé dans user_login.");
            }

            _context.UserLogins.Remove(userLogin);
            await _context.SaveChangesAsync();

            return Ok("Déconnexion réussie.");
        }





    }
}
// 🔹 Modèle de la requête de connexion
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

//
public class LogoutRequest
{
    public string Username { get; set; }
}
