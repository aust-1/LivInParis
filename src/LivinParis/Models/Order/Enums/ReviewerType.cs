namespace LivinParisRoussilleTeynier.Models.Order.Enums;

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
