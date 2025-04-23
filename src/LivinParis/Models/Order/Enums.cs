namespace LivinParisRoussilleTeynier.Models.Order;

/// <summary>
/// Represents the type of dish in the menu.
/// </summary>
/// <remarks>
/// Categorizes dishes into three types:
/// <list type="bullet">
/// <item><description>Starter</description></item>
/// <item><description>MainCourse</description></item>
/// <item><description>Dessert</description></item>
/// </list>
/// </remarks>
public enum DishType
{
    /// <summary>
    /// Represents a starter dish.
    /// </summary>
    Starter,

    /// <summary>
    /// Represents a main course dish.
    /// </summary>
    MainCourse,

    /// <summary>
    /// Represents a dessert dish.
    /// </summary>
    Dessert,
}

/// <summary>
/// Represents the largest origin of the products used in the dish.
/// </summary>
/// <remarks>
/// Categorizes products into three origins:
/// <list type="bullet">
/// <item><description>France</description></item>
/// <item><description>Europe</description></item>
/// <item><description>Other</description></item>
/// </list>
/// </remarks>
public enum ProductsOrigin
{
    /// <summary>
    /// Represents products originating from France.
    /// </summary>
    France,

    /// <summary>
    /// Represents products originating from Europe.
    /// </summary>
    Europe,

    /// <summary>
    /// Represents products originating from other regions.
    /// </summary>
    Other,
}

/// <summary>
/// Represents the status of an order.
/// </summary>
/// <remarks>
/// Categorizes orders into five statuses:
/// <list type="bullet">
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

/// <summary>
/// Represents the type of reviewer for a review.
/// </summary>
/// <remarks>
/// Categorizes reviewers into two types:
/// <list type="bullet">
/// <item><description>Customer</description></item>
/// <item><description>Chef</description></item>
/// </list>
/// </remarks>
public enum ReviewerType
{
    /// <summary>
    /// The review is for a chef by a customer.
    /// </summary>
    Customer,

    /// <summary>
    /// The review is for a customer by another chef.
    /// </summary>
    Chef,
}
