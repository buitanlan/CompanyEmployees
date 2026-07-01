using System.Security.Claims;
using CompanyEmployees.IDP.Entities;
using Microsoft.AspNetCore.Identity;

namespace CompanyEmployees.IDP;

public static class SeedUserData
{
    public static async Task EnsureSeedDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        await CreateUserAsync(scope, "John", "Doe", "John Doe's Boulevard 323", "USA", "97a3aa4a-7a89-47f3-9814-74497fb92ccb", "JohnPassword", "Administrator", "john@mail.com");
        await CreateUserAsync(scope, "Jane", "Doe", "Jane Doe's Avenue 214", "USA", "64aca900-7bc7-4645-b291-38f1b7b5963c", "JanePassword", "Visitor", "jane@mail.com");
    }

    private static async Task CreateUserAsync(IServiceScope scope, string name, string lastName, string address, string country,
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
        if (result.Succeeded || result.Errors.All(e => e.Code == "DuplicateUserName"))
        {
            user = await userManager.FindByNameAsync(email) ?? user;
        }
        else
        {
            CheckResult(result);
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            result = await userManager.AddToRoleAsync(user, role);
            CheckResult(result);
        }

        var existingClaims = await userManager.GetClaimsAsync(user);
        var desiredClaims = new[]
        {
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.StreetAddress, address),
            new Claim("country", country)
        };

        foreach (var claim in desiredClaims.Where(c => !existingClaims.Any(e => e.Type == c.Type && e.Value == c.Value)))
        {
            result = await userManager.AddClaimAsync(user, claim);
            CheckResult(result);
        }
    }

    private static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);
    }
}
