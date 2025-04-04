using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing user accounts in the system.
/// </summary>
public interface IAccountService
{
    #region CRUD
    //TODO:Check Password
    /// <summary>
    /// Creates a new user account with the specified information.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="accountEmail">The email address associated with the account.</param>
    /// <param name="accountPassword">The password associated with the account.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Create(
        int? accountId,
        string accountEmail,
        string accountPassword,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of user accounts from the database.
    /// </summary>
    /// <param name="limit">The maximum number of accounts to retrieve.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    /// <returns>A list of accounts, each represented as a list of strings.</returns>
    List<List<string>> Read(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Updates the email address of an existing user account.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="accountEmail">The new email address.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void UpdateEmail(int accountId, string accountEmail, MySqlCommand? command = null);

    /// <summary>
    /// Updates the password of an existing user account.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="accountPassword">The new password.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void UpdatePassword(int accountId, string accountPassword, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a user account by its email.
    /// </summary>
    /// <param name="accountEmail">The email of the account to delete.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Delete(string accountEmail, MySqlCommand? command = null);

    #endregion CRUD
}
