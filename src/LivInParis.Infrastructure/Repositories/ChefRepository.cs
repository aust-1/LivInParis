using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for chef-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class ChefRepository(LivInParisContext context) : Repository<Chef>(context), IChefRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Chef>> ReadAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null
    )
    {
        var min = minRating ?? 0m;
        var max = maxRating ?? 5m;

        var query = _context.Chefs.Where(c => c.ChefRating >= min).Where(c => c.ChefRating <= max);

        if (isBanned.HasValue)
        {
            query = query.Where(c => c.ChefIsBanned == isBanned.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Customer?>> GetCustomersServedByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Include(ol => ol.OrderTransaction)
            .ThenInclude(ot => ot!.Customer)
            .Where(ol => ol.Chef == chef)
            .Where(ol => ol.OrderLineStatus == OrderLineStatus.Delivered)
            .Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Select(ol => ol.OrderTransaction!.Customer);

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Dish?> GetTodayDishByChefAsync(Chef chef)
    {
        var query = _context
            .MenuProposals.Include(mp => mp.Dish)
            .Where(mp =>
                mp.Chef == chef && mp.ProposalDate == DateOnly.FromDateTime(DateTime.Today)
            )
            .Select(mp => mp.Dish);

        return await query.SingleOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<(Chef Chef, int OrderCount)>> GetDeliveryCountByChefAsync(
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
            .Include(ol => ol.Chef)
            .GroupBy(ol => ol.Chef)
            .Select(g => new { Chef = g.Key, OrderCount = g.Count() })
            .OrderByDescending(g => g.OrderCount);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.Chef!, g.OrderCount));
    }

    /// <inheritdoc/>
    public async Task<
        IEnumerable<(Chef Chef, decimal TotalSpent)>
    > GetDeliveryCountValueByChefAsync(DateTime? from = null, DateTime? to = null)
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineStatus == OrderLineStatus.Delivered)
            .Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Join(_context.Chefs, ol => ol.Chef, c => c, (ol, c) => new { ol, c })
            .Join(
                _context.MenuProposals,
                olc => new
                {
                    ChefId = olc.c.ChefAccountId,
                    Date = DateOnly.FromDateTime(olc.ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.ChefAccountId, Date = mp.ProposalDate },
                (olc, mp) =>
                    new
                    {
                        olc.ol,
                        olc.c,
                        mp,
                    }
            )
            .Join(_context.Dishes, olcmp => olcmp.mp.Dish, d => d, (olcmp, d) => new { olcmp.c, d })
            .GroupBy(cd => cd.c)
            .Select(g => new { Chef = g.Key, TotalSpent = g.Sum(cd => cd.d.Price) })
            .OrderByDescending(g => g.TotalSpent);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.Chef!, g.TotalSpent));
    }
}
