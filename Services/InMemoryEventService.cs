using nafsibooking.Models;

namespace nafsibooking.Services;

public interface IEventService
{
    IReadOnlyList<Event> GetEvents(string? query = null, DateTime? date = null);
    Event? GetById(string id);
    BookingResult Book(string eventId, BookingRequest request);
    Event AddEvent(Event newEvent);
}

public class InMemoryEventService : IEventService
{
    private readonly List<Event> _events;

    public InMemoryEventService()
    {
        _events = BuildSeedData();
    }

    public IReadOnlyList<Event> GetEvents(string? query = null, DateTime? date = null)
    {
        IEnumerable<Event> results = _events;

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.Trim();
            results = results.Where(e =>
                e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.City.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.Venue.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        if (date.HasValue)
        {
            var targetDate = date.Value.Date;
            results = results.Where(e => e.Date.Date == targetDate);
        }

        return results
            .Where(e => e.Date >= DateTime.Today.AddDays(-1))
            .OrderBy(e => e.Date)
            .ToList();
    }

    public Event? GetById(string id)
    {
        return _events.FirstOrDefault(e => e.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public BookingResult Book(string eventId, BookingRequest request)
    {
        var ev = GetById(eventId) ?? throw new InvalidOperationException("Event not found.");

        if (!request.AcceptTerms)
        {
            throw new InvalidOperationException("Terms must be accepted.");
        }

        var tier = ev.Tiers.FirstOrDefault(t => t.Name.Equals(request.Tier, StringComparison.OrdinalIgnoreCase))
                   ?? throw new InvalidOperationException("Ticket tier not found.");

        if (request.Tickets <= 0 || request.Tickets > tier.Available)
        {
            throw new InvalidOperationException($"Please select between 1 and {tier.Available} tickets for this tier.");
        }

        var total = tier.Price * request.Tickets;
        var confirmation = $"{ev.Id[..Math.Min(3, ev.Id.Length)]}-{Random.Shared.Next(10000, 99999)}".ToUpperInvariant();

        return new BookingResult(confirmation, DateTime.UtcNow, total, tier.Name, request.Tickets);
    }

    public Event AddEvent(Event newEvent)
    {
        if (string.IsNullOrWhiteSpace(newEvent.Id))
        {
            throw new InvalidOperationException("Event id is required.");
        }

        if (_events.Any(e => e.Id.Equals(newEvent.Id, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("An event with this id already exists.");
        }

        _events.Add(newEvent);
        return newEvent;
    }

    private static List<Event> BuildSeedData()
    {
        return new List<Event>
        {
            new(
                id: "coastal-jazz-night",
                title: "Coastal Jazz Night",
                category: "Music",
                venue: "Harborfront Pavilion",
                city: "Seattle, WA",
                date: new DateTime(2026, 3, 14, 19, 30, 0, DateTimeKind.Local),
                basePrice: 120,
                description: "An intimate waterfront jazz experience featuring world-class performers and candlelit ambience.",
                highlights: new []
                {
                    "Sunset harbor views",
                    "Pop-up oyster and wine bar",
                    "Meet-and-greet with the quartet"
                },
                tiers: new []
                {
                    new TicketTier("General", 120, "Reserved seating with harbor views.", 120),
                    new TicketTier("Premium", 185, "Premium rows plus welcome drink.", 60),
                    new TicketTier("Lounge", 260, "Front-row lounge seats, tasting board, and concierge check-in.", 24)
                }
            ),
            new(
                id: "starlight-tech-2026",
                title: "Starlight Tech Conference",
                category: "Conference",
                venue: "Aurora Convention Center",
                city: "Austin, TX",
                date: new DateTime(2026, 5, 8, 9, 0, 0, DateTimeKind.Local),
                basePrice: 420,
                description: "Two days of keynotes, hands-on labs, and founder AMAs for builders and product teams.",
                highlights: new []
                {
                    "6 tracks including AI, product, and cloud ops",
                    "Hands-on prototyping lab",
                    "Evening rooftop social with live set"
                },
                tiers: new []
                {
                    new TicketTier("Standard", 420, "Full conference access, coffee, and lunches.", 350),
                    new TicketTier("Builder", 580, "Standard perks plus lab access and mentor hours.", 180),
                    new TicketTier("Executive", 890, "Reserved seating, hosted dinners, and green room access.", 80)
                }
            ),
            new(
                id: "skyline-theatre-premiere",
                title: "Skyline Theatre Premiere",
                category: "Theatre",
                venue: "Grand Skyline Theatre",
                city: "Chicago, IL",
                date: new DateTime(2026, 2, 20, 20, 0, 0, DateTimeKind.Local),
                basePrice: 95,
                description: "A modern retelling of a classic drama, staged with immersive lighting and live score.",
                highlights: new []
                {
                    "Live orchestra pit",
                    "Cast talkback after the show",
                    "Limited signed playbills"
                },
                tiers: new []
                {
                    new TicketTier("Balcony", 95, "Upper balcony seating.", 140),
                    new TicketTier("Orchestra", 145, "Center orchestra rows.", 110),
                    new TicketTier("Dress Circle", 210, "Premium dress circle, souvenir program, and lounge access.", 50)
                }
            )
        };
    }
}
