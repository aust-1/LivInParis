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
    /// <param name="status">The status of the order line to filter by.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of order lines.</returns>
    Task<IEnumerable<OrderLine>> ReadAsync(
        Chef? chef = null,
        Customer? customer = null,
        OrderLineStatus? status = null,
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the average order price for a specific chef and customer within a date range if provided.
    /// </summary>
    /// <param name="chef">The chef to filter by.</param>
    /// <param name="customer">The customer to filter by.</param>
    /// <param name="status">The status of the order line to filter by.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>>A task that represents the asynchronous operation, containing the average order price.</returns>
    Task<decimal> GetAverageOrderPriceAsync(
        Chef? chef = null,
        Customer? customer = null,
        OrderLineStatus? status = null,
        DateTime? from = null,
        DateTime? to = null
    );
}
