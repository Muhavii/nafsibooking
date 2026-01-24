using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Services;

namespace nafsibooking.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly IUserAuthService _auth;

    public LogoutModel(IUserAuthService auth)
    {
        _auth = auth;
    }

    public IActionResult OnPost()
    {
        _auth.SignOut(HttpContext);
        return RedirectToPage("/Index");
    }
}
