using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for individual-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class IndividualRepository(LivInParisContext context)
    : Repository<Individual>(context),
        IIndividualRepository
{ }
