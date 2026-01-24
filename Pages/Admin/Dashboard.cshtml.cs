using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly IPromoterRequestService _requestService;
    private readonly IAdminAuthService _authService;

    public DashboardModel(IPromoterRequestService requestService, IAdminAuthService authService)
    {
        _requestService = requestService;
        _authService = authService;
    }

    public IReadOnlyList<PromoterRequest> Requests { get; set; } = Array.Empty<PromoterRequest>();
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }

    public IActionResult OnGet()
    {
        if (!_authService.IsAdminAuthenticated(HttpContext))
        {
            return RedirectToPage("Login");
        }

        Requests = _requestService.GetRequests();
        PendingCount = _requestService.GetRequests("pending").Count;
        ApprovedCount = _requestService.GetRequests("approved").Count;
        RejectedCount = _requestService.GetRequests("rejected").Count;

        return Page();
    }

    public IActionResult OnPostLogout()
    {
        _authService.SignOut(HttpContext);
        return RedirectToPage("/Index");
    }
}
