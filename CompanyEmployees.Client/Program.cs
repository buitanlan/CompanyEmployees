using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("APIClient", client => 
{ 
    client.BaseAddress = new Uri("https://localhost:5001/"); 
    client.DefaultRequestHeaders.Clear(); 
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json"); 
});

builder.Services.AddHttpClient("IDPClient", client => 
{ 
    client.BaseAddress = new Uri("https://localhost:5005/"); 
    client.DefaultRequestHeaders.Clear(); 
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json"); 
}); 

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(opt => 
{ 
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; 
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.AccessDeniedPath = "/Auth/AccessDenied";
}) 
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt => 
{ 
    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
    opt.Authority = "https://localhost:5005"; 
    opt.ClientId = "companyemployeeclient"; 
    opt.ResponseType = OpenIdConnectResponseType.Code; 
    opt.SaveTokens = true; 
    opt.ClientSecret = "CompanyEmployeeClientSecret"; 
    opt.UsePkce = true; 
    opt.GetClaimsFromUserInfoEndpoint = true;
    opt.ClaimActions.DeleteClaim("sid");
    opt.ClaimActions.DeleteClaim("idp");
    opt.Scope.Add("address");
    opt.Scope.Add("roles"); 
    opt.ClaimActions.MapUniqueJsonKey("role", "role");
    opt.Scope.Add("companyemployeeapi.scope");
    opt.TokenValidationParameters = new TokenValidationParameters 
    { 
        RoleClaimType = JwtClaimTypes.Role 
    }; 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
