using System.Security.Cryptography;
using nafsibooking.Models;
using Microsoft.AspNetCore.Http;

namespace nafsibooking.Services;

public interface IUserAuthService
{
    (string hash, string salt) HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
    void SignIn(HttpContext context, User user);
    void SignOut(HttpContext context);
    bool IsAuthenticated(HttpContext context);
    string? GetUserId(HttpContext context);
}

public class SimpleUserAuthService : IUserAuthService
{
    private const string CookieName = "UserAuth";
    private const int Iterations = 100_000;
    private const int SaltSize = 16; // 128-bit
    private const int KeySize = 32; // 256-bit

    public (string hash, string salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        return (Convert.ToBase64String(key), Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, KeySize);
        return Convert.ToBase64String(key) == hash;
    }

    public void SignIn(HttpContext context, User user)
    {
        context.Response.Cookies.Append(CookieName, user.Id, new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    public void SignOut(HttpContext context)
    {
        context.Response.Cookies.Delete(CookieName);
    }

    public bool IsAuthenticated(HttpContext context)
    {
        return context.Request.Cookies.ContainsKey(CookieName);
    }

    public string? GetUserId(HttpContext context)
    {
        return context.Request.Cookies.TryGetValue(CookieName, out var id) ? id : null;
    }
}
