using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.DTOs.Extensions;
using PawHaven.Application.Exceptions;

namespace PawHaven.Application.Features.Pets.Commands;

/// <summary>
/// Handler for the UpdatePetCommand.
/// Coordinates update of an existing pet entity with validation and persistence.
/// </summary>
public class UpdatePetCommandHandler : ICommandHandler<UpdatePetCommand, PetDto>
{
    private readonly IPetRepository _petRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePetCommandHandler(IPetRepository petRepository, IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PetDto> HandleAsync(UpdatePetCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Retrieve existing tracked pet for update
        var pet = await _petRepository.GetByIdForUpdateAsync(command.Id, cancellationToken);
        if (pet is null)
        {
            throw new NotFoundException($"Pet with ID {command.Id} not found.");
        }

        // Update pet entity with validation (ArgumentException thrown on invalid input)
        try
        {
            pet.Update(
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

        // Save changes to database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return updated pet as DTO
        return pet.ToPetDto();
    }
}
