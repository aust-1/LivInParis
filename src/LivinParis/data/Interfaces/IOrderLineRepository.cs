namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage order lines in the database.
/// </summary>
public interface IOrderLineRepository : IRepository<OrderLine>
{
    /// <summary>
    /// Retrieves a list of order lines for a specific transaction.
    /// </summary>
    /// <param name="transaction">The transaction to filter by.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of order lines.</returns>
    Task<IEnumerable<OrderLine>> ReadAsync(OrderTransaction transaction);

    /// <summary>
    /// Retrieves the orders placed by a specific chef within a date range and for a specific customer if provided.
    /// </summary>
    /// <param name="chef">The chef to filter by.</param>
    /// <param name="customer">The customer to filter by.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of order lines.</returns>
    Task<IEnumerable<OrderLine>> ReadAsync(
        Chef? chef = null,
        Customer? customer = null,
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the average order price for a specific chef and customer within a date range if provided.
    /// </summary>
    /// <param name="chef">The chef to filter by.</param>
    /// <param name="customer">The customer to filter by.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>>A task that represents the asynchronous operation, containing the average order price.</returns>
    Task<decimal> GetAverageOrderPriceAsync(
        Chef? chef = null,
        Customer? customer = null,
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the count of orders grouped by street name.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of street names and their order counts.</returns>
    Task<IEnumerable<(string street, int OrderCount)>> GetCommandCountByStreetAsync();

    /// <summary>
    /// Retrieves the count of orders grouped by postal code.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of postal codes and their order counts.</returns>
    Task<IEnumerable<(int postalCode, int OrderCount)>> GetCommandCountByPostalCodeAsync();

    /// <summary>
    /// Retrieves the total order value grouped by street name.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of street names and their total order values.</returns>
    Task<IEnumerable<(string street, int TotalSpent)>> GetTotalOrderValueByStreetAsync();

    /// <summary>
    /// Retrieves the total order value grouped by postal code.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of postal codes and their total order values.</returns>
    Task<IEnumerable<(int postalCode, int TotalSpent)>> GetTotalOrderValueByPostalCodeAsync();
}


//TODO: to implement
