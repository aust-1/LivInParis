using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IAdressService
{
    void CreateAdress(
        int addressId,
        int number,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    );

    List<List<string>> GetAdresses(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    );

    List<List<string>> RetrieveCommandCountByStreet(int limit, MySqlCommand? command = null);

    List<List<string>> RetrieveCommandCountByDistrict(int limit, MySqlCommand? command = null);

    void UpdateNearestMetro(int addressId, string nearestMetro, MySqlCommand? command = null);

    void DeleteAdress(string addressId, MySqlCommand? command = null);
}

//HACK: stats par rue, par arrondissement en € ou nb
