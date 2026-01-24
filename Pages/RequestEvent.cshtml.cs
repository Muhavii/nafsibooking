using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nafsibooking.Models;
using nafsibooking.Services;

namespace nafsibooking.Pages;

public class RequestEventModel : PageModel
{
    private readonly IPromoterRequestService _requestService;

    public RequestEventModel(IPromoterRequestService requestService)
    {
        _requestService = requestService;
    }

    [BindProperty]
    public PromoterEventInput Input { get; set; } = new();

    public string? SuccessMessage { get; private set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.ProposedDate < DateTime.Now.AddDays(7))
        {
            ModelState.AddModelError(nameof(Input.ProposedDate), "Event must be at least 7 days in the future.");
            return Page();
        }

        try
        {
            var id = Guid.NewGuid().ToString()[..8];
            var request = new PromoterRequest(
                id: id,
                promoterName: Input.PromoterName!.Trim(),
                promoterEmail: Input.PromoterEmail!.Trim(),
                eventTitle: Input.EventTitle!.Trim(),
                eventCategory: Input.EventCategory!.Trim(),
                venue: Input.Venue!.Trim(),
                city: Input.City!.Trim(),
                proposedDate: Input.ProposedDate,
                estimatedAttendance: Input.EstimatedAttendance,
                description: Input.Description!.Trim(),
                submittedAt: DateTime.UtcNow);

            _requestService.SubmitRequest(request);
            SuccessMessage = "Your event request has been submitted! We'll review it and contact you soon.";
            Input = new();
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public class PromoterEventInput
    {
        [Required]
        [Display(Name = "Your Name")]
        public string? PromoterName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string? PromoterEmail { get; set; }

        [Required]
        [Display(Name = "Event Title")]
        public string? EventTitle { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? EventCategory { get; set; }

        [Required]
        [Display(Name = "Proposed Date")]
        public DateTime ProposedDate { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string? Venue { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal EstimatedAttendance { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
