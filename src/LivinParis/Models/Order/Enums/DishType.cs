namespace LivinParisRoussilleTeynier.Models.Order.Enums;

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
