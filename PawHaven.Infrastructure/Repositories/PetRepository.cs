using PawHaven.Application.Abstractions;
using PawHaven.Domain.Entities;
using PawHaven.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PawHaven.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for pets using Entity Framework Core.
/// </summary>
internal class PetRepository : IPetRepository
{
    private readonly AppDbContext _context;

    public PetRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pet);

        await _context.Pets.AddAsync(pet, cancellationToken);
    }

    public async Task<Pet?> GetByIdAsync(int petId, CancellationToken cancellationToken = default)
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId, cancellationToken);
    }
}
