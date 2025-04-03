using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IAdress
{
    void CreateAdress(
        int addressId,
        int number,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetAdresses(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    );

    Dictionary<string, int> RetrieveCommandCountByStreet(int limit, MySqlCommand? command = null);

    Dictionary<int, int> RetrieveCommandCountByDistrict(int limit, MySqlCommand? command = null);

    void UpdateNearestMetro(int addressId, string nearestMetro, MySqlCommand? command = null);

    void DeleteAdress(string addressId, MySqlCommand? command = null);
}

//HACK: stats par rue, par arrondissement en € ou nb
