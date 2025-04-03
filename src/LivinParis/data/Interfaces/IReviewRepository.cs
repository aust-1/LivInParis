using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IReview
{
    void CreateReview(
        int reviewId,
        ReviewType reviewType,
        decimal rating,
        string comment,
        DateOnly reviewDate,
        int orderLineId,
        MySqlCommand? command = null
    );

    List<List<string>> GetReviews(
        int limit,
        int? reviewId = null,
        ReviewType? reviewType = null,
        decimal? ratingHigherThan = null,
        decimal? ratingBelow = null,
        int? orderLineId = null,
        MySqlCommand? command = null
    );

    void UpdateReview(
        int reviewId,
        decimal? rating = null,
        string? comment = null,
        MySqlCommand? command = null
    );

    void DeleteReview(int reviewId, MySqlCommand? command = null);
}
