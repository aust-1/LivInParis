namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing user accounts in the system.
/// </summary>
public interface IAccountRepository : IRepository<Account>
{
    //TODO:Check Password

    /// <summary>
    /// Finds an account by its email.
    /// </summary>
    /// <param name="email">The email of the account to find.</param>
    /// <returns>A task that represents the asynchronous operation, containing the found account or null if not found.</returns>
    Task<Account?> FindByEmailAsync(string email);
}
