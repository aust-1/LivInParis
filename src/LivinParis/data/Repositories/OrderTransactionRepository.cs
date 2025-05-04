using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for order transaction-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class OrderTransactionRepository(LivInParisContext context)
    : Repository<OrderTransaction>(context),
        IOrderTransactionRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<OrderTransaction>> ReadAsync(
        Customer? customer = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Join(
                _context.MenuProposals,
                ol => new
                {
                    ChefId = ol.Chef!.AccountId,
                    Date = DateOnly.FromDateTime(ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.AccountId, Date = mp.ProposalDate },
                (ol, mp) => new { ol.OrderTransaction, mp.Dish!.Price }
            )
            .GroupBy(g => g.OrderTransaction)
            .Select(g => new { ot = g.Key, Total = g.Sum(x => x.Price) });

        if (minTotalPrice.HasValue)
        {
            query = query.Where(t => t.Total >= minTotalPrice.Value);
        }

        if (maxTotalPrice.HasValue)
        {
            query = query.Where(t => t.Total <= maxTotalPrice.Value);
        }

        if (customer != null)
        {
            query = query.Where(ott => ott.ot!.Customer == customer);
        }

        var raw = await query.ToListAsync();

        return raw.Select(g => g.ot!);
    }

    /// <inheritdoc/>
    public async Task<decimal> GetOrderTotalPriceAsync(OrderTransaction transaction)
    {
        var query = _context
            .OrderLines.Where(ol => ol.OrderTransaction == transaction)
            .Join(
                _context.MenuProposals,
                ol => new
                {
                    ChefId = ol.Chef!.AccountId,
                    Date = DateOnly.FromDateTime(ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.AccountId, Date = mp.ProposalDate },
                (ol, mp) => new { ol, mp }
            );

        return await query.SumAsync(olmp => olmp.mp.Dish!.Price);
    }
}
