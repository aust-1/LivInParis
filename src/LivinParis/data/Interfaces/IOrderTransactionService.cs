using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for transaction-related operations in the database.
/// </summary>
public interface IOrderTransactionService
{
    #region CRUD

    /// <summary>
    /// Creates a new transaction in the database.
    /// </summary>
    /// <param name="transactionId">The ID of the transaction.</param>
    /// <param name="transactionDate">The date of the transaction.</param>
    /// <param name="customerAccountId">The ID of the customer account associated with the transaction.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int transactionId,
        DateTime transactionDate,
        int customerAccountId,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Reads transactions from the database based on the specified criteria.
    /// </summary>
    /// <param name="limit">The maximum number of transactions to read.</param>
    /// <param name="transactionDate">The date of the transaction.</param>
    /// <param name="customerAccountId">The ID of the customer account associated with the transaction.</param>
    /// <param name="minTotalPrice">The minimum total price of the transaction.</param>
    /// <param name="maxTotalPrice">The maximum total price of the transaction.</param>
    /// <param name="orderBy">The column to order the results by.</param>
    /// <param name="orderDirection">The direction to order the results (true for ascending, false for descending).</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of transactions matching the specified criteria.</returns>
    List<List<string>> Read(
        int limit,
        DateTime? transactionDate = null,
        int? customerAccountId = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes a transaction from the database.
    /// </summary>
    /// <param name="transactionId">The ID of the transaction to delete.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int transactionId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the top customers by order count from the database.
    /// </summary>
    /// <param name="limit">The maximum number of customers to retrieve.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of customers sorted by order count.</returns>
    List<List<string>> GetTopCustomersByOrderCount(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the top customers by spending from the database.
    /// </summary>
    /// <param name="limit">The maximum number of customers to retrieve.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of customers sorted by spending.</returns>
    List<List<string>> GetTopCustomersBySpending(int limit, MySqlCommand? command = null);

    #endregion Statistics
}
