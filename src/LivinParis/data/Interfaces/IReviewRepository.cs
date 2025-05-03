using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage reviews in the database.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    //TODO: calcul note customer and chef
    //FIXME: supprimer rating de customer et chef

    /// <summary>
    /// Retrieves the reviews given to a specific customer.
    /// </summary>
    /// <param name="account">The account associated with the reviews.</param>
    /// <param name="reviewerType">The type of the review (e.g., customer or chef).</param>
    /// <param name="rating">Optional rating filter.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of reviews.</returns>
    Task<IEnumerable<Review>> ReadAsync(
        Account account,
        ReviewerType reviewerType,
        decimal? rating = null
    );
}
