using PawHaven.Domain.Enums;

namespace PawHaven.Application.Features.Pets.Commands;

/// <summary>
/// Command to update an existing pet.
/// </summary>
public record UpdatePetCommand(
    int Id,
    string Name,
    Species Species,
    string Description,
    int AgeInMonths = 0,
    string? Breed = null,
    string? PhotoUrl = null);
