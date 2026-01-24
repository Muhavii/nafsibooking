namespace nafsibooking.Services;

public interface IAdminAuthService
{
    bool ValidateAdminPassword(string password);
    bool IsAdminAuthenticated(Microsoft.AspNetCore.Http.HttpContext context);
    void SignIn(Microsoft.AspNetCore.Http.HttpContext context);
    void SignOut(Microsoft.AspNetCore.Http.HttpContext context);
}

public class SimpleAdminAuthService : IAdminAuthService
{
    /// <remarks>
    /// SECURITY WARNING: This hardcoded password is for development only.
    /// In production, use environment variables or a secure configuration system (e.g., Azure Key Vault, AWS Secrets Manager).
    /// Example: var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? throw new InvalidOperationException("Admin password not configured");
    /// </remarks>
    private const string AdminPassword = "admin123";
    private const string AuthCookieName = "AdminAuth";

    public bool ValidateAdminPassword(string password)
    {
        return password == AdminPassword;
    }

    public bool IsAdminAuthenticated(Microsoft.AspNetCore.Http.HttpContext context)
    {
        return context.Request.Cookies.ContainsKey(AuthCookieName) && 
               context.Request.Cookies[AuthCookieName] == "true";
    }

    public void SignIn(Microsoft.AspNetCore.Http.HttpContext context)
    {
        context.Response.Cookies.Append(AuthCookieName, "true", new Microsoft.AspNetCore.Http.CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    public void SignOut(Microsoft.AspNetCore.Http.HttpContext context)
    {
        context.Response.Cookies.Delete(AuthCookieName);
    }
}
