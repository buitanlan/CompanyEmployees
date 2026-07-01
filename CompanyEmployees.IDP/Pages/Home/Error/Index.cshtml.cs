using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEmployees.IDP.Pages.Error;

[AllowAnonymous]
[SecurityHeaders]
public class Index : PageModel
{
    public ViewModel View { get; set; } = new();

    public void OnGet(string? error, string? errorDescription)
    {
        View.Error = error;
        View.ErrorDescription = errorDescription;
    }
}
