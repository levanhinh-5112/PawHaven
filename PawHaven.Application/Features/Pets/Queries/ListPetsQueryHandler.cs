using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.DTOs.Extensions;

namespace PawHaven.Application.Features.Pets.Queries;

/// <summary>
/// Handler for retrieving a paginated list of pets with optional filtering and sorting.
/// </summary>
public class ListPetsQueryHandler : IQueryHandler<ListPetsQuery, PagedResult<PetDto>>
{
    private readonly IPetRepository _petRepository;

    /// <summary>
    /// Initializes a new instance of the ListPetsQueryHandler class.
    /// </summary>
    /// <param name="petRepository">The pet repository.</param>
    /// <exception cref="ArgumentNullException">Thrown when petRepository is null.</exception>
    public ListPetsQueryHandler(IPetRepository petRepository)
    {
        _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
    }

    /// <summary>
    /// Handles the query to retrieve a paginated list of pets.
    /// </summary>
    /// <param name="query">The query containing pagination and filter parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A PagedResult containing PetDtos and pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when query is null.</exception>
    /// <exception cref="ArgumentException">Thrown when query parameters fail validation.</exception>
    public async Task<PagedResult<PetDto>> HandleAsync(ListPetsQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Validate query parameters
        query.Validate();

        // Convert query to filter object
        var filter = ListPetsQueryFilter.FromQuery(query);

        // Retrieve paginated results from repository
        var pagedPets = await _petRepository.ListAsync(filter, query.Page, query.PageSize, cancellationToken);

        // Map Pet collection to PetDto collection
        var petDtos = pagedPets.Items.Select(pet => pet.ToPetDto()).ToList();

        // Return paginated result with mapped DTOs
        return new PagedResult<PetDto>(petDtos, pagedPets.Page, pagedPets.PageSize, pagedPets.TotalCount);
    }
}
