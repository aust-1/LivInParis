using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing chef entities in the database.
/// </summary>
public interface IChefRepository : IRepository<Chef>
{
    /// <summary>
    /// Retrieves a list of chefs, optionally filtered by rating or ban status.
    /// </summary>
    /// <param name="minRating">Optional minimum rating to filter chefs.</param>
    /// <param name="maxRating">Optional maximum rating to filter chefs.</param>
    /// <param name="isBanned">Optional filter to select chefs who are banned or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of chefs.</returns>
    Task<IEnumerable<Chef>> ReadAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null
    );

    /// <summary>
    /// Retrieves the rating of a specific chef.
    /// </summary>
    /// <param name="chef">The chef.</param>
    /// <returns>A task that represents the asynchronous operation, containing the rating of the chef.</returns>
    Task<decimal?> GetChefRatingAsync(Chef chef);

    /// <summary>
    /// Retrieves customers served by a specific chef within a date range.
    /// </summary>
    /// <param name="chef">The chef.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers.</returns>
    Task<IEnumerable<Customer?>> GetCustomersServedByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the dish proposed by a specific chef for today.
    /// </summary>
    /// <param name="chef">The chef.</param>
    /// <returns>A task that represents the asynchronous operation, containing the dish if found; otherwise, null.</returns>
    Task<Dish?> GetTodayDishByChefAsync(Chef chef);

    /// <summary>
    /// Retrieves the number of orders made by each chef in the specified period,
    /// ordered descending by delivery count.
    /// </summary>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a sequence of tuples where each tuple contains a <see cref="Chef"/> and
    /// the count of deliveries they made in the specified period.</returns>
    Task<IEnumerable<(Chef Chef, int OrderCount)>> GetDeliveryCountByChefAsync(
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the value of orders made by each chef in the specified period,
    /// ordered descending by delivery value.
    /// </summary>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing sequence of tuples where each tuple contains a <see cref="Chef"/> and
    /// the total value of deliveries they made in the specified period.</returns>
    Task<IEnumerable<(Chef Chef, decimal TotalSpent)>> GetDeliveryCountValueByChefAsync(
        DateTime? from = null,
        DateTime? to = null
    );
}
