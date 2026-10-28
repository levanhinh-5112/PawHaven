using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.DTOs.Extensions;
using PawHaven.Application.Exceptions;

namespace PawHaven.Application.Features.Pets.Queries;

/// <summary>
/// Handler for retrieving a pet by its ID.
/// </summary>
public class GetPetByIdQueryHandler : IQueryHandler<GetPetByIdQuery, PetDto>
{
    private readonly IPetRepository _petRepository;

    /// <summary>
    /// Initializes a new instance of the GetPetByIdQueryHandler class.
    /// </summary>
    /// <param name="petRepository">The pet repository.</param>
    /// <exception cref="ArgumentNullException">Thrown when petRepository is null.</exception>
    public GetPetByIdQueryHandler(IPetRepository petRepository)
    {
        _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
    }

    /// <summary>
    /// Handles the query to retrieve a pet by its ID.
    /// </summary>
    /// <param name="query">The query containing the pet ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The pet DTO if found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when query is null.</exception>
    /// <exception cref="NotFoundException">Thrown when the pet is not found.</exception>
    public async Task<PetDto> HandleAsync(GetPetByIdQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Retrieve pet from repository
        var pet = await _petRepository.GetByIdAsync(query.PetId, cancellationToken);

        // Throw exception if not found
        if (pet is null)
        {
            throw new NotFoundException("Pet", query.PetId);
        }

        // Map to DTO and return
        return pet.ToPetDto();
    }
}
