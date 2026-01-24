using System.ComponentModel.DataAnnotations;

namespace nafsibooking.Models;

public class TicketTier
{
    public TicketTier() { }

    public TicketTier(string name, decimal price, string description, int available)
    {
        Name = name;
        Price = price;
        Description = description;
        Available = available;
    }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Available { get; set; }
}

public class Event
{
    public Event() { }

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

    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal BasePrice { get; set; }
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<string> Highlights { get; set; } = new List<string>();
    public IReadOnlyList<TicketTier> Tiers { get; set; } = new List<TicketTier>();
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

    [Required(ErrorMessage = "Please accept the terms to continue.")]
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
