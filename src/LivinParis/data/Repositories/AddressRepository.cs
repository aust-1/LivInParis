using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for address-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class AddressRepository(LivInParisContext context)
    : Repository<Address>(context),
        IAddressRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Address>> FindByStreetAsync(string street)
    {
        return await _dbSet.Where(a => a.Street.Contains(street)).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Address>> FindByNeareastStationAsync(Station nearestStation)
    {
        return await _dbSet.Where(a => a.NearestStation == nearestStation).ToListAsync();
    }
}
