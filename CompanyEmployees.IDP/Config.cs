using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace CompanyEmployees.IDP;

public static class Config
{
    public const string ApiResourceName = "companyemployeeapi";
    public const string ApiScopeName = "companyemployeeapi.scope";
    public const string ClientId = "companyemployeeclient";

    public static IEnumerable<OpenIddictScopeDescriptor> ScopeDescriptors =>
    [
        new()
        {
            Name = Scopes.OpenId,
            DisplayName = "OpenID",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = Scopes.Profile,
            DisplayName = "User profile",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = Scopes.Address,
            DisplayName = "User address",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = Scopes.Roles,
            DisplayName = "User roles",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = "country",
            DisplayName = "User country",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = ApiScopeName,
            DisplayName = "CompanyEmployee API Scope",
            Resources = { ApiResourceName }
        },
        new()
        {
            Name = Scopes.OfflineAccess,
            DisplayName = "Offline access",
            Resources = { ApiResourceName }
        }
    ];

    public static OpenIddictApplicationDescriptor ClientApplication =>
        new()
        {
            ClientId = ClientId,
            ClientSecret = "CompanyEmployeeClientSecret",
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "CompanyEmployeeClient",
            RedirectUris =
            {
                new Uri("https://localhost:5010/signin-oidc")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://localhost:5010/signout-callback-oidc")
            },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.EndSession,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Prefixes.Scope + Scopes.Address,
                Permissions.Prefixes.Scope + "country",
                Permissions.Prefixes.Scope + ApiScopeName,
                Permissions.Prefixes.Scope + Scopes.OfflineAccess
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        };
}
