using nafsibooking.Models;

namespace nafsibooking.Services;

public interface IPromoterRequestService
{
    PromoterRequest SubmitRequest(PromoterRequest request);
    IReadOnlyList<PromoterRequest> GetRequests(string? status = null);
    PromoterRequest? GetById(string id);
    void UpdateStatus(string id, string status);
}

public class InMemoryPromoterRequestService : IPromoterRequestService
{
    private readonly List<PromoterRequest> _requests = new();

    public PromoterRequest SubmitRequest(PromoterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            throw new InvalidOperationException("Request id is required.");

        _requests.Add(request);
        return request;
    }

    public IReadOnlyList<PromoterRequest> GetRequests(string? status = null)
    {
        var results = _requests.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(status))
        {
            results = results.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }
        return results.OrderByDescending(r => r.SubmittedAt).ToList();
    }

    public PromoterRequest? GetById(string id)
    {
        return _requests.FirstOrDefault(r => r.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public void UpdateStatus(string id, string status)
    {
        var request = GetById(id);
        if (request != null)
        {
            request.Status = status;
        }
    }
}
