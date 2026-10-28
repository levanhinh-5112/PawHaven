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
            "name" => isAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
            "species" => isAscending ? query.OrderBy(p => p.Species) : query.OrderByDescending(p => p.Species),
            "age" => isAscending ? query.OrderBy(p => p.AgeInMonths) : query.OrderByDescending(p => p.AgeInMonths),
            "status" => isAscending ? query.OrderBy(p => p.Status) : query.OrderByDescending(p => p.Status),
            "created" => isAscending ? query.OrderBy(p => p.CreatedAtUtc) : query.OrderByDescending(p => p.CreatedAtUtc),
            _ => query.OrderByDescending(p => p.CreatedAtUtc) // Default to created descending
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
}
