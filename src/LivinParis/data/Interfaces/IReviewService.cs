using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IReviewService
{
    void Create(
        int reviewId,
        ReviewType reviewType,
        decimal rating,
        string comment,
        DateOnly reviewDate,
        int orderLineId,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        int? reviewId = null,
        ReviewType? reviewType = null,
        decimal? minRating = null,
        decimal? maxRating = null,
        int? orderLineId = null,
        MySqlCommand? command = null
    );

    List<List<string>> GetReviewsByAccount(
        int limit,
        int accountId,
        ReviewType reviewType,
        string? orderBy = "review_rating",
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void Update(
        int reviewId,
        decimal? rating = null,
        string? comment = null,
        MySqlCommand? command = null
    );

    void Delete(int reviewId, MySqlCommand? command = null);
}
