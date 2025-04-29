namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing address records in the database.
/// </summary>
public interface IAddressRepository : IRepository<Address>
{
    /// <summary>
    /// Finds addresses by their street.
    /// </summary>
    /// <param name="street">Optional street name filter.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of addresses matching the filter.</returns>
    Task<IEnumerable<Address>> FindByStreetAsync(string street);

    /// <summary>
    /// Finds addresses by their nearest station.
    /// </summary>
    /// <param name="nearestStation">The nearest station to filter addresses.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of addresses matching the filter.</returns>
    Task<IEnumerable<Address>> FindByNeareastStationAsync(Station nearestStation);
}

//QUESTION: delete ces deux méthodes ?
