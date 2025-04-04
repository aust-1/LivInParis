using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for managing chef menu proposals in the database.
/// </summary>
[ConnectionControl]
public class MenuProposalService : IMenuProposalService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int chefId,
        DateOnly proposalDate,
        int dishId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO MenuProposal (account_id, proposal_date, dish_id)
                VALUES (@chefId, @proposalDate, @dishId)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@chefId", chefId);
        command.Parameters.AddWithValue(
            "@proposalDate",
            proposalDate.ToDateTime(TimeOnly.MinValue)
        );
        command.Parameters.AddWithValue("@dishId", dishId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        int? chefId = null,
        DateOnly? proposalDate = null,
        int? dishId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> proposals = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM MenuProposal");

        if (chefId is not null)
        {
            conditions.Add("account_id = @chefId");
        }

        if (proposalDate is not null)
        {
            conditions.Add("proposal_date = @proposalDate");
        }

        if (dishId is not null)
        {
            conditions.Add("dish_id = @dishId");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE ");
            query.Append(string.Join(" AND ", conditions));
        }

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query.Append(" ORDER BY ");
            query.Append(orderBy);
            query.Append(orderDirection == true ? " ASC" : " DESC");
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (chefId is not null)
        {
            command.Parameters.AddWithValue("@chefId", chefId);
        }
        if (proposalDate is not null)
        {
            command.Parameters.AddWithValue(
                "@proposalDate",
                proposalDate.Value.ToDateTime(TimeOnly.MinValue)
            );
        }
        if (dishId is not null)
        {
            command.Parameters.AddWithValue("@dishId", dishId);
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
            proposals.Add(row);
        }

        return proposals;
    }

    /// <inheritdoc/>
    public virtual void Update(
        int chefId,
        DateOnly proposalDate,
        int dishId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                UPDATE MenuProposal
                SET dish_id = @dishId
                WHERE account_id = @chefId AND proposal_date = @proposalDate";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@chefId", chefId);
        command.Parameters.AddWithValue(
            "@proposalDate",
            proposalDate.ToDateTime(TimeOnly.MinValue)
        );
        command.Parameters.AddWithValue("@dishId", dishId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int chefId, DateOnly proposalDate, MySqlCommand? command = null)
    {
        command!.CommandText =
            @"
                DELETE FROM MenuProposal
                WHERE account_id = @chefId AND proposal_date = @proposalDate";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@chefId", chefId);
        command.Parameters.AddWithValue(
            "@proposalDate",
            proposalDate.ToDateTime(TimeOnly.MinValue)
        );
        command.ExecuteNonQuery();
    }

    #endregion CRUD
}
