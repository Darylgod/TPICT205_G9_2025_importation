using module_user.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace module_user.Middleware
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // Vérifie toutes les 5 minutes
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(30); // Durée de validité d'une session

        public TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("🔄 Vérification des sessions expirées...");

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<BonitaContext>();
                        var nowUtc = DateTime.UtcNow;

                        // Récupérer les sessions expirées
                        var expiredLogins = await dbContext.UserLogins
                            .Where(ul => ul.LastUpdate < nowUtc - _sessionTimeout)
                            .ToListAsync();

                        if (expiredLogins.Any())
                        {
                            _logger.LogInformation($"🗑️ Suppression de {expiredLogins.Count} sessions expirées...");
                            dbContext.UserLogins.RemoveRange(expiredLogins);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogInformation("✅ Aucune session expirée trouvée.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"❌ Erreur dans TokenCleanupService : {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken); // Attente avant la prochaine vérification
            }
        }
    }
}
