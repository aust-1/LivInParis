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
    /// <param name="individualCustomerAccountId">The account ID of the individual customer.</param>
    /// <param name="lastName">The last name of the individual customer.</param>
    /// <param name="firstName">The first name of the individual customer.</param>
    /// <param name="personalEmail">The personal email of the individual customer.</param>
    /// <param name="phoneNumber">The phone number of the individual customer.</param>
    /// <param name="addressId">The ID of the address associated with the individual customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
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
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="lastName">Filter by last name.</param>
    /// <param name="firstName">Filter by first name.</param>
    /// <param name="personalEmail">Filter by personal email.</param>
    /// <param name="phoneNumber">Filter by phone number.</param>
    /// <param name="street">Filter by street address.</param>
    /// <param name="postalCode">Filter by postal code.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing individual profiles.</returns>
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
    /// <param name="individualCustomerAccountId">The account ID of the individual customer.</param>
    /// <param name="lastName">The last name of the individual customer.</param>
    /// <param name="firstName">The first name of the individual customer.</param>
    /// <param name="personalEmail">The personal email of the individual customer.</param>
    /// <param name="phoneNumber">The phone number of the individual customer.</param>
    /// <param name="addressId">The ID of the address associated with the individual customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
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
    /// <param name="individualCustomerAccountId">The account ID of the individual customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int individualCustomerAccountId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the customers grouped by street name.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="streetName">The street name to filter by.</param>
    /// <param name="orderBy">Column to order the result set by.</param>
    /// <param name="orderDirection">True for ascending, false for descending ordering.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing customer rows.</returns>
    List<List<string>> GetCustomersByStreet(
        int limit,
        string streetName,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    #endregion Statistics
}
