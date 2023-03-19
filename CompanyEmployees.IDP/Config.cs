using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace CompanyEmployees.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };
    
    public static IEnumerable<ApiResource> Apis => 
        new ApiResource[] 
            { }; 

    public static IEnumerable<Client> Clients =>
        new Client[] 
        { 
            new()
            { 
                ClientName = "CompanyEmployeeClient", 
                ClientId = "companyemployeeclient", 
                AllowedGrantTypes = GrantTypes.Code, 
                RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" }, 
                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                    IdentityServerConstants.StandardScopes.Profile }, 
                ClientSecrets = { new Secret("CompanyEmployeeClientSecret".Sha512()) }, 
                RequirePkce = true,  
                RequireConsent = true,
                PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" } 

            } 
        };
}