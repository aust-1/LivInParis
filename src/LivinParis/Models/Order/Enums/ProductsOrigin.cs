namespace LivInParisRoussilleTeynier.Models.Order.Enums;

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
