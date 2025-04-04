using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing individual customer profiles.
/// </summary>
public interface IIndividualService
{
    #region CRUD

    /// <summary>
    /// Creates a new individual customer profile.
    /// </summary>
    void Create(
        int individualCustomerAccountId,
        string lastName,
        string firstName,
        string personalEmail,
        string phoneNumber,
        int addressId,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves individual profiles with optional filters.
    /// </summary>
    List<List<string>> Read(
        int limit,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        string? street = null,
        int? postalCode = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates fields of an individual profile.
    /// </summary>
    void Update(
        int individualCustomerAccountId,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        int? addressId = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes an individual profile.
    /// </summary>
    void Delete(int individualCustomerAccountId, MySqlCommand? command = null);

    #endregion CRUD
}
