using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods to manage menu proposals for chefs.
/// </summary>
public interface IMenuProposalRepository : IRepository<MenuProposal>
{
    /// <summary>
    /// Retrieves the dishes proposed by a specific chef, ordered by frequency of proposal.
    /// </summary>
    /// <param name="chef">The chef whose proposals to retrieve.</param>
    /// <param name="from">
    /// The start of the period to include. If null, includes all deliveries from the beginning of time.
    /// </param>
    /// <param name="to">
    /// The end of the period to include. If null, includes all deliveries up to the end of time.
    /// </param>
    /// <returns>A task that represents the asynchronous operation, containing a list of dishs and their order counts.</returns>
    Task<IEnumerable<(Dish Dish, int OrderCount)>> GetDeliveryCountPerDishByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    );
}
