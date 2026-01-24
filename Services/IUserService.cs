using nafsibooking.Models;
using nafsibooking.Data;
using Microsoft.EntityFrameworkCore;

namespace nafsibooking.Services;

public interface IUserService
{
    User Register(string email, string displayName, string password);
    User? GetByEmail(string email);
    User? GetById(string id);
}

public class DatabaseUserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserAuthService _auth;

    public DatabaseUserService(ApplicationDbContext context, IUserAuthService auth)
    {
        _context = context;
        _auth = auth;
    }

    public User Register(string email, string displayName, string password)
    {
        email = email.Trim().ToLowerInvariant();
        if (_context.Users.Any(u => u.Email == email))
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        var (hash, salt) = _auth.HashPassword(password);
        var user = new User
        {
            Email = email,
            DisplayName = displayName.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? GetByEmail(string email)
    {
        email = email.Trim().ToLowerInvariant();
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? GetById(string id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
}
