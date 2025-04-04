using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for review-related operations in the database.
/// </summary>
[ConnectionControl]
public class ReviewService : IReviewService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int reviewId,
        ReviewType reviewType,
        decimal rating,
        string comment,
        DateOnly reviewDate,
        int orderLineId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Review (
                    review_id, review_type, review_rating, comment,
                    review_date, order_line_id
                )
                VALUES (
                    @id, @type, @rating, @comment,
                    @date, @orderLine
                )";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", reviewId);
        command.Parameters.AddWithValue("@type", reviewType.ToString());
        command.Parameters.AddWithValue("@rating", rating);
        command.Parameters.AddWithValue("@comment", comment);
        command.Parameters.AddWithValue("@date", reviewDate.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@orderLine", orderLineId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        int? reviewId = null,
        ReviewType? reviewType = null,
        decimal? minRating = null,
        decimal? maxRating = null,
        int? orderLineId = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM Review");

        if (reviewId is not null)
        {
            conditions.Add("review_id = @id");
        }
        if (reviewType is not null)
        {
            conditions.Add("review_type = @type");
        }
        if (minRating is not null)
        {
            conditions.Add("review_rating >= @minRating");
        }
        if (maxRating is not null)
        {
            conditions.Add("review_rating <= @maxRating");
        }
        if (orderLineId is not null)
        {
            conditions.Add("order_line_id = @orderLine");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (reviewId is not null)
        {
            command.Parameters.AddWithValue("@id", reviewId);
        }
        if (reviewType is not null)
        {
            command.Parameters.AddWithValue("@type", reviewType.ToString());
        }
        if (minRating is not null)
        {
            command.Parameters.AddWithValue("@minRating", minRating);
        }
        if (maxRating is not null)
        {
            command.Parameters.AddWithValue("@maxRating", maxRating);
        }
        if (orderLineId is not null)
        {
            command.Parameters.AddWithValue("@orderLine", orderLineId);
        }

        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> row = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? string.Empty
                    : reader.GetValue(i).ToString() ?? string.Empty;
                row.Add(value);
            }
            results.Add(row);
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual void Update(
        int reviewId,
        decimal? rating = null,
        string? comment = null,
        MySqlCommand? command = null
    )
    {
        List<string> updates = [];

        if (rating is not null)
        {
            updates.Add("review_rating = @rating");
        }
        if (comment is not null)
        {
            updates.Add("comment = @comment");
        }

        if (updates.Count == 0)
        {
            return;
        }

        StringBuilder query = new();
        query.Append("UPDATE Review SET ");
        query.Append(string.Join(", ", updates));
        query.Append(" WHERE review_id = @id");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();
        if (rating is not null)
        {
            command.Parameters.AddWithValue("@rating", rating);
        }
        if (comment is not null)
        {
            command.Parameters.AddWithValue("@comment", comment);
        }
        command.Parameters.AddWithValue("@id", reviewId);

        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int reviewId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Review WHERE review_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", reviewId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    /// <inheritdoc/>
    public virtual List<List<string>> GetReviewsByAccount(
        int limit,
        int accountId,
        ReviewType reviewType,
        string? orderBy = "review_rating",
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        StringBuilder query = new(
            @"
        SELECT r.*
        FROM Review r
        JOIN OrderLine o ON r.order_line_id = o.order_line_id
        WHERE r.review_type = @type AND o.account_id = @accountId
    "
        );

        query.Append(" ORDER BY ");
        query.Append(orderBy ?? "review_rating");
        query.Append(orderDirection == true ? " ASC" : " DESC");
        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@type", reviewType.ToString());
        command.Parameters.AddWithValue("@accountId", accountId);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> row = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.IsDBNull(i) ? string.Empty : reader[i].ToString() ?? string.Empty);
            }
            results.Add(row);
        }

        return results;
    }

    #endregion Statistics
}
