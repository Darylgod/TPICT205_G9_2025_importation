using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using module_user.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace module_user.Middleware
{
    //🔹 Comment ça marche ?


    // L'idée est d'ajouter une vérification dans un middleware qui va :

    //  1️⃣ Lire le token JWT depuis l'en-tête de la requête (Authorization: Bearer <token>)
    //2️⃣ Vérifier la date d'expiration du token

    //    3️⃣ Supprimer l'utilisateur de user_login si le token est expiré
    //apparement si aucune requete nest envoyer ca ne marche pas
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public TokenExpirationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory; // ✅ Utilise un ScopeFactory pour obtenir BonitaContext
        }

        public async Task Invoke(HttpContext context)
        {
            using (var scope = _scopeFactory.CreateScope()) // ✅ Crée un scope pour BonitaContext
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BonitaContext>(); // ✅ Récupère BonitaContext

                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                        if (!string.IsNullOrEmpty(username))
                        {
                            var userLogin = await dbContext.UserLogins
                                .Where(ul => ul.Name == username)
                                .FirstOrDefaultAsync();

                            if (userLogin != null)
                            {
                                dbContext.UserLogins.Remove(userLogin);
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }


}



