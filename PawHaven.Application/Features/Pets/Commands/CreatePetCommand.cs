using PawHaven.Domain.Enums;

namespace PawHaven.Application.Features.Pets.Commands;

/// <summary>
/// Command to create a new pet.
/// </summary>
public record CreatePetCommand(
    string Name,
    Species Species,
    string Description,
    int AgeInMonths = 0,
    string? Breed = null,
    string? PhotoUrl = null);
