using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IIndividual
{
    void CreateIndividual(
        int individualCustomerAccountId,
        string lastName,
        string firstName,
        string personalEmail,
        string phoneNumber,
        int addressId,
        MySqlCommand? command = null
    );

    List<List<string>> GetIndividuals(
        int limit,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        string? street = null,
        int? postalCode = null,
        MySqlCommand? command = null
    );

    void UpdateIndividual(
        int individualCustomerAccountId,
        string? lastName = null,
        string? firstName = null,
        string? personalEmail = null,
        string? phoneNumber = null,
        int? addressId = null,
        MySqlCommand? command = null
    );

    void DeleteIndividual(int individualCustomerAccountId, MySqlCommand? command = null);
}

//HACK: stats par prenom, par nom
