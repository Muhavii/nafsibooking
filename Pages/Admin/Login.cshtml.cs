using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Services;

namespace nafsibooking.Pages.Admin;

public class LoginModel : PageModel
{
    private readonly IAdminAuthService _authService;

    public LoginModel(IAdminAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required.";
            return Page();
        }

        if (_authService.ValidateAdminPassword(Password))
        {
            _authService.SignIn(HttpContext);
            return RedirectToPage("/Index");
        }

        ErrorMessage = "Invalid password.";
        return Page();
    }
}

