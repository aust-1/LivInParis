using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for customer-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class CustomerRepository(LivInParisContext context)
    : Repository<Customer>(context),
        ICustomerRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Customer>> ReadAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null
    )
    {
        var min = minRating ?? 0m;
        var max = maxRating ?? 5m;

        var query = _context
            .Customers.Where(c => c.CustomerRating >= min)
            .Where(c => c.CustomerRating <= max);

        if (isBanned.HasValue)
        {
            query = query.Where(c => c.CustomerIsBanned == isBanned.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<
        IEnumerable<(Customer Customer, int OrderCount)>
    > GetCustomersByOrderCountAsync(DateTime? from = null, DateTime? to = null)
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineStatus == OrderLineStatus.Delivered)
            .Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Include(ol => ol.OrderTransaction)
            .ThenInclude(ot => ot!.Customer)
            .GroupBy(ol => ol.OrderTransaction!.Customer)
            .Select(g => new { Customer = g.Key, OrderCount = g.Count() })
            .OrderByDescending(g => g.OrderCount);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.Customer!, g.OrderCount));
    }

    /// <inheritdoc/>
    public async Task<
        IEnumerable<(Customer Customer, decimal TotalSpent)>
    > GetCustomersBySpendingAsync(DateTime? from = null, DateTime? to = null)
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context
            .OrderLines.Where(ol => ol.OrderLineStatus == OrderLineStatus.Delivered)
            .Where(ol => ol.OrderLineDatetime >= start)
            .Where(ol => ol.OrderLineDatetime <= end)
            .Join(
                _context.OrderTransactions,
                ol => ol.TransactionId,
                ot => ot.TransactionId,
                (ol, ot) => new { ol, ot }
            )
            .Join(
                _context.MenuProposals,
                olot => new
                {
                    ChefId = olot.ol.Chef!.AccountId,
                    Date = DateOnly.FromDateTime(olot.ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.AccountId, Date = mp.ProposalDate },
                (tmp, mp) =>
                    new
                    {
                        tmp.ol,
                        tmp.ot,
                        mp,
                    }
            )
            .Join(
                _context.Dishes,
                tmp => tmp.mp.DishId,
                d => d.DishId,
                (tmp, d) => new { tmp.ot.AccountId, d.Price }
            )
            .GroupBy(ap => ap.AccountId)
            .Select(g => new { AccountId = g.Key, Total = g.Sum(x => x.Price) })
            .Join(
                _context.Customers,
                g => g.AccountId,
                c => c.AccountId,
                (g, c) => new { Customer = c, TotalSpent = g.Total }
            )
            .OrderByDescending(g => g.TotalSpent);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.Customer, g.TotalSpent));
    }

    /// <inheritdoc/>
    public async Task<
        IEnumerable<(string CuisineNationality, int OrderCount)>
    > GetCustomerCuisinePreferencesAsync(
        Customer customer,
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
            .Include(ol => ol.OrderTransaction)
            .ThenInclude(ot => ot!.Customer)
            .Where(ol => ol.OrderTransaction!.Customer == customer)
            .Join(
                _context.MenuProposals,
                ol => new
                {
                    ChefId = ol.Chef!.AccountId,
                    Date = DateOnly.FromDateTime(ol.OrderLineDatetime),
                },
                mp => new { ChefId = mp.AccountId, Date = mp.ProposalDate },
                (ol, mp) => new { ol, mp }
            )
            .Join(
                _context.Dishes,
                tmp => tmp.mp.DishId,
                d => d.DishId,
                (tmp, d) =>
                    new
                    {
                        tmp.ol,
                        tmp.mp,
                        d,
                    }
            )
            .GroupBy(cd => cd.d.CuisineNationality)
            .Select(g => new { CuisineNationality = g.Key, OrderCount = g.Count() })
            .OrderByDescending(g => g.OrderCount);

        var raw = await query.ToListAsync();

        return raw.Select(g => (g.CuisineNationality!, g.OrderCount));
    }
}
