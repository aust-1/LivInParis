using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage order lines in the database.
/// </summary>
public interface IOrderLineService
{
    #region CRUD

    /// <summary>
    /// Creates a new order line in the database.
    /// </summary>
    /// <param name="orderLineId">The unique identifier for the order line.</param>
    /// <param name="orderLineDate">The date and time of the order line.</param>
    /// <param name="duration">The duration of the order line in minutes.</param>
    /// <param name="orderLineStatus">The status of the order line.</param>
    /// <param name="isEatIn">Indicates whether the order is for eat-in.</param>
    /// <param name="addressId">The unique identifier for the address associated with the order line.</param>
    /// <param name="transactionId">The unique identifier for the transaction associated with the order line.</param>
    /// <param name="chefAccountId">The unique identifier for the chef account associated with the order line.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int? orderLineId,
        DateTime orderLineDate,
        int duration,
        OrderLineStatus orderLineStatus,
        bool isEatIn,
        int addressId,
        int transactionId,
        int chefAccountId,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Reads order lines from the database based on the specified criteria.
    /// </summary>
    /// <param name="limit">The maximum number of order lines to retrieve.</param>
    /// <param name="orderLineDate">The date and time of the order line.</param>
    /// <param name="duration">The duration of the order line in minutes.</param>
    /// <param name="orderLineStatus">The status of the order line.</param>
    /// <param name="isEatIn">Indicates whether the order is for eat-in.</param>
    /// <param name="addressId">The unique identifier for the address associated with the order line.</param>
    /// <param name="transactionId">The unique identifier for the transaction associated with the order line.</param>
    /// <param name="chefAccountId">The unique identifier for the chef account associated with the order line.</param>
    /// <param name="orderBy">The column to order the results by.</param>
    /// <param name="orderDirection">The direction to order the results (ascending or descending).</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of order lines that match the specified criteria.</returns>
    List<List<string>> Read(
        int limit,
        DateTime? orderLineDate = null,
        int? duration = null,
        OrderLineStatus? orderLineStatus = null,
        bool? isEatIn = null,
        int? addressId = null,
        int? transactionId = null,
        int? chefAccountId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the status of an existing order line in the database.
    /// </summary>
    /// <param name="orderLineId">The unique identifier for the order line.</param>
    /// <param name="orderLineStatus">The new status of the order line.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateStatus(
        int orderLineId,
        OrderLineStatus orderLineStatus,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes an order line from the database.
    /// </summary>
    /// <param name="orderLineId">The unique identifier for the order line.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int orderLineId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the count of orders grouped by street name.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the count of orders grouped by street name.</returns>
    List<List<string>> GetCommandCountByStreet(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the count of orders grouped by postal code.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the count of orders grouped by postal code.</returns>
    List<List<string>> GetCommandCountByPostalCode(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the total order value grouped by street name.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the total order value grouped by street name.</returns>
    List<List<string>> GetTotalOrderValueByStreet(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the total order value grouped by postal code.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the total order value grouped by postal code.</returns>
    List<List<string>> GetTotalOrderValueByPostalCode(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the most ordered hours of the day.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the most ordered hours of the day.</returns>
    List<List<string>> GetMostOrderedHours(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the most ordered weekdays.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the most ordered weekdays.</returns>
    List<List<string>> GetMostOrderedWeekdays(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the average order duration.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the average order duration.</returns>
    List<List<string>> GetAverageOrderDuration(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves the orders placed within a specific date range.
    /// </summary>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing the orders placed within the specified date range.</returns>
    List<List<string>> GetOrdersByPeriod(
        int limit,
        DateTime from,
        DateTime to,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves the average order price.
    /// </summary>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>The average order price as a decimal.</returns>
    decimal GetAverageOrderPrice(MySqlCommand? command = null);

    #endregion Statistics
}
