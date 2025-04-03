using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing user accounts in the system.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Creates a new user account with the specified information.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="email">The email address associated with the account.</param>
    /// <param name="password">The password associated with the account.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Create(int accountId, string email, string password, MySqlCommand? command = null);

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
    /// <param name="email">The new email address.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void UpdateEmail(int accountId, string email, MySqlCommand? command = null);

    /// <summary>
    /// Updates the password of an existing user account.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="password">The new password.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void UpdatePassword(int accountId, string password, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a user account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to delete.</param>
    /// <param name="command">Optional SQL command object for transaction support.</param>
    void Delete(int accountId, MySqlCommand? command = null);
}
