using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for company-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class CompanyRepository(LivInParisContext context)
    : Repository<Company>(context),
        ICompanyRepository { }
