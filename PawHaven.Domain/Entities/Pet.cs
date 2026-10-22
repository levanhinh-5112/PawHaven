using PawHaven.Domain.Enums;

namespace PawHaven.Domain.Entities;

public class Pet
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Species Species { get; private set; }
    public string? Breed { get; private set; }
    public int AgeInMonths { get; private set; }
    public PetStatus Status { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? PhotoUrl { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // Private parameterless constructor for EF Core
    private Pet()
    {
    }

    public Pet(
        string name,
        Species species,
        string description,
        int ageInMonths = 0,
        PetStatus status = PetStatus.Available,
        string? breed = null,
        string? photoUrl = null,
        DateTime? createdAtUtc = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Pet name cannot be empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Pet name cannot exceed 100 characters.", nameof(name));

        if (ageInMonths < 0)
            throw new ArgumentException("Pet age cannot be negative.", nameof(ageInMonths));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Pet description cannot be empty.", nameof(description));

        if (description.Length > 2000)
            throw new ArgumentException("Pet description cannot exceed 2000 characters.", nameof(description));

        if (breed != null && breed.Length > 100)
            throw new ArgumentException("Pet breed cannot exceed 100 characters.", nameof(breed));

        if (photoUrl != null && photoUrl.Length > 500)
            throw new ArgumentException("Photo URL cannot exceed 500 characters.", nameof(photoUrl));

        Name = name;
        Species = species;
        Description = description;
        AgeInMonths = ageInMonths;
        Status = status;
        Breed = breed;
        PhotoUrl = photoUrl;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
    }

    public void MarkAsAdopted()
    {
        Status = PetStatus.Adopted;
    }
}
