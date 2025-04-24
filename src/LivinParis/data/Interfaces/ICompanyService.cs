using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing company customer accounts.
/// </summary>
public interface ICompanyService
{
    #region CRUD

    /// <summary>
    /// Creates a new company customer account with the specified information.
    /// </summary>
    /// <param name="companyCustomerAccountId">The account ID of the company (linked to the Account table).</param>
    /// <param name="companyName">The name of the company.</param>
    /// <param name="contactFirstName">The first name of the contact person.</param>
    /// <param name="contactLastName">The last name of the contact person.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int? companyCustomerAccountId,
        string companyName,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of company customer accounts with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="companyIsBanned">Optional filter to select banned or non-banned companies.</param>
    /// <param name="orderBy">Optional column to order the result set by.</param>
    /// <param name="orderDirection">True for ascending, false for descending ordering.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing company rows.</returns>
    List<List<string>> Read(
        int limit,
        bool? companyIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Upates the name of a company customer account.
    /// </summary>
    /// <param name="companyCustomerAccountId">The account ID of the company (linked to the Account table).</param>
    /// <param name="companyName">The new name of the company.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateName(int companyCustomerAccountId, string companyName, MySqlCommand? command = null);

    /// <summary>
    /// Updates the contact information of a company customer account.
    /// </summary>
    /// <param name="companyCustomerAccountId">Company customer account ID (linked to the Account table).</param>
    /// <param name="contactFirstName">The new first name of the contact person.</param>
    /// <param name="contactLastName">The new last name of the contact person.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateContact(
        int companyCustomerAccountId,
        string contactFirstName,
        string contactLastName,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes a company customer account by its ID.
    /// </summary>
    /// <param name="companyCustomerAccountId">The account ID of the company (linked to the Account table).</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int companyCustomerAccountId, MySqlCommand? command = null);

    #endregion CRUD
}
