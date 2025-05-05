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
    public async Task<Account?> FindByEmailAsync(string email)
    {
        return await _context.Accounts.SingleOrDefaultAsync(a => a.AccountEmail == email);
    }
}
