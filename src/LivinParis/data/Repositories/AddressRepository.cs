using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class AddressRepository(LivInParisContext context)
    : Repository<Address>(context),
        IAddressRepository
{
    public async Task<IEnumerable<Address>> FindByStreetAsync(string street)
    {
        return await _dbSet.Where(a => a.Street.Contains(street)).ToListAsync();
    }

    public async Task<IEnumerable<Address>> FindByNeareastStationAsync(Station nearestStation)
    {
        return await _dbSet.Where(a => a.NearestStation == nearestStation).ToListAsync();
    }
}
