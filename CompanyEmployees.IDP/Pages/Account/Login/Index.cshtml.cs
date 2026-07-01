using CompanyEmployees.IDP.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEmployees.IDP.Pages.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Index(SignInManager<User> signInManager) : PageModel
{
    public ViewModel View { get; set; } = new();

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet(string returnUrl)
    {
        Input.ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Input.Button != "login")
            return Redirect("~/");

        if (!ModelState.IsValid)
            return Page();

        var result = await signInManager.PasswordSignInAsync(
            Input.Username, Input.Password, Input.RememberLogin, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(Input.ReturnUrl) && Input.ReturnUrl.StartsWith('/'))
                return Redirect(Input.ReturnUrl);

            return Redirect("~/");
        }

        ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
        return Page();
    }
}
