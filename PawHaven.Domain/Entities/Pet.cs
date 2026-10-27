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
        ValidateProperties(name, species, description, ageInMonths, breed, photoUrl);
        ValidateStatus(status);

        Name = name;
        Species = species;
        Description = description;
        AgeInMonths = ageInMonths;
        Status = status;
        Breed = breed;
        PhotoUrl = photoUrl;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
    }

    public static Pet CreateSeed(
        int id,
        string name,
        Species species,
        string description,
        int ageInMonths = 0,
        PetStatus status = PetStatus.Available,
        string? breed = null,
        string? photoUrl = null,
        DateTime? createdAtUtc = null)
    {
        var pet = new Pet(name, species, description, ageInMonths, status, breed, photoUrl, createdAtUtc);
        pet.Id = id;
        return pet;
    }

    public void MarkAsAdopted()
    {
        Status = PetStatus.Adopted;
    }

    public void Update(
        string name,
        Species species,
        string description,
        int ageInMonths = 0,
        string? breed = null,
        string? photoUrl = null)
    {
        ValidateProperties(name, species, description, ageInMonths, breed, photoUrl);

        // Update properties (Status remains unchanged)
        Name = name;
        Species = species;
        Description = description;
        AgeInMonths = ageInMonths;
        Breed = breed;
        PhotoUrl = photoUrl;
    }

    /// <summary>
    /// Validates all updateable pet properties.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    private void ValidateProperties(
        string name,
        Species species,
        string description,
        int ageInMonths,
        string? breed,
        string? photoUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Pet name cannot be empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Pet name cannot exceed 100 characters.", nameof(name));

        if (!Enum.IsDefined(typeof(Species), species))
            throw new ArgumentException("Invalid pet species.", nameof(species));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Pet description cannot be empty.", nameof(description));

        if (description.Length > 2000)
            throw new ArgumentException("Pet description cannot exceed 2000 characters.", nameof(description));

        if (ageInMonths < 0)
            throw new ArgumentException("Pet age cannot be negative.", nameof(ageInMonths));

        if (ageInMonths > 360)
            throw new ArgumentException("Pet age cannot exceed 360 months.", nameof(ageInMonths));

        if (breed != null && breed.Length > 100)
            throw new ArgumentException("Pet breed cannot exceed 100 characters.", nameof(breed));

        if (photoUrl != null && photoUrl.Length > 500)
            throw new ArgumentException("Photo URL cannot exceed 500 characters.", nameof(photoUrl));
    }

    /// <summary>
    /// Validates the pet status enum.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when status is not a valid enum value.</exception>
    private void ValidateStatus(PetStatus status)
    {
        if (!Enum.IsDefined(typeof(PetStatus), status))
            throw new ArgumentException("Invalid pet status.", nameof(status));
    }
}
