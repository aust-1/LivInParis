namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing chef entities in the database.
/// </summary>
public interface IChefRepository : IRepository<Chef>
{
    /// <summary>
    /// Retrieves a list of chefs, optionally filtered by rating, ban status, or eating-on-site option.
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
    /// Retrieves customers served by a specific chef within a date range.
    /// </summary>
    /// <param name="chef">The chef.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of customers.</returns>
    Task<IEnumerable<Customer>> GetCustomersServedByChefAsync(
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
    /// Retrieves the number of orders made by the chefs, ordered by the number of orders.
    /// </summary>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of chefs and their order counts.</returns>
    Task<Dictionary<Chef, int>> GetDeliveryCountByChefAsync(
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the value of orders made by the chefs, ordered by the value of orders.
    /// </summary>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of chefs and their order values.</returns>
    Task<Dictionary<Chef, decimal>> GetDeliveryCountValueByChefAsync(
        DateTime? from = null,
        DateTime? to = null
    );
}
