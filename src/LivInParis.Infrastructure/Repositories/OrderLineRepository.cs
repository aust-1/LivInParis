using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

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
        var query = _context
            .OrderLines.Where(ol => ol.OrderTransaction == transaction)
            .Include(ol => ol.Address);

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderLine>> ReadAsync(
        int? chefId = null,
        int? customerId = null,
        OrderLineStatus? status = null,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        IQueryable<OrderLine> query = _context
            .OrderLines.Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Include(ol => ol.Address);

        if (chefId != null)
        {
            query = query.Where(ol => ol.ChefAccountId == chefId);
        }

        if (customerId != null)
        {
            query = query.Where(ol => ol.OrderTransaction!.CustomerAccountId == customerId);
        }

        if (status.HasValue)
        {
            query = query.Where(ol => ol.OrderLineStatus == status.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public Task<Dish> GetOrderDishAsync(OrderLine orderLine)
    {
        var query = _context
            .OrderLines.Where(ol => ol == orderLine)
            .Join(
                _context.MenuProposals,
                ol => new { ChefId = ol.Chef, Date = DateOnly.FromDateTime(ol.OrderLineDatetime) },
                mp => new { ChefId = mp.Chef, Date = mp.ProposalDate },
                (ol, mp) => new { ol, mp }
            );

        return query.Select(olmp => olmp.mp.Dish!).FirstAsync();
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
