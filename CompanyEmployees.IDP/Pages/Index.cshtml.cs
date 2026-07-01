using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEmployees.IDP.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public string Version { get; set; } = string.Empty;

    public void OnGet()
    {
        Version = typeof(IndexModel).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion
            .Split('+')
            .FirstOrDefault() ?? "unknown";
    }
}
