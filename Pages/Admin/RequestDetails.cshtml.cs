using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages.Admin;

public class RequestDetailsModel : PageModel
{
    private readonly IPromoterRequestService _requestService;
    private readonly IEventService _eventService;
    private readonly IAdminAuthService _authService;

    public RequestDetailsModel(
        IPromoterRequestService requestService,
        IEventService eventService,
        IAdminAuthService authService)
    {
        _requestService = requestService;
        _eventService = eventService;
        _authService = authService;
    }

    public PromoterRequest? PromoterRequestData { get; set; }

    public IActionResult OnGet(string id)
    {
        if (!_authService.IsAdminAuthenticated(HttpContext))
        {
            return RedirectToPage("Login");
        }

        PromoterRequestData = _requestService.GetById(id);
        if (PromoterRequestData == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPostApprove(string requestId)
    {
        if (!_authService.IsAdminAuthenticated(HttpContext))
        {
            return RedirectToPage("Login");
        }

        var request = _requestService.GetById(requestId);
        if (request != null && request.Status == "pending")
        {
            _requestService.UpdateStatus(requestId, "approved");
            // Optionally auto-create the event here
        }

        return RedirectToPage("Dashboard");
    }

    public IActionResult OnPostReject(string requestId)
    {
        if (!_authService.IsAdminAuthenticated(HttpContext))
        {
            return RedirectToPage("Login");
        }

        var request = _requestService.GetById(requestId);
        if (request != null && request.Status == "pending")
        {
            _requestService.UpdateStatus(requestId, "rejected");
        }

        return RedirectToPage("Dashboard");
    }
}
