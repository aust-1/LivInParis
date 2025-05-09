using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing user accounts in the system.
/// </summary>
public interface IAccountRepository : IRepository<Account>
{
    /// <summary>
    /// Validates the provided credentials against the stored account information.
    /// </summary>
    /// <param name="userName">The user name of the account.</param>
    /// <param name="password">The password of the account.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the authenticated Account if successful; otherwise, null.
    /// </returns>
    Task<Account?> ValidateCredentialsAsync(string userName, string password);

    /// <summary>
    /// Finds an account by its user name.
    /// </summary>
    /// <param name="userName">The user name of the account to find.</param>
    /// <returns>A task that represents the asynchronous operation, containing the found account or null if not found.</returns>
    Task<Account?> FindByUserNameAsync(string userName);

    /// <summary>
    /// Checks if an account with the specified user name already exists.
    /// </summary>
    /// <param name="userName">The user name of the account to check.</param>
    /// <returns>A task that represents the asynchronous operation, containing true if the account exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(string userName);
}
