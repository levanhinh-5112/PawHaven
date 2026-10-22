using PawHaven.Domain.Enums;

namespace PawHaven.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // Private parameterless constructor for EF Core
    private User()
    {
    }

    public User(
        string email,
        string passwordHash,
        string fullName,
        UserRole role = UserRole.Public,
        DateTime? createdAtUtc = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (email.Length > 256)
            throw new ArgumentException("Email cannot exceed 256 characters.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        if (fullName.Length > 150)
            throw new ArgumentException("Full name cannot exceed 150 characters.", nameof(fullName));

        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
    }
}
