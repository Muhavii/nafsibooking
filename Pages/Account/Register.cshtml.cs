using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Services;

namespace nafsibooking.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IUserService _users;
    private readonly IUserAuthService _auth;

    public RegisterModel(IUserService users, IUserAuthService auth)
    {
        _users = users;
        _auth = auth;
    }

    [BindProperty]
    public RegisterInput Input { get; set; } = new();

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

        if (!string.Equals(Input.Password, Input.ConfirmPassword))
        {
            ModelState.AddModelError(nameof(Input.ConfirmPassword), "Passwords do not match.");
            return Page();
        }

        try
        {
            var user = _users.Register(Input.Email!, Input.DisplayName!, Input.Password!);
            _auth.SignIn(HttpContext, user);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }

    public class RegisterInput
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string? DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
