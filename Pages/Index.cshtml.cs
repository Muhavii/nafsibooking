using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages;

public class IndexModel : PageModel
{
    private readonly IEventService _eventService;

    public IndexModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    public IReadOnlyList<Event> Events { get; private set; } = Array.Empty<Event>();

    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? DateFilter { get; set; }

    public void OnGet()
    {
        DateTime? date = null;
        if (DateTime.TryParse(DateFilter, out var parsed))
        {
            date = parsed;
        }

        Events = _eventService.GetEvents(Query, date);
    }
}
