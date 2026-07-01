using CompanyEmployees.IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace CompanyEmployees.IDP.InitialSeed;

public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<UserContext>();
        await context.Database.MigrateAsync();

        await SeedUsersAsync(services);
        await SeedOpenIddictAsync(services);

        return host;
    }

    private static async Task SeedUsersAsync(IServiceProvider services)
    {
        await SeedUserData.EnsureSeedDataAsync(services);
    }

    private static async Task SeedOpenIddictAsync(IServiceProvider services)
    {
        var scopeManager = services.GetRequiredService<IOpenIddictScopeManager>();
        foreach (var descriptor in Config.ScopeDescriptors)
        {
            if (await scopeManager.FindByNameAsync(descriptor.Name!) is null)
                await scopeManager.CreateAsync(descriptor);
        }

        var applicationManager = services.GetRequiredService<IOpenIddictApplicationManager>();
        if (await applicationManager.FindByClientIdAsync(Config.ClientId) is null)
            await applicationManager.CreateAsync(Config.ClientApplication);
    }
}
