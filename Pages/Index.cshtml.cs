using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages;

public class IndexModel : PageModel
{
    private readonly IEventService _eventService;
    private readonly IAdminAuthService _authService;

    public IndexModel(IEventService eventService, IAdminAuthService authService)
    {
        _eventService = eventService;
        _authService = authService;
    }

    public IReadOnlyList<Event> Events { get; private set; } = Array.Empty<Event>();
    public bool IsAdminAuthenticated { get; private set; }

    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? DateFilter { get; set; }

    public void OnGet()
    {
        IsAdminAuthenticated = _authService.IsAdminAuthenticated(HttpContext);
        
        DateTime? date = null;
        if (DateTime.TryParse(DateFilter, out var parsed))
        {
            date = parsed;
        }

        Events = _eventService.GetEvents(Query, date);
    }

    public IActionResult OnPostLogout()
    {
        _authService.SignOut(HttpContext);
        return RedirectToPage();
    }
}
