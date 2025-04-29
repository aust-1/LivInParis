using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for account-related operations.
/// </summary>
[ConnectionControl]
public class AccountRepository(LivInParisContext context)
    : Repository<Account>(context),
        IAccountRepository
{
    public async Task<Account?> FindByEmailAsync(string email)
    {
        return await _context.Accounts.SingleOrDefaultAsync(a => a.AccountEmail == email);
    }
}
