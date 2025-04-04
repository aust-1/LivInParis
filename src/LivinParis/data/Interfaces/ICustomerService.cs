using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing customer data in the system.
/// </summary>
public interface ICustomerService
{
    #region CRUD

    /// <summary>
    /// Creates a new customer account.
    /// </summary>
    /// <param name="customerAccountId">The account ID of the customer (linked to the Account table).</param>
    /// <param name="customerRating">The rating of the customer.</param>
    /// <param name="loyaltyRank">The loyalty rank of the customer.</param>
    /// <param name="customerIsBanned">Indicates whether the customer is banned.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int customerAccountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of customers with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="minRating">Minimum rating filter.</param>
    /// <param name="maxRating">Maximum rating filter.</param>
    /// <param name="loyaltyRank">Loyalty rank filter.</param>
    /// <param name="customerIsBanned">Filter for banned or non-banned customers.</param>
    /// <param name="orderBy">Column to order the result set by.</param>
    /// <param name="orderDirection">True for ascending, false for descending ordering.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing customer rows.</returns>
    List<List<string>> Read(
        int limit,
        decimal? minRating = null,
        decimal? maxRating = null,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the rating of a customer.
    /// </summary>
    /// <param name="customerAccountId">The account ID of the customer.</param>
    /// <param name="customerRating">The new rating of the customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateRating(int customerAccountId, decimal customerRating, MySqlCommand? command = null);

    /// <summary>
    /// Updates the loyalty rank of a customer.
    /// </summary>
    /// <param name="customerAccountId">The account ID of the customer.</param>
    /// <param name="loyaltyRank">The new loyalty rank of the customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateLoyaltyRank(
        int customerAccountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the ban status of a customer.
    /// </summary>
    /// <param name="customerAccountId">The account ID of the customer.</param>
    /// <param name="customerIsBanned">Indicates whether the customer is banned.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateBanStatus(
        int customerAccountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes a customer from the system.
    /// </summary>
    /// <param name="customerAccountId">The account ID of the customer.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int customerAccountId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves customers served by a specific chef within a date range.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="chefId">The unique identifier for the chef.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing customers served by the chef.</returns>
    List<List<string>> GetCustomersServedByChef(
        int limit,
        int chefId,
        DateTime? from = null,
        DateTime? to = null,
        MySqlCommand? command = null
    );

    #endregion Statistics
}
