using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage reviews in the database.
/// </summary>
public interface IReviewService
{
    #region CRUD

    /// <summary>
    /// Creates a new review in the database.
    /// </summary>
    /// <param name="reviewId">The unique identifier for the review.</param>
    /// <param name="reviewType">The type of the review (e.g., customer or chef).</param>
    /// <param name="rating">The rating given in the review.</param>
    /// <param name="comment">The comment provided in the review.</param>
    /// <param name="reviewDate">The date of the review.</param>
    /// <param name="orderLineId">The unique identifier for the order line associated with the review.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int? reviewId,
        ReviewType reviewType,
        decimal rating,
        string comment,
        DateOnly reviewDate,
        int orderLineId,
        MySqlCommand? command = null
    );

    //TODO: calcul note customer and chef
    //FIXME: supprimer rating de customer et chef

    /// <summary>
    /// Reads reviews from the database based on the specified criteria.
    /// </summary>
    /// <param name="limit">The maximum number of reviews to retrieve.</param>
    /// <param name="reviewId">The unique identifier for the review.</param>
    /// <param name="reviewType">The type of the review (e.g., customer or chef).</param>
    /// <param name="minRating">The minimum rating for the review.</param>
    /// <param name="maxRating">The maximum rating for the review.</param>
    /// <param name="orderLineId">The unique identifier for the order line associated with the review.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of reviews that match the specified criteria.</returns>
    List<List<string>> Read(
        int limit,
        int? reviewId = null,
        ReviewType? reviewType = null,
        decimal? minRating = null,
        decimal? maxRating = null,
        int? orderLineId = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates an existing review in the database.
    /// </summary>
    /// <param name="reviewId">The unique identifier for the review.</param>
    /// <param name="rating">The new rating for the review.</param>
    /// <param name="comment">The new comment for the review.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Update(
        int reviewId,
        decimal? rating = null,
        string? comment = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes a review from the database.
    /// </summary>
    /// <param name="reviewId">The unique identifier for the review.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int reviewId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves reviews from the database based on the specified criteria.
    /// </summary>
    /// <param name="limit">The maximum number of reviews to retrieve.</param>
    /// <param name="accountId">The unique identifier for the account associated with the reviews.</param>
    /// <param name="reviewType">The type of the review (e.g., customer or chef).</param>
    /// <param name="orderBy">The column to order the results by.</param>
    /// <param name="orderDirection">The direction to order the results (ascending or descending).</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of reviews that match the specified criteria.</returns>
    List<List<string>> GetReviewsByAccount(
        int limit,
        int accountId,
        ReviewType reviewType,
        string? orderBy = "review_rating",
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    #endregion Statistics
}
