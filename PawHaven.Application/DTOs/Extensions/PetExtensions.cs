using PawHaven.Application.DTOs;
using PawHaven.Domain.Entities;

namespace PawHaven.Application.DTOs.Extensions;

/// <summary>
/// Extension methods for mapping Pet entities to DTOs.
/// </summary>
public static class PetExtensions
{
    /// <summary>
    /// Maps a Pet entity to a PetDto.
    /// </summary>
    /// <param name="pet">The Pet entity to map.</param>
    /// <returns>A PetDto containing the pet's data.</returns>
    public static PetDto ToPetDto(this Pet pet)
    {
        return new PetDto
        {
            Id = pet.Id,
            Name = pet.Name,
            Species = pet.Species,
            Breed = pet.Breed,
            AgeInMonths = pet.AgeInMonths,
            Status = pet.Status,
            Description = pet.Description,
            PhotoUrl = pet.PhotoUrl,
            CreatedAtUtc = pet.CreatedAtUtc
        };
    }
}
