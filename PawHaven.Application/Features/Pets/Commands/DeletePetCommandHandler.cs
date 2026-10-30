using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.Exceptions;

namespace PawHaven.Application.Features.Pets.Commands;

/// <summary>
/// Handler for the DeletePetCommand.
/// Coordinates deletion of a pet entity from persistence.
/// </summary>
public class DeletePetCommandHandler : ICommandHandler<DeletePetCommand>
{
    private readonly IPetRepository _petRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetCommandHandler(IPetRepository petRepository, IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task HandleAsync(DeletePetCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Delete pet from repository
        var deleted = await _petRepository.DeleteAsync(command.Id, cancellationToken);
        if (!deleted)
        {
            throw new NotFoundException($"Pet with ID {command.Id} not found.");
        }

        // Save changes to database
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
