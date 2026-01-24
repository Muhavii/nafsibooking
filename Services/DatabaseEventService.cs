using nafsibooking.Models;
using nafsibooking.Data;
using Microsoft.EntityFrameworkCore;

namespace nafsibooking.Services;

public class DatabaseEventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public DatabaseEventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Event> GetEvents(string? query = null, DateTime? date = null)
    {
        IQueryable<Event> results = _context.Events;

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.Trim();
            var lowerQuery = query.ToLower();
            results = results.Where(e =>
                e.Title.ToLower().Contains(lowerQuery) ||
                e.City.ToLower().Contains(lowerQuery) ||
                e.Venue.ToLower().Contains(lowerQuery));
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
        return _context.Events.FirstOrDefault(e => e.Id.ToLower() == id.ToLower());
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

        if (_context.Events.Any(e => e.Id.ToLower() == newEvent.Id.ToLower()))
        {
            throw new InvalidOperationException("An event with this id already exists.");
        }

        _context.Events.Add(newEvent);
        _context.SaveChanges();
        return newEvent;
    }
}
