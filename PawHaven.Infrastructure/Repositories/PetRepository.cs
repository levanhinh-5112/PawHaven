using PawHaven.Application.Abstractions;
using PawHaven.Application.DTOs;
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
        return await _context.Pets.AsNoTracking().FirstOrDefaultAsync(p => p.Id == petId, cancellationToken);
    }

    public async Task<Pet?> GetByIdForUpdateAsync(int petId, CancellationToken cancellationToken = default)
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId, cancellationToken);
    }

    public async Task<PagedResult<Pet>> ListAsync(ListPetsQueryFilter filter, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        // Build base query
        var query = _context.Pets.AsNoTracking();

        // Apply optional Status filter
        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        // Apply optional Species filter
        if (filter.Species.HasValue)
        {
            query = query.Where(p => p.Species == filter.Species.Value);
        }

        // Apply sorting
        var sortBy = filter.SortBy?.ToLower() ?? "created";
        var sortDirection = filter.SortDirection?.ToLower() ?? "desc";
        var isAscending = sortDirection == "asc";

        query = sortBy switch
        {
            "name" => isAscending ? query.OrderBy(p => p.Name).ThenBy(p => p.Id) : query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            "species" => isAscending ? query.OrderBy(p => p.Species).ThenBy(p => p.Id) : query.OrderByDescending(p => p.Species).ThenBy(p => p.Id),
            "age" => isAscending ? query.OrderBy(p => p.AgeInMonths).ThenBy(p => p.Id) : query.OrderByDescending(p => p.AgeInMonths).ThenBy(p => p.Id),
            "status" => isAscending ? query.OrderBy(p => p.Status).ThenBy(p => p.Id) : query.OrderByDescending(p => p.Status).ThenBy(p => p.Id),
            "created" => isAscending ? query.OrderBy(p => p.CreatedAtUtc).ThenBy(p => p.Id) : query.OrderByDescending(p => p.CreatedAtUtc).ThenBy(p => p.Id),
            _ => query.OrderByDescending(p => p.CreatedAtUtc).ThenBy(p => p.Id) // Default to created descending
        };

        // Calculate offset for pagination
        var offset = (page - 1) * pageSize;

        // Execute count query for total count (before pagination)
        var totalCount = await query.CountAsync(cancellationToken);

        // Execute data query with pagination
        var items = await query
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Return paginated result using constructor
        return new PagedResult<Pet>(items, page, pageSize, totalCount);
    }

    public async Task<bool> DeleteAsync(int petId, CancellationToken cancellationToken = default)
    {
        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId, cancellationToken);
        if (pet is null)
        {
            return false;
        }

        _context.Pets.Remove(pet);
        return true;
    }
}
