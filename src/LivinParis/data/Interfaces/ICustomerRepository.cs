using LivInParisRoussilleTeynier.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing customer data in the system.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// Retrieves a list of customers with optional filters.
    /// </summary>
    /// <param name="minRating"></param>
    /// <param name="maxRating"></param>
    /// <param name="isBanned"></param>
    /// <param name="loyaltyRank">Loyalty rank filter.</param>
    /// <param name="customerIsBanned">Filter for banned or non-banned customers.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers.</returns>
    Task<IEnumerable<Customer>> ReadAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null
    );

    /// <summary>
    /// Retrieves the top customers by order count from the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers sorted by order count.</returns>
    Task<IEnumerable<(Account Account, int OrderCount)>> GetCustomersByOrderCountAsync();

    /// <summary>
    /// Retrieves the top customers by spending from the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers sorted by spending.</returns>
    Task<IEnumerable<(Account Account, decimal TotalSpent)>> GetCustomersBySpendingAsync();

    /// <summary>
    /// Retrieves the average price per customer order from the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the average price per customer order.</returns>
    Task<decimal> GetAveragePricePerCustomerOrderAsync();

    /// <summary>
    /// Retrieves how many times each cuisine was ordered by a customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cuisine nationalities and their order counts.</returns>
    Task<
        IEnumerable<(string CuisineNationality, int OrderCount)>
    > GetCustomerCuisinePreferencesAsync(Customer customer, DateTime? from, DateTime? to);
}
