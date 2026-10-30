using Microsoft.AspNetCore.Mvc;
using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.Features.Pets.Commands;
using PawHaven.Application.Features.Pets.Queries;
using PawHaven.API.Models;
using PawHaven.Domain.Enums;

namespace PawHaven.API.Controllers;

/// <summary>
/// Controller for managing pet operations.
/// Exposes REST endpoints for CRUD operations on pets.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly ICommandHandler<CreatePetCommand, PetDto> _createPetHandler;
    private readonly ICommandHandler<UpdatePetCommand, PetDto> _updatePetHandler;
    private readonly ICommandHandler<DeletePetCommand> _deletePetHandler;
    private readonly IQueryHandler<GetPetByIdQuery, PetDto> _getPetByIdHandler;
    private readonly IQueryHandler<ListPetsQuery, PagedResult<PetDto>> _listPetsHandler;

    public PetsController(
        ICommandHandler<CreatePetCommand, PetDto> createPetHandler,
        ICommandHandler<UpdatePetCommand, PetDto> updatePetHandler,
        ICommandHandler<DeletePetCommand> deletePetHandler,
        IQueryHandler<GetPetByIdQuery, PetDto> getPetByIdHandler,
        IQueryHandler<ListPetsQuery, PagedResult<PetDto>> listPetsHandler)
    {
        _createPetHandler = createPetHandler ?? throw new ArgumentNullException(nameof(createPetHandler));
        _updatePetHandler = updatePetHandler ?? throw new ArgumentNullException(nameof(updatePetHandler));
        _deletePetHandler = deletePetHandler ?? throw new ArgumentNullException(nameof(deletePetHandler));
        _getPetByIdHandler = getPetByIdHandler ?? throw new ArgumentNullException(nameof(getPetByIdHandler));
        _listPetsHandler = listPetsHandler ?? throw new ArgumentNullException(nameof(listPetsHandler));
    }

    /// <summary>
    /// Creates a new pet.
    /// </summary>
    /// <param name="request">The pet creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created pet with 201 Created status and Location header.</returns>
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<PetDto>> CreatePet(
        [FromBody] CreatePetRequest request,
        CancellationToken cancellationToken)
    {
        // TODO: Implement authentication (PAW-31)

        var command = new CreatePetCommand(
            request.Name,
            request.Species,
            request.Description,
            request.AgeInMonths,
            request.Breed,
            request.PhotoUrl);

        var result = await _createPetHandler.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(GetPetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Retrieves a pet by its ID.
    /// </summary>
    /// <param name="id">The ID of the pet.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The pet if found; otherwise 404 Not Found.</returns>
    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<PetDto>> GetPetById(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetPetByIdQuery(id);
        var result = await _getPetByIdHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paginated list of pets with optional filtering and sorting.
    /// </summary>
    /// <param name="page">The page number (1-based). Default is 1.</param>
    /// <param name="pageSize">The number of items per page. Default is 10.</param>
    /// <param name="status">Optional filter by pet status.</param>
    /// <param name="species">Optional filter by pet species.</param>
    /// <param name="sortBy">Optional sort field (name, species, age, status, created). Default is created.</param>
    /// <param name="sortDirection">Optional sort direction (asc, desc). Default is desc.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of pets.</returns>
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PagedResult<PetDto>>> ListPets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? species = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null,
        CancellationToken cancellationToken = default)
    {
        // Parse optional enums from string query parameters.
        if (!string.IsNullOrWhiteSpace(status) && !Enum.TryParse<PetStatus>(status, ignoreCase: true, out var parsedStatusValue))
        {
            return BadRequest($"Invalid status value '{status}'.");
        }

        PetStatus? parsedStatus = string.IsNullOrWhiteSpace(status)
            ? null
            : Enum.Parse<PetStatus>(status, ignoreCase: true);

        if (!string.IsNullOrWhiteSpace(species) && !Enum.TryParse<Species>(species, ignoreCase: true, out var parsedSpeciesValue))
        {
            return BadRequest($"Invalid species value '{species}'.");
        }

        Species? parsedSpecies = string.IsNullOrWhiteSpace(species)
            ? null
            : Enum.Parse<Species>(species, ignoreCase: true);

        var query = new ListPetsQuery(
            Page: page,
            PageSize: pageSize,
            Status: parsedStatus,
            Species: parsedSpecies,
            SortBy: sortBy,
            SortDirection: sortDirection);

        var result = await _listPetsHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing pet.
    /// </summary>
    /// <param name="id">The ID of the pet to update.</param>
    /// <param name="request">The pet update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>204 No Content on success; 404 if pet not found; 400 if invalid.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePet(
        int id,
        [FromBody] UpdatePetRequest request,
        CancellationToken cancellationToken)
    {
        // TODO: Implement authentication (PAW-31)

        var command = new UpdatePetCommand(
            id,
            request.Name,
            request.Species,
            request.Description,
            request.AgeInMonths,
            request.Breed,
            request.PhotoUrl);

        await _updatePetHandler.HandleAsync(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes a pet by its ID.
    /// </summary>
    /// <param name="id">The ID of the pet to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>204 No Content on success; 404 if pet not found.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePet(
        int id,
        CancellationToken cancellationToken)
    {
        // TODO: Implement authentication (PAW-31)

        var command = new DeletePetCommand(id);

        await _deletePetHandler.HandleAsync(command, cancellationToken);

        return NoContent();
    }
}
