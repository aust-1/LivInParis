using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for address-related operations in the database.
/// </summary>
[ConnectionControl]
public class AddressService : IAddressService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? addressId,
        int addressNumber,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Address (address_id, address_number, street, postal_code, nearest_metro)
                VALUES (@id, @nb, @street, @postalCode, @metro)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", addressId);
        command.Parameters.AddWithValue("@nb", addressNumber);
        command.Parameters.AddWithValue("@street", street);
        command.Parameters.AddWithValue("@postalCode", postalCode);
        command.Parameters.AddWithValue("@metro", nearestMetro);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> addresses = [];
        List<string> conditions = [];
        command!.CommandText = "SELECT * FROM Address";

        if (street is not null)
        {
            conditions.Add("street = @street");
        }
        if (postalCode is not null)
        {
            conditions.Add("postal_code = @postalCode");
        }
        if (nearestMetro is not null)
        {
            conditions.Add("nearest_metro = @metro");
        }

        if (conditions.Count > 0)
        {
            command.CommandText += " WHERE " + string.Join(" AND ", conditions);
        }

        command.CommandText += " LIMIT @limit";

        command.Parameters.Clear();
        if (street is not null)
        {
            command.Parameters.AddWithValue("@street", street);
        }
        if (postalCode is not null)
        {
            command.Parameters.AddWithValue("@postalCode", postalCode);
        }
        if (nearestMetro is not null)
        {
            command.Parameters.AddWithValue("@metro", nearestMetro);
        }
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var address = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? string.Empty
                    : reader.GetValue(i).ToString() ?? string.Empty;
                address.Add(value);
            }
            addresses.Add(address);
        }

        return addresses;
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
    public virtual void Delete(int addressId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Address WHERE address_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", addressId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD
}
