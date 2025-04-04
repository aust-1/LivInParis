using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing address records in the database.
/// </summary>
public interface IAddressService
{
    #region CRUD

    /// <summary>
    /// Creates a new address record with the specified information.
    /// </summary>
    /// <param name="addressId">The unique identifier for the address.</param>
    /// <param name="addressNumber">The number of the address.</param>
    /// <param name="street">The street name of the address.</param>
    /// <param name="postalCode">The postal code of the address.</param>
    /// <param name="nearestMetro">The nearest metro station to the address.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Create(
        int addressId,
        int addressNumber,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of addresses from the database with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of addresses to retrieve.</param>
    /// <param name="street">Optional street name filter.</param>
    /// <param name="postalCode">Optional postal code filter.</param>
    /// <param name="nearestMetro">Optional nearest metro station filter.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    /// <returns>A list of addresses, each represented as a list of strings.</returns>
    List<List<string>> Read(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the street name of an existing address.
    /// </summary>
    /// <param name="addressId">The ID of the address to update.</param>
    /// <param name="nearestMetro">The new nearest metro station.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void UpdateNearestMetro(int addressId, string nearestMetro, MySqlCommand? command = null);

    /// <summary>
    /// Deletes an address by its ID.
    /// </summary>
    /// <param name="addressId">The ID of the address to delete.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Delete(int addressId, MySqlCommand? command = null);

    #endregion CRUD
}
