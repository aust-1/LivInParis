using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing user accounts in the system.
/// </summary>
public interface IAccountRepository : IRepository<Account>
{
    //TODO:Check Password

    /// <summary>
    /// Finds an account by its user name.
    /// </summary>
    /// <param name="userName">The user name of the account to find.</param>
    /// <returns>A task that represents the asynchronous operation, containing the found account or null if not found.</returns>
    Task<Account?> FindByUserNameAsync(string userName);
}
