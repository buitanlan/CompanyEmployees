using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.IDP.InitialSeed;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PersistedGrantDbContext>();
        context.Database.Migrate();

        var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
        try
        {
            configurationDbContext.Database.Migrate();
            if (!configurationDbContext.Clients.Any())
            {
                foreach(var entity in Config.Clients)
                {
                    configurationDbContext.Clients.Add(entity.ToEntity());
                }
            }


            if (!configurationDbContext.IdentityResources.Any())
            {
                foreach (var entity in Config.IdentityResources)
                {
                    configurationDbContext.IdentityResources.Add(entity.ToEntity());
                }
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                foreach (var entity in Config.ApiScopes)
                {
                    configurationDbContext.ApiScopes.Add(entity.ToEntity());

                }
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var entity in Config.Apis)
                {
                    configurationDbContext.ApiResources.Add(entity.ToEntity());
                }
            }

            configurationDbContext.SaveChanges();

        }
        catch (Exception e)
        {
            throw;
        }
        return host;

    }
}
