using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for account-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class AccountRepository(LivInParisContext context)
    : Repository<Account>(context),
        IAccountRepository
{
    /// <inheritdoc/>
    public async Task<Account?> ValidateCredentialsAsync(string userName, string password)
    {
        return await _context.Accounts.SingleOrDefaultAsync(a =>
            a.AccountUserName == userName && a.AccountPassword == password
        );
    }

    /// <inheritdoc/>
    public async Task<Account?> FindByUserNameAsync(string userName)
    {
        return await _context.Accounts.SingleOrDefaultAsync(a => a.AccountUserName == userName);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string userName)
    {
        return await _context.Accounts.AnyAsync(a => a.AccountUserName == userName);
    }
}
