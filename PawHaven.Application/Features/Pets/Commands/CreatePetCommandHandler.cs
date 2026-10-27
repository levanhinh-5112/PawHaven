using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.DTOs.Extensions;
using PawHaven.Application.Exceptions;
using PawHaven.Domain.Entities;

namespace PawHaven.Application.Features.Pets.Commands;

/// <summary>
/// Handler for the CreatePetCommand.
/// Coordinates creation of a new pet entity with validation and persistence.
/// </summary>
public class CreatePetCommandHandler : ICommandHandler<CreatePetCommand, PetDto>
{
    private readonly IPetRepository _petRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetCommandHandler(IPetRepository petRepository, IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PetDto> HandleAsync(CreatePetCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Create Pet entity with validation (ArgumentException thrown on invalid input)
        Pet pet;
        try
        {
            pet = new Pet(
                name: command.Name,
                species: command.Species,
                description: command.Description,
                ageInMonths: command.AgeInMonths,
                breed: command.Breed,
                photoUrl: command.PhotoUrl);
        }
        catch (ArgumentException ex)
        {
            // Convert domain validation exception to application exception
            throw ValidationException.FromArgumentException(ex);
        }

        // Persist pet to repository
        await _petRepository.AddAsync(pet, cancellationToken);

        // Save changes to database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return created pet as DTO
        return pet.ToPetDto();
    }
}
