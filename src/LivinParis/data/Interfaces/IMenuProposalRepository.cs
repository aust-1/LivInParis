using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage menu proposals for chefs.
/// </summary>
public interface IMenuProposalRepository : IRepository<MenuProposal>
{
    /// <summary>
    /// Retrieves the dishes proposed by a specific chef, ordered by frequency of proposal.
    /// </summary>
    /// <param name="chef">The chef whose proposals to retrieve.</param>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of dishs and their order counts.</returns>
    Task<IEnumerable<(Dish dish, int OrderCount)>> GetDeliveryCountPerDishByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    );
}
