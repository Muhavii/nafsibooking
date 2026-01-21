using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages.Admin.Events;

public class CreateModel : PageModel
{
    private readonly IEventService _eventService;

    public CreateModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    public IReadOnlyList<Event> ExistingEvents { get; private set; } = Array.Empty<Event>();

    [BindProperty]
    public EventInput Input { get; set; } = new();

    [BindProperty]
    public List<TierInput> Tiers { get; set; } = new() { new TierInput(), new TierInput(), new TierInput() };

    public string? SuccessMessage { get; private set; }

    public void OnGet()
    {
        ExistingEvents = _eventService.GetEvents();
        EnsureTierSlots();
    }

    public IActionResult OnPost()
    {
        Tiers ??= new List<TierInput>();
        ExistingEvents = _eventService.GetEvents();
        EnsureTierSlots();

        var tiers = Tiers ?? new List<TierInput>();
        Tiers = tiers;

        var usableTiers = tiers
            .Where(t => t != null && !string.IsNullOrWhiteSpace(t.Name))
            .Select(t => new TicketTier(
                t!.Name!.Trim(),
                t.Price,
                t.Description ?? string.Empty,
                t.Available <= 0 ? 1 : t.Available))
            .ToList();

        if (!usableTiers.Any())
        {
            ModelState.AddModelError(string.Empty, "Add at least one ticket tier with a name.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Input.Title))
        {
            ModelState.AddModelError(nameof(Input.Title), "Title is required.");
            return Page();
        }

        var highlights = (Input.HighlightsRaw ?? string.Empty)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        var basePrice = Input.BasePrice > 0 ? Input.BasePrice : usableTiers.Min(t => t.Price);

        var title = Input.Title!.Trim();
        var id = Slugify(title);
        var eventDate = Input.StartsAt ?? DateTime.Now.AddDays(7);

        var newEvent = new Event(
            id: id,
            title: title,
            category: Input.Category?.Trim() ?? "General",
            venue: Input.Venue?.Trim() ?? "TBD",
            city: Input.City?.Trim() ?? "",
            date: eventDate,
            basePrice: basePrice,
            description: Input.Description ?? string.Empty,
            highlights: highlights,
            tiers: usableTiers);

        try
        {
            _eventService.AddEvent(newEvent);
            SuccessMessage = $"Event '{newEvent.Title}' created.";
            ModelState.Clear();
            Input = new EventInput { BasePrice = basePrice, StartsAt = Input.StartsAt };
            Tiers = new() { new TierInput(), new TierInput(), new TierInput() };
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        ExistingEvents = _eventService.GetEvents();
        return Page();
    }

    private static string Slugify(string text)
    {
        var safe = new string(text
            .ToLowerInvariant()
            .Select(c => char.IsLetterOrDigit(c) ? c : '-')
            .ToArray());

        while (safe.Contains("--"))
        {
            safe = safe.Replace("--", "-");
        }

        safe = safe.Trim('-');
        return string.IsNullOrWhiteSpace(safe) ? $"event-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}" : safe;
    }

    private void EnsureTierSlots()
    {
        while (Tiers.Count < 3)
        {
            Tiers.Add(new TierInput());
        }
    }

    public class EventInput
    {
        [Required]
        [StringLength(120)]
        public string? Title { get; set; }

        [Required]
        [StringLength(60)]
        public string? Category { get; set; }

        [Required]
        [StringLength(120)]
        public string? Venue { get; set; }

        [Required]
        [StringLength(80)]
        public string? City { get; set; }

        [Required]
        public DateTime? StartsAt { get; set; }

        [Range(0, 100000)]
        public decimal BasePrice { get; set; }

        [Required]
        [StringLength(400)]
        public string? Description { get; set; }

        public string? HighlightsRaw { get; set; }
    }

    public class TierInput
    {
        [StringLength(60)]
        public string? Name { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Range(0, 5000)]
        public int Available { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
