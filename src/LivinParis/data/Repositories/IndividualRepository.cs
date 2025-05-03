using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class IndividualRepository(LivInParisContext context)
    : Repository<Individual>(context),
        IIndividualRepository { }
