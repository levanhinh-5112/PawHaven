namespace PawHaven.Application.Features.Pets.Queries;

/// <summary>
/// Query to retrieve a pet by its ID.
/// </summary>
/// <param name="PetId">The ID of the pet to retrieve.</param>
public record GetPetByIdQuery(int PetId);
