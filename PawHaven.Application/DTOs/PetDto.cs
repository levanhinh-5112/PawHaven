using PawHaven.Domain.Enums;

namespace PawHaven.Application.DTOs;

/// <summary>
/// Data transfer object for Pet entity.
/// Used for API responses and read operations.
/// </summary>
public class PetDto
{
    /// <summary>
    /// Gets the unique identifier of the pet.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name of the pet.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the species of the pet.
    /// </summary>
    public Species Species { get; init; }

    /// <summary>
    /// Gets the breed of the pet (optional).
    /// </summary>
    public string? Breed { get; init; }

    /// <summary>
    /// Gets the age of the pet in months.
    /// </summary>
    public int AgeInMonths { get; init; }

    /// <summary>
    /// Gets the current status of the pet.
    /// </summary>
    public PetStatus Status { get; init; }

    /// <summary>
    /// Gets the description of the pet.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the photo URL of the pet (optional).
    /// </summary>
    public string? PhotoUrl { get; init; }

    /// <summary>
    /// Gets the UTC timestamp when the pet was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; init; }
}
