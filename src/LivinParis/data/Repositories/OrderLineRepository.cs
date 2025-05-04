using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for order line-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class OrderLineRepository(LivInParisContext context)
    : Repository<OrderLine>(context),
        IOrderLineRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<OrderLine>> ReadAsync(OrderTransaction transaction)
    {
        var query = _context.OrderLines.Where(ol => ol.OrderTransaction == transaction);

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderLine>> ReadAsync(
        Chef? chef = null,
        Customer? customer = null,
        OrderLineStatus? status = null,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end);

        if (chef != null)
        {
            query = query.Where(ol => ol.Chef == chef);
        }

        if (customer != null)
        {
            query = query.Where(ol => ol.OrderTransaction!.Customer == customer);
        }

        if (status.HasValue)
        {
            query = query.Where(ol => ol.OrderLineStatus == status.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> GetAverageOrderPriceAsync(
        Chef? chef = null,
        Customer? customer = null,
        OrderLineStatus? status = null,
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
                ol => new { ChefId = ol.Chef, Date = DateOnly.FromDateTime(ol.OrderLineDatetime) },
                mp => new { ChefId = mp.Chef, Date = mp.ProposalDate },
                (ol, mp) => new { ol, mp }
            );

        if (chef != null)
        {
            query = query.Where(olmp => olmp.ol.Chef == chef);
        }

        if (customer != null)
        {
            query = query.Where(olmp => olmp.ol.OrderTransaction!.Customer == customer);
        }

        if (status.HasValue)
        {
            query = query.Where(olmp => olmp.ol.OrderLineStatus == status.Value);
        }

        return await query.AverageAsync(olmp => olmp.mp.Dish!.Price);
    }
}
