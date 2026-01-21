using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages.Events;

public class DetailsModel : PageModel
{
    private readonly IEventService _eventService;

    public DetailsModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    public Event? Event { get; private set; }

    [BindProperty]
    public BookingRequest Booking { get; set; } = new();

    public BookingResult? Result { get; private set; }
    public string? Error { get; private set; }

    public IActionResult OnGet(string id)
    {
        Event = _eventService.GetById(id);
        if (Event is null)
        {
            return NotFound();
        }

        Booking.Tier = Event.Tiers.First().Name;
        return Page();
    }

    public IActionResult OnPost(string id)
    {
        Event = _eventService.GetById(id);
        if (Event is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            Result = _eventService.Book(id, Booking);
            ModelState.Clear();
            Booking = new BookingRequest
            {
                Tier = Event.Tiers.First().Name,
                Tickets = 1,
                AcceptTerms = false
            };
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return Page();
    }
}
