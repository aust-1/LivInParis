using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Repositories.Implementations;

[ConnectionControl]
public class AdressRepository : IAdressService
{
    public virtual void CreateAdress(
        int idAdress,
        int number,
        string street,
        string nearestMetro,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "INSERT INTO Adress VALUES (@i, @n, @s, @m) ";
        command.Parameters.AddWithValue("@i", idAdress);
        command.Parameters.AddWithValue("@n", number);
        command.Parameters.AddWithValue("@s", street);
        command.Parameters.AddWithValue("@m", nearestMetro);
        command.ExecuteNonQuery();
    }

    public virtual List<List<string>> GetAdresses(int limit, MySqlCommand? command = null)
    {
        command!.CommandText = "SELECT * FROM Adress LIMIT @l";
        command.Parameters.AddWithValue("@l", limit);

        using var reader = command.ExecuteReader();
        List<List<string>> adresses = [];
        while (reader.Read())
        {
            List<string> adress = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                adress.Add(value);
            }
            adresses.Add(adress);
        }
        return adresses;
    }

    public virtual void UpdateNearestMetro(
        int idAdress,
        string nearestMetro,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Adress SET nearestMetro = @m WHERE adress_id = @a";
        command.Parameters.AddWithValue("@m", nearestMetro);
        command.Parameters.AddWithValue("@a", idAdress);
        command.ExecuteNonQuery();
    }

    public virtual void DeleteAdress(string idAdress, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Adress WHERE account_id = @a";
        command.Parameters.AddWithValue("@a", idAdress);
        command.ExecuteNonQuery();
    }
}
