using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services.Interfaces
{
    /// <summary>
    /// Provides methods for retrieving various statistics such as customer rankings, spending, and chef performance.
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Retrieves customers optionally filtered by their rating and ban status.
        /// </summary>
        /// <param name="minRating">Minimum rating to include (inclusive).</param>
        /// <param name="maxRating">Maximum rating to include (inclusive).</param>
        /// <param name="isBanned">Filter for banned status if specified.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of customers.</returns>
        Task<IEnumerable<Customer>> GetCustomersAsync(
            decimal? minRating = null,
            decimal? maxRating = null,
            bool? isBanned = null
        );

        /// <summary>
        /// Retrieves top customers by order count within the specified date range.
        /// </summary>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of customer and their order count.</returns>
        Task<IEnumerable<(Customer Customer, int OrderCount)>> GetCustomersByOrderCountAsync(
            DateTime? from = null,
            DateTime? to = null
        );

        /// <summary>
        /// Retrieves top customers by total spending within the specified date range.
        /// </summary>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of customer and their total spending.</returns>
        Task<IEnumerable<(Customer Customer, decimal TotalSpent)>> GetCustomersBySpendingAsync(
            DateTime? from = null,
            DateTime? to = null
        );

        /// <summary>
        /// Retrieves the cuisine order preferences of a specific customer within a date range.
        /// </summary>
        /// <param name="customer">The customer whose preferences to retrieve.</param>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of cuisine nationality and order count.</returns>
        Task<
            IEnumerable<(string CuisineNationality, int OrderCount)>
        > GetCustomerCuisinePreferencesAsync(
            Customer customer,
            DateTime? from = null,
            DateTime? to = null
        );

        /// <summary>
        /// Retrieves the number of deliveries per chef within the specified date range.
        /// </summary>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of chef and their delivery counts.</returns>
        Task<IEnumerable<(Chef Chef, int OrderCount)>> GetChefDeliveryCountAsync(
            DateTime? from = null,
            DateTime? to = null
        );

        /// <summary>
        /// Retrieves the total value of deliveries per chef within the specified date range.
        /// </summary>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of chef and their total delivery value.</returns>
        Task<IEnumerable<(Chef Chef, decimal TotalValue)>> GetChefDeliveryValueAsync(
            DateTime? from = null,
            DateTime? to = null
        );

        /// <summary>
        /// Retrieves the delivery count per dish for a specific chef within the specified date range.
        /// </summary>
        /// <param name="chef">The chef whose dish delivery counts to retrieve.</param>
        /// <param name="from">Start of the period to include; if null, from beginning of time.</param>
        /// <param name="to">End of the period to include; if null, up to the end of time.</param>
        /// <returns>A task representing the asynchronous operation, containing tuples of dish and order count.</returns>
        Task<IEnumerable<(Dish Dish, int OrderCount)>> GetDeliveryCountPerDishByChefAsync(
            Chef chef,
            DateTime? from = null,
            DateTime? to = null
        );
    }
}
