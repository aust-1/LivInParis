using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IAddressService
{
    void Create(
        int addressId,
        int number,
        string street,
        int postalCode,
        string nearestMetro,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        string? street = null,
        int? postalCode = null,
        string? nearestMetro = null,
        MySqlCommand? command = null
    );

    void UpdateNearestMetro(int addressId, string nearestMetro, MySqlCommand? command = null);

    void Delete(string addressId, MySqlCommand? command = null);
}
