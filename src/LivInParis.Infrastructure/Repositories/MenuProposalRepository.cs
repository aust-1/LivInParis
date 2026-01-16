using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for menu proposal-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class MenuProposalRepository(LivInParisContext context)
    : Repository<MenuProposal>(context),
        IMenuProposalRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<(Dish Dish, int OrderCount)>> GetDeliveryCountPerDishByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineStatus == OrderLineStatus.Delivered)
            .Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Where(ol => ol.Chef == chef)
            .Join(
                _context.MenuProposals,
                ol => new
                {
                    ChefId = ol.Chef!.ChefAccountId,
                    Date = DateOnly.FromDateTime(ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.ChefAccountId, Date = mp.ProposalDate },
                (ol, mp) => new { ol, mp }
            )
            .GroupBy(olmp => olmp.mp.Dish)
            .Select(g => new { g.First().mp.Dish, OrderCount = g.Count() })
            .OrderByDescending(g => g.OrderCount);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.Dish!, g.OrderCount));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MenuProposal>> GetProposalsByChefAsync(int chefId) =>
        await _context
            .MenuProposals.Where(mp => mp.ChefAccountId == chefId)
            .Include(mp => mp.Dish)
            .ThenInclude(d => d!.Contains)
            .ThenInclude(c => c.Ingredient)
            .ToListAsync();

}
