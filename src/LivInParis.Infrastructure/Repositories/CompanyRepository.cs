using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for company-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class CompanyRepository(LivInParisContext context)
    : Repository<Company>(context),
        ICompanyRepository { }
