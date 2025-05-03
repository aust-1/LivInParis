using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class CompanyRepository(LivInParisContext context)
    : Repository<Company>(context),
        ICompanyRepository { }
