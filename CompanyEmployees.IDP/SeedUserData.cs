using System.Security.Claims;
using CompanyEmployees.IDP.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace CompanyEmployees.IDP;

public class SeedUserData
{
    public static void EnsureSeedData(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddNpgsql<UserContext>(connectionString);

        services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        {
            CreateUser(scope, "John", "Doe", "John Doe's Boulevard 323", "USA", "97a3aa4a-7a89-47f3-9814-74497fb92ccb", "JohnPassword", "Administrator", "john@mail.com");
            CreateUser(scope, "Jane", "Doe", "Jane Doe's Avenue 214", "USA", "64aca900-7bc7-4645-b291-38f1b7b5963c", "JanePassword", "Visitor", "jane@mail.com");
        }
    }

    public static async void CreateUser(IServiceScope scope, string name, string lastName, string address, string country,
        string id, string password, string role, string email)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByNameAsync(email);

        user ??= new User
        {
            UserName = email,
            Email = email,
            FirstName = name,
            LastName = lastName,
            Address = address,
            Country = country,
            Id = id
        };
        var result = await userManager.CreateAsync(user, password);
        CheckResult(result);

        result = await userManager.AddToRoleAsync(user, role);
        CheckResult(result);

        result = await userManager.AddClaimsAsync(user, new Claim[]
        {
            new(JwtClaimTypes.GivenName, user.FirstName),
            new(JwtClaimTypes.FamilyName, user.LastName),
            new(JwtClaimTypes.Role, role),
            new(JwtClaimTypes.Address, address),
            new("country", country)
        });
        CheckResult(result);
    }

    public static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }
}
