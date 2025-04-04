using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for company-related operations in the database.
/// </summary>
[ConnectionControl]
public class CompanyService : ICompanyService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? companyCustomerAccountId,
        string companyName,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Company (account_id, company_name, contact_first_name, contact_last_name)
                VALUES (@id, @name, @firstName, @lastName)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", companyCustomerAccountId);
        command.Parameters.AddWithValue("@name", companyName);
        command.Parameters.AddWithValue("@firstName", contactFirstName);
        command.Parameters.AddWithValue("@lastName", contactLastName);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        bool? companyIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> companies = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM Company");

        if (companyIsBanned is not null)
        {
            conditions.Add("is_banned = @banned");
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

        if (companyIsBanned is not null)
        {
            command.Parameters.AddWithValue("@banned", companyIsBanned);
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
            companies.Add(row);
        }

        return companies;
    }

    /// <inheritdoc/>
    public virtual void UpdateName(
        int companyCustomerAccountId,
        string companyName,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Company SET company_name = @name WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", companyCustomerAccountId);
        command.Parameters.AddWithValue("@name", companyName);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateContact(
        int companyCustomerAccountId,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                UPDATE Company
                SET contact_first_name = @firstName, contact_last_name = @lastName
                WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", companyCustomerAccountId);
        command.Parameters.AddWithValue("@firstName", contactFirstName);
        command.Parameters.AddWithValue("@lastName", contactLastName);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int companyCustomerAccountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Company WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", companyCustomerAccountId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD
}
