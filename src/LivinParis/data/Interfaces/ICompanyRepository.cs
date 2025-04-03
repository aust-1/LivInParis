using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ICompany
{
    void CreateCompany(
        int companyCustomerAccountId,
        string companyName,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    List<List<string>> GetCompanies(
        int limit,
        bool? companyIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateCompanyName(
        int companyCustomerAccountId,
        string companyName,
        MySqlCommand? command = null
    );

    void UpdateCompanyContact(
        int companyCustomerAccountId,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    void DeleteCompany(int companyCustomerAccountId, MySqlCommand? command = null);
}
