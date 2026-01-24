using System.ComponentModel.DataAnnotations;

namespace nafsibooking.Models;

public class PromoterRequest
{
    public PromoterRequest() { }

    public PromoterRequest(
        string id,
        string promoterName,
        string promoterEmail,
        string eventTitle,
        string eventCategory,
        string venue,
        string city,
        DateTime proposedDate,
        decimal estimatedAttendance,
        string description,
        DateTime submittedAt,
        string status = "pending")
    {
        Id = id;
        PromoterName = promoterName;
        PromoterEmail = promoterEmail;
        EventTitle = eventTitle;
        EventCategory = eventCategory;
        Venue = venue;
        City = city;
        ProposedDate = proposedDate;
        EstimatedAttendance = estimatedAttendance;
        Description = description;
        SubmittedAt = submittedAt;
        Status = status; // pending, approved, rejected
    }

    public string Id { get; set; } = string.Empty;
    public string PromoterName { get; set; } = string.Empty;
    public string PromoterEmail { get; set; } = string.Empty;
    public string EventTitle { get; set; } = string.Empty;
    public string EventCategory { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime ProposedDate { get; set; }
    public decimal EstimatedAttendance { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = "pending";
}
