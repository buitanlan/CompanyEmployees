using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CompanyEmployees.Client.Handlers;

public class BearerTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (string.IsNullOrWhiteSpace(accessToken))
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

        request.SetBearerToken(accessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}
