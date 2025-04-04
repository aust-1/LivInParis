using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for individual customer-related operations in the database.
/// </summary>
[ConnectionControl]
public class IndividualService : IIndividualService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? individualCustomerAccountId,
        string lastName,
        string firstName,
        string personalEmail,
        string phoneNumber,
        int addressId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Individual (
                    account_id, last_name, first_name,
                    personal_email, phone_number, address_id
                )
                VALUES (@id, @lastName, @firstName, @email, @phone, @address)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", individualCustomerAccountId);
        command.Parameters.AddWithValue("@lastName", lastName);
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@email", personalEmail);
        command.Parameters.AddWithValue("@phone", phoneNumber);
        command.Parameters.AddWithValue("@address", addressId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        string? street = null,
        int? postalCode = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> individuals = [];
        List<string> conditions = [];
        StringBuilder query = new(
            @"
                SELECT i.*
                FROM Individual i
                JOIN Address a ON i.address_id = a.address_id
            "
        );

        if (lastName is not null)
        {
            conditions.Add("i.last_name LIKE @lastName");
        }

        if (firstName is not null)
        {
            conditions.Add("i.first_name LIKE @firstName");
        }

        if (personalEmail is not null)
        {
            conditions.Add("i.personal_email LIKE @email");
        }

        if (phoneNumber is not null)
        {
            conditions.Add("i.phone_number LIKE @phone");
        }

        if (street is not null)
        {
            conditions.Add("a.street LIKE @street");
        }

        if (postalCode is not null)
        {
            conditions.Add("a.postal_code = @postalCode");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (lastName is not null)
        {
            command.Parameters.AddWithValue("@lastName", $"%{lastName}%");
        }
        if (firstName is not null)
        {
            command.Parameters.AddWithValue("@firstName", $"%{firstName}%");
        }
        if (personalEmail is not null)
        {
            command.Parameters.AddWithValue("@email", $"%{personalEmail}%");
        }
        if (phoneNumber is not null)
        {
            command.Parameters.AddWithValue("@phone", $"%{phoneNumber}%");
        }
        if (street is not null)
        {
            command.Parameters.AddWithValue("@street", $"%{street}%");
        }
        if (postalCode is not null)
        {
            command.Parameters.AddWithValue("@postalCode", postalCode);
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
            individuals.Add(row);
        }

        return individuals;
    }

    /// <inheritdoc/>
    public virtual void Update(
        int individualCustomerAccountId,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        int? addressId = null,
        MySqlCommand? command = null
    )
    {
        List<string> updates = [];

        if (lastName is not null)
        {
            updates.Add("last_name = @lastName");
        }
        if (firstName is not null)
        {
            updates.Add("first_name = @firstName");
        }
        if (personalEmail is not null)
        {
            updates.Add("personal_email = @email");
        }
        if (phoneNumber is not null)
        {
            updates.Add("phone_number = @phone");
        }
        if (addressId is not null)
        {
            updates.Add("address_id = @address");
        }

        if (updates.Count == 0)
        {
            return;
        }

        StringBuilder query = new();
        query.Append("UPDATE Individual SET ");
        query.Append(string.Join(", ", updates));
        query.Append(" WHERE account_id = @id");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (lastName is not null)
        {
            command.Parameters.AddWithValue("@lastName", lastName);
        }
        if (firstName is not null)
        {
            command.Parameters.AddWithValue("@firstName", firstName);
        }
        if (personalEmail is not null)
        {
            command.Parameters.AddWithValue("@email", personalEmail);
        }
        if (phoneNumber is not null)
        {
            command.Parameters.AddWithValue("@phone", phoneNumber);
        }
        if (addressId is not null)
        {
            command.Parameters.AddWithValue("@address", addressId);
        }

        command.Parameters.AddWithValue("@id", individualCustomerAccountId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int individualCustomerAccountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Individual WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", individualCustomerAccountId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    /// <inheritdoc/>
    public virtual List<List<string>> GetCustomersByStreet(
        int limit,
        string streetName,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        StringBuilder query = new(
            @"
        SELECT c.*
        FROM Customer c
        JOIN Individual i ON c.account_id = i.account_id
        JOIN Address a ON i.address_id = a.address_id
        WHERE a.street LIKE @street"
        );

        query.Append(" ORDER BY ");
        query.Append(orderBy ?? "c.account_id");
        query.Append(orderDirection == true ? " ASC" : " DESC");
        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@street", $"%{streetName}%");
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> row = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader[i]?.ToString() ?? string.Empty);
            }
            results.Add(row);
        }

        return results;
    }

    #endregion Statistics
}
