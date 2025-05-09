namespace LivInParisRoussilleTeynier.Domain.Models.Order.Enums;

/// <summary>
/// Represents the status of an order.
/// </summary>
/// <remarks>
/// Categorizes orders into five statuses:
/// <list type="bullet">
/// <item><description>InCart</description></item>
/// <item><description>Pending</description></item>
/// <item><description>Preparing</description></item>
/// <item><description>Delivering</description></item>
/// <item><description>Delivered</description></item>
/// <item><description>Canceled</description></item>
/// </list>
/// </remarks>
public enum OrderLineStatus
{
    /// <summary>
    /// The order is in the cart and has not been placed yet.
    /// </summary>
    InCart,

    /// <summary>
    /// The order has been placed but not yet confirmed.
    /// </summary>
    Pending,

    /// <summary>
    /// The order has been confirmed and is being prepared.
    /// </summary>
    Preparing,

    /// <summary>
    /// The order is out for delivery.
    /// </summary>
    Delivering,

    /// <summary>
    /// The order has been delivered successfully.
    /// </summary>
    Delivered,

    /// <summary>
    /// The order has been canceled.
    /// </summary>
    Canceled,
}
