using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing customer data in the system.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// Retrieves a list of customers, optionally filtered by rating or ban status.
    /// </summary>
    /// <param name="minRating">Optional minimum rating to filter customers.</param>
    /// <param name="maxRating">Optional maximum rating to filter customers.</param>
    /// <param name="isBanned">Optional filter to select customers who are banned or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers.</returns>
    Task<IEnumerable<Customer>> ReadAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null
    );

    /// <summary>
    /// Retrieves the rating of a specific customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>A task that represents the asynchronous operation, containing the rating of the customer.</returns>
    Task<decimal?> GetCustomerRatingAsync(Customer customer);

    /// <summary>
    /// Retrieves the top customers by order count from the database.
    /// </summary>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers sorted by order count.</returns>
    Task<IEnumerable<(Customer Customer, int OrderCount)>> GetCustomersByOrderCountAsync(
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the top customers by spending from the database.
    /// </summary>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers sorted by spending.</returns>
    Task<IEnumerable<(Customer Customer, decimal TotalSpent)>> GetCustomersBySpendingAsync(
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves how many times each cuisine was ordered by a customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of cuisine nationalities and their order counts.</returns>
    Task<
        IEnumerable<(string CuisineNationality, int OrderCount)>
    > GetCustomerCuisinePreferencesAsync(
        Customer customer,
        DateTime? from = null,
        DateTime? to = null
    );
}
