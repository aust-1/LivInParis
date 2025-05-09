using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for transaction-related operations in the database.
/// </summary>
public interface IOrderTransactionRepository : IRepository<OrderTransaction>
{
    /// <summary>
    /// Reads transactions from the database based on the specified criteria.
    /// </summary>
    /// <param name="customerId">The customer Id associated with the transaction.</param>
    /// <param name="minTotalPrice">The minimum total price of the transaction.</param>
    /// <param name="maxTotalPrice">The maximum total price of the transaction.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of transactions.</returns>
    Task<IEnumerable<OrderTransaction>> ReadAsync(
        int? customerId = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        DateTime? from = null,
        DateTime? to = null
    );

    /// <summary>
    /// Retrieves the current transaction for a customer.
    /// This is the transaction that is currently being processed and has not yet been completed.
    /// </summary>
    /// <param name="customerId">Id of the customer whose current transaction is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, containing the current transaction.</returns>
    Task<OrderTransaction?> GetCurrentTransactionAsync(int customerId);

    /// <summary>
    /// Retrieves the total price of an transaction.
    /// </summary>
    /// <param name="transaction">The transaction to calculate the total price for.</param>
    /// <returns>A task that represents the asynchronous operation, containing the total price of the transaction.</returns>
    Task<decimal> GetOrderTotalPriceAsync(OrderTransaction transaction);
}
