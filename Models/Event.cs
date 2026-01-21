using System.ComponentModel.DataAnnotations;

namespace nafsibooking.Models;

public class TicketTier
{
    public TicketTier(string name, decimal price, string description, int available)
    {
        Name = name;
        Price = price;
        Description = description;
        Available = available;
    }

    public string Name { get; }
    public decimal Price { get; }
    public string Description { get; }
    public int Available { get; }
}

public class Event
{
    public Event(
        string id,
        string title,
        string category,
        string venue,
        string city,
        DateTime date,
        decimal basePrice,
        string description,
        IReadOnlyList<string> highlights,
        IReadOnlyList<TicketTier> tiers)
    {
        Id = id;
        Title = title;
        Category = category;
        Venue = venue;
        City = city;
        Date = date;
        BasePrice = basePrice;
        Description = description;
        Highlights = highlights;
        Tiers = tiers;
    }

    public string Id { get; }
    public string Title { get; }
    public string Category { get; }
    public string Venue { get; }
    public string City { get; }
    public DateTime Date { get; }
    public decimal BasePrice { get; }
    public string Description { get; }
    public IReadOnlyList<string> Highlights { get; }
    public IReadOnlyList<TicketTier> Tiers { get; }
}

public class BookingRequest
{
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string? FullName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(40)]
    public string? Tier { get; set; }

    [Range(1, 10)]
    public int Tickets { get; set; } = 1;

    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the terms to continue.")]
    public bool AcceptTerms { get; set; }
}

public class BookingResult
{
    public BookingResult(string confirmationCode, DateTime createdAt, decimal total, string tierName, int tickets)
    {
        ConfirmationCode = confirmationCode;
        CreatedAt = createdAt;
        Total = total;
        TierName = tierName;
        Tickets = tickets;
    }

    public string ConfirmationCode { get; }
    public DateTime CreatedAt { get; }
    public decimal Total { get; }
    public string TierName { get; }
    public int Tickets { get; }
}
