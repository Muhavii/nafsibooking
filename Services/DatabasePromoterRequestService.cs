using nafsibooking.Models;
using nafsibooking.Data;
using Microsoft.EntityFrameworkCore;

namespace nafsibooking.Services;

public class DatabasePromoterRequestService : IPromoterRequestService
{
    private readonly ApplicationDbContext _context;

    public DatabasePromoterRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public PromoterRequest SubmitRequest(PromoterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            throw new InvalidOperationException("Request id is required.");

        _context.PromoterRequests.Add(request);
        _context.SaveChanges();
        return request;
    }

    public IReadOnlyList<PromoterRequest> GetRequests(string? status = null)
    {
        IQueryable<PromoterRequest> results = _context.PromoterRequests;
        
        if (!string.IsNullOrWhiteSpace(status))
        {
            results = results.Where(r => r.Status.ToLower() == status.ToLower());
        }
        
        return results.OrderByDescending(r => r.SubmittedAt).ToList();
    }

    public PromoterRequest? GetById(string id)
    {
        return _context.PromoterRequests.FirstOrDefault(r => r.Id.ToLower() == id.ToLower());
    }

    public void UpdateStatus(string id, string status)
    {
        var request = GetById(id);
        if (request != null)
        {
            request.Status = status;
            _context.SaveChanges();
        }
    }
}
