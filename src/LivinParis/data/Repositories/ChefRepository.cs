using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories
{
    public class ChefRepository : Repository<Chef>, IChefRepository
    {
        public ChefRepository(LivInParisContext context)
            : base(context) { }

        public async Task<IEnumerable<Chef>> ReadAsync(
            decimal? minRating = null,
            decimal? maxRating = null,
            bool? isBanned = null
        )
        {
            IQueryable<Chef> query = _context.Chefs;

            if (minRating.HasValue)
                query = query.Where(c => c.ChefRating >= minRating.Value);
            if (maxRating.HasValue)
                query = query.Where(c => c.ChefRating <= maxRating.Value);
            if (isBanned.HasValue)
                query = query.Where(c => c.ChefIsBanned == isBanned.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersServedByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null)
        {
            var query = _context.Customers
                .Include(c => c.OrderTransactions)
                .ThenInclude(ot => ot.OrderLines)
                .ThenInclude(ol => ol.Chef)
                .Where(c => c.OrderTransactions.Any(ot => ot.OrderLines.Any(ol => ol.Chef == chef)));

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(c => c.OrderTransactions.Any(ot => ot.TransactionDatetime >= from.Value && ot.TransactionDatetime <= to.Value));
            }

            return await query.ToListAsync();
        }

        public async Task<Dish?> GetTodayDishByChefAsync(Chef chef)
        {
            return await _context.Dishes
                .Include(d => d.MenuProposals)
                .FirstOrDefaultAsync(d => d.MenuProposals.Any(
                    mp => mp.Chef == chef
                    && mp.ProposalDate == DateOnly.FromDateTime(DateTime.Today)));
        }

        public async Task<IEnumerable<(Chef, int)>> GetDeliveryCountByChefAsync(
            DateTime? from = null,
            DateTime? to = null
        )
        {
            var query = _context.Chefs
                .Include(c => c.OrderLines)
                .Where(c => c.OrderLines != null);

            if (!from.HasValue)
            {
                from = DateTime.MinValue;
            }
            if (!to.HasValue)
            {
                to = DateTime.MaxValue;
            }

            query = query.Where(c => c.OrderLines.Any(ol => ol.OrderLineDatetime >= from.Value && ol.OrderLineDatetime <= to.Value));
            
//FIXME: en cours
            return await query.Select(c => new { Chef = c, Count = c.OrderLines.Count() }).ToListAsync();
        }

        public async Task<IEnumerable<(Chef, int)>> GetDeliveryCountValueByChefAsync(
            DateTime? from = null,
            DateTime? to = null
        )
        {
            var query = _context.Chefs
                .Include(c => c.OrderLines)
                .ThenInclude(ol => ol.Order);

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(c => c.OrderLines.Any(ol => ol.Order.Date >= from.Value && ol.Order.Date <= to.Value));
            }

            return await query.Select(c => new { Chef = c, Value = c.OrderLines.Sum(ol => ol.Price) }).ToListAsync();
        }
    }
}
}
