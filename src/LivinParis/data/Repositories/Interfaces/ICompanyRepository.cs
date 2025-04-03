using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface ICompany
{
    void CreateCompany(
        int accountId,
        string companyName,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetCompanies(
        int limit,
        bool? companyIsBanned = null,
        string? orderBy = null,
        MySqlCommand? command = null
    );

    void UpdateCompanyName(int accountId, string companyName, MySqlCommand? command = null);

    void UpdateCompanyContact(
        int accountId,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    void DeleteCompany(int accountId, MySqlCommand? command = null);
}
