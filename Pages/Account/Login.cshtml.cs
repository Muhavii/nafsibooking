using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Services;

namespace nafsibooking.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _users;
    private readonly IUserAuthService _auth;

    public LoginModel(IUserService users, IUserAuthService auth)
    {
        _users = users;
        _auth = auth;
    }

    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existing = _users.GetByEmail(Input.Email!);
        if (existing == null || !_auth.VerifyPassword(Input.Password!, existing.PasswordHash, existing.PasswordSalt))
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        _auth.SignIn(HttpContext, existing);
        return RedirectToPage("/Index");
    }

    public class LoginInput
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
