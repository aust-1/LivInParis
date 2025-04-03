using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for address-related operations in the database.
/// </summary>
[ConnectionControl]
public class AdressService : IAdressService
{
    /// <inheritdoc/>
    public virtual void CreateAdress(
        int addressId,
        int number,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Address (address_id, number, street, postal_code, nearest_metro)
                VALUES (@id, @number, @street, @postalCode, @metro)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", addressId);
        command.Parameters.AddWithValue("@number", number);
        command.Parameters.AddWithValue("@street", street);
        command.Parameters.AddWithValue("@postalCode", postalCode);
        command.Parameters.AddWithValue("@metro", nearestMetro);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetAdresses(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> adresses = [];
        List<string> conditions = [];
        command!.CommandText = "SELECT * FROM Address";

        if (street is not null)
            conditions.Add("street = @street");
        if (postalCode is not null)
            conditions.Add("postal_code = @postalCode");
        if (nearestMetro is not null)
            conditions.Add("nearest_metro = @metro");

        if (conditions.Count > 0)
            command.CommandText += " WHERE " + string.Join(" AND ", conditions);

        command.CommandText += " LIMIT @limit";

        command.Parameters.Clear();
        if (street is not null)
            command.Parameters.AddWithValue("@street", street);
        if (postalCode is not null)
            command.Parameters.AddWithValue("@postalCode", postalCode);
        if (nearestMetro is not null)
            command.Parameters.AddWithValue("@metro", nearestMetro);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var row = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? string.Empty
                    : reader.GetValue(i).ToString() ?? string.Empty;
                row.Add(value);
            }
            adresses.Add(row);
        }

        return adresses;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> RetrieveCommandCountByStreet(
        int limit,
        MySqlCommand? command = null
    )
    {
        var results = new List<List<string>>();

        command!.CommandText =
            @"
                SELECT street, COUNT(command_id) AS command_count
                FROM Address
                JOIN Client ON Address.address_id = Client.address_id
                JOIN Command ON Client.client_id = Command.client_id
                GROUP BY street
                ORDER BY command_count DESC
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var row = new List<string>
            {
                reader["street"].ToString() ?? string.Empty,
                reader["command_count"].ToString() ?? string.Empty,
            };
            results.Add(row);
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> RetrieveCommandCountByDistrict(
        int limit,
        MySqlCommand? command = null
    )
    {
        var results = new List<List<string>>();

        command!.CommandText =
            @"
                SELECT postal_code, COUNT(command_id) AS command_count
                FROM Address
                JOIN Client ON Address.address_id = Client.address_id
                JOIN Command ON Client.client_id = Command.client_id
                GROUP BY postal_code
                ORDER BY command_count DESC
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var row = new List<string>
            {
                reader["postal_code"].ToString() ?? string.Empty,
                reader["command_count"].ToString() ?? string.Empty,
            };
            results.Add(row);
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual void UpdateNearestMetro(
        int addressId,
        string nearestMetro,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Address SET nearest_metro = @metro WHERE address_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", addressId);
        command.Parameters.AddWithValue("@metro", nearestMetro);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void DeleteAdress(string addressId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Address WHERE address_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", addressId);
        command.ExecuteNonQuery();
    }
}
