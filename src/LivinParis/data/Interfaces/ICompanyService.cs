using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ICompanyService
{
    void Create(
        int companyCustomerAccountId,
        string companyName,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        bool? companyIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateName(int companyCustomerAccountId, string companyName, MySqlCommand? command = null);

    void UpdateContact(
        int companyCustomerAccountId,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    void Delete(int companyCustomerAccountId, MySqlCommand? command = null);
}
