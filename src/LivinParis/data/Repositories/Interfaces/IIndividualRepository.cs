using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IIndividual
{
    void CreateIndividual(
        int accountId,
        string lastName,
        string firstName,
        string email,
        string phoneNumber,
        int addressId,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetIndividuals(
        int limit,
        string? lastName = null,
        string? firstName = null,
        string? email = null,
        string? phoneNumber = null,
        string? street = null,
        int? postalCode = null,
        MySqlCommand? command = null
    );

    void UpdateIndividual(
        int accountId,
        string? lastName = null,
        string? firstName = null,
        string? email = null,
        string? phoneNumber = null,
        int? addressId = null,
        MySqlCommand? command = null
    );

    void DeleteIndividual(int idIndividual, MySqlCommand? command = null);
}

//HACK: stats par prenom, par nom
