using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for individual-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class IndividualRepository(LivInParisContext context)
    : Repository<Individual>(context),
        IIndividualRepository { }
