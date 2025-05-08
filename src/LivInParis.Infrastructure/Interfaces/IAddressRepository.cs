using LivInParisRoussilleTeynier.Domain.Models.Maps;
using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing address records in the database.
/// </summary>
public interface IAddressRepository : IRepository<Address> { }
