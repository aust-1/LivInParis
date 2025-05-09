using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for address-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class AddressRepository(LivInParisContext context)
    : Repository<Address>(context),
        IAddressRepository { }
