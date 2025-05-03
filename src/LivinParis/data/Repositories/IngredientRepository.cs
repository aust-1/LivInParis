using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class IngredientRepository(LivInParisContext context)
    : Repository<Ingredient>(context),
        IIngredientRepository { }
