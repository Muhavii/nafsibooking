using System.ComponentModel.DataAnnotations;

namespace nafsibooking.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string DisplayName { get; set; } = string.Empty;

    // Password storage (PBKDF2)
    public string PasswordHash { get; set; } = string.Empty; // base64
    public string PasswordSalt { get; set; } = string.Empty; // base64

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}