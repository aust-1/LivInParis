using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services.Interfaces;

/// <summary>
/// Provides methods for managing user accounts, including registration, authentication, retrieval, updating, and deletion.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Retrieves an account by its username.
    /// </summary>
    /// <param name="userName">The username of the account.</param>
    /// <returns>A task representing the asynchronous operation, containing the Account if found; otherwise, null.</returns>
    Task<Account?> GetAccountByUserNameAsync(string userName);

    /// <summary>
    /// Registers a new individual account with the specified personal details and password.
    /// </summary>
    /// <param name="individual">The individual entity containing personal information.</param>
    /// <param name="password">The password for the new account.</param>
    /// <returns>A task representing the asynchronous operation, containing the registered individual with its associated account.</returns>
    Task<Individual> RegisterIndividualAsync(Individual individual, string password);

    /// <summary>
    /// Registers a new company account with the specified company details and password.
    /// </summary>
    /// <param name="company">The company entity containing company information.</param>
    /// <param name="password">The password for the new account.</param>
    /// <returns>A task representing the asynchronous operation, containing the registered company with its associated account.</returns>
    Task<Company> RegisterCompanyAsync(Company company, string password);

    /// <summary>
    /// Authenticates a user by verifying the provided username and password.
    /// </summary>
    /// <param name="userName">The username of the account.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the authenticated Account if successful; otherwise, null.
    /// </returns>
    Task<Account?> AuthenticateAsync(string userName, string password);

    /// <summary>
    /// Retrieves an account by its unique identifier.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <returns>A task representing the asynchronous operation, containing the Account or null if not found.</returns>
    Task<Account?> GetByIdAsync(int accountId);

    /// <summary>
    /// Updates the details of an existing account.
    /// </summary>
    /// <param name="account">The account entity with updated information.</param>
    /// <returns>
    /// The updated Account.
    /// </returns>
    Account Update(Account account);

    /// <summary>
    /// Deletes an account by its unique identifier.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account to delete.</param>
    void Delete(int accountId);
}
