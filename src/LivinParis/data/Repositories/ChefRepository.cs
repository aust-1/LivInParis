using LivInParisRoussilleTeynier.Data.Interfaces;
using LivInParisRoussilleTeynier.Models.Order.Enums;
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
            if (!from.HasValue)
            {
                from = DateTime.MinValue;
            }
            if (!to.HasValue)
            {
                to = DateTime.MaxValue;
            }

            var query = _context
                .OrderLines.Include(ol => ol.OrderTransaction)
                .ThenInclude(ot => ot.Customer)
                .Where(ol => ol.Chef == chef)
                .Where(ol => ol.OrderLineDatetime >= from.Value)
                .Where(ol => ol.OrderLineDatetime <= to.Value)
                .Select(ol => ol.OrderTransaction.Customer);

            return await query.ToListAsync();
        }

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
                .Include(ol => ol.Chef)
                .GroupBy(ol => ol.Chef)
                .Select(g => new { Chef = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count);

            return await query.ToDictionaryAsync(g => g.Chef, g => g.Count);
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

            var query = _context
                .OrderLines.Where(ol =>
                    ol.OrderLineDatetime >= from.Value && ol.OrderLineDatetime <= to.Value
                )
                .Join(_context.Chefs, ol => ol.Chef, c => c, (ol, c) => new { ol, c })
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
                .OrderByDescending(g => g.Sum);

            return await query.ToDictionaryAsync(g => g.Chef, g => g.Sum);
        }
    }
}
