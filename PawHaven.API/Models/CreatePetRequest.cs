using PawHaven.Domain.Enums;

namespace PawHaven.API.Models;

/// <summary>
/// Request model for creating a new pet.
/// </summary>
public class CreatePetRequest
{
    /// <summary>
    /// Gets or sets the name of the pet.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the species of the pet.
    /// </summary>
    public Species Species { get; set; }

    /// <summary>
    /// Gets or sets the description of the pet.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the age of the pet in months.
    /// </summary>
    public int AgeInMonths { get; set; }

    /// <summary>
    /// Gets or sets the breed of the pet (optional).
    /// </summary>
    public string? Breed { get; set; }

    /// <summary>
    /// Gets or sets the photo URL of the pet (optional).
    /// </summary>
    public string? PhotoUrl { get; set; }
}
