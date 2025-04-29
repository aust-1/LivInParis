using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing individual customer profiles.
/// </summary>
public interface IIndividualRepository : IRepository<Individual>
{
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
    /// Deletes an individual profile.
    /// </summary>
    /// <param name="individualCustomerAccountId">The account ID of the individual customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int individualCustomerAccountId, MySqlCommand? command = null);

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
