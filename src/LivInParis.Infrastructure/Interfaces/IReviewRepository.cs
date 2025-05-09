using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods to manage reviews in the database.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    /// <summary>
    /// Retrieves the reviews given to a specific customer.
    /// </summary>
    /// <param name="accountId">The identifier of the account associated with the reviews.</param>
    /// <param name="reviewerType">The type of the review (e.g., customer or chef).</param>
    /// <param name="rating">Optional rating filter.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of reviews.</returns>
    Task<IEnumerable<Review>> ReadAsync(
        int accountId,
        ReviewerType reviewerType,
        decimal? rating = null
    );

    /// <summary>
    /// Retrieves the reviews given by a specific customer.
    /// </summary>
    /// <param name="accountId">The identifier of the account associated with the reviews.</param>
    /// <param name="reviewerType">The type of the review (e.g., customer or chef).</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of reviews.</returns>
    Task<decimal?> GetAverageRatingAsync(
        int accountId,
        ReviewerType reviewerType,
        DateTime? from = null,
        DateTime? to = null
    );
}
