using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories
{
    public class ChefRepository(LivInParisContext context)
        : Repository<Chef>(context),
            IChefRepository
    {
        public async Task<IEnumerable<Chef>> ReadAsync(
            decimal? minRating = null,
            decimal? maxRating = null,
            bool? isBanned = null
        )
        {
            IQueryable<Chef> query = _context.Chefs;

            if (minRating.HasValue)
            {
                query = query.Where(c => c.ChefRating >= minRating.Value);
            }
            if (maxRating.HasValue)
            {
                query = query.Where(c => c.ChefRating <= maxRating.Value);
            }
            if (isBanned.HasValue)
            {
                query = query.Where(c => c.ChefIsBanned == isBanned.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersServedByChefAsync(
            Chef chef,
            DateTime? from = null,
            DateTime? to = null
        )
        {
            var query = _context
                .Customers.Include(c => c.OrderTransactions)
                .ThenInclude(ot => ot.OrderLines)
                .ThenInclude(ol => ol.Chef)
                .Where(c =>
                    c.OrderTransactions.Any(ot => ot.OrderLines.Any(ol => ol.Chef == chef))
                );

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(c =>
                    c.OrderTransactions.Any(ot =>
                        ot.TransactionDatetime >= from.Value && ot.TransactionDatetime <= to.Value
                    )
                );
            }

            return await query.ToListAsync();
        }

        public async Task<Dish?> GetTodayDishByChefAsync(Chef chef)
        {
            return await _context
                .Dishes.Include(d => d.MenuProposals)
                .FirstOrDefaultAsync(d =>
                    d.MenuProposals.Any(mp =>
                        mp.Chef == chef && mp.ProposalDate == DateOnly.FromDateTime(DateTime.Today)
                    )
                );
        }

        public async Task<Dictionary<Chef, int>> GetDeliveryCountByChefAsync(
            DateTime? from = null,
            DateTime? to = null
        )
        {
            if (!from.HasValue)
            {
                from = DateTime.MinValue;
            }
            if (!to.HasValue)
            {
                to = DateTime.MaxValue;
            }

            var query = _context
                .OrderLines.Where(
                    (ol) => ol.OrderLineDatetime >= from.Value && ol.OrderLineDatetime <= to.Value
                )
                .Include(ol => ol.Chef);

            return await query
                .GroupBy(ol => ol.Chef)
                .Select(g => new { Chef = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToDictionaryAsync(g => g.Chef, g => g.Count);
        }

        public async Task<Dictionary<Chef, decimal>> GetDeliveryCountValueByChefAsync(
            DateTime? from = null,
            DateTime? to = null
        )
        {
            if (!from.HasValue)
            {
                from = DateTime.MinValue;
            }
            if (!to.HasValue)
            {
                to = DateTime.MaxValue;
            }
            return await _context
                .OrderLines.Join(_context.Chefs, ol => ol.Chef, c => c, (ol, c) => new { ol, c })
                .Join(
                    _context.MenuProposals,
                    olc => olc.c,
                    mp => mp.Chef,
                    (olc, mp) =>
                        new
                        {
                            olc.ol,
                            olc.c,
                            mp,
                        }
                )
                .Where(olcmp =>
                    olcmp.ol.OrderLineDatetime >= from.Value
                    && olcmp.ol.OrderLineDatetime <= to.Value
                )
                .Where(olcmp =>
                    olcmp.mp.ProposalDate == DateOnly.FromDateTime(olcmp.ol.OrderLineDatetime)
                )
                .Join(
                    _context.Dishes,
                    olcmp => olcmp.mp.Dish,
                    d => d,
                    (olcmp, d) => new { olcmp.c, d }
                )
                .GroupBy(cd => cd.c)
                .Select(g => new { Chef = g.Key, Sum = g.Sum(cd => cd.d.Price) })
                .OrderByDescending(g => g.Sum)
                .ToDictionaryAsync(g => g.Chef, g => g.Sum);
        }
    }
}
