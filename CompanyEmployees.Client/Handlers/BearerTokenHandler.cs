using System.Globalization;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CompanyEmployees.Client.Handlers;

public class BearerTokenHandler(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(accessToken))
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

        request.SetBearerToken(accessToken);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var tokenExpireAt = await httpContextAccessor.HttpContext.GetTokenAsync("expires_at");
        var expiresAtDateTimeOffset = DateTimeOffset.Parse(tokenExpireAt, CultureInfo.InvariantCulture);

        if(expiresAtDateTimeOffset.AddSeconds(-60).ToUniversalTime() > DateTime.UtcNow)
        {
            return await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        }

        var refreshResponse = await GetRefreshTokenFromIDP();
        var updatedToken = GetUpdateTokens(refreshResponse);

        var currentAuthenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        currentAuthenticationResult.Properties.StoreTokens(updatedToken);
        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, currentAuthenticationResult.Principal, currentAuthenticationResult.Properties);
        return refreshResponse.AccessToken;
    }

    private async Task<TokenResponse> GetRefreshTokenFromIDP()
    {
        var  idpClient = httpClientFactory.CreateClient(("IDPClient"));
        var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
        var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        var tokenResponse = await idpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = metaDataResponse.TokenEndpoint,
            ClientId = "companyemployeeclient",
            ClientSecret = "CompanyEmployeeClientSecret",
            RefreshToken = refreshToken
        });
        return tokenResponse;
    }

    private static IEnumerable<AuthenticationToken> GetUpdateTokens(TokenResponse refreshToken)
        =>
        [
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.IdToken,
                Value = refreshToken.IdentityToken
            },

            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = refreshToken.AccessToken
            },

            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = refreshToken.RefreshToken
            },

            new AuthenticationToken
            {
                Name = "expires_at",
                Value = DateTime.UtcNow.AddSeconds(refreshToken.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
            }
        ];
}
