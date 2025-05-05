using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for review-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class ReviewRepository(LivInParisContext context)
    : Repository<Review>(context),
        IReviewRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Review>> ReadAsync(
        Account account,
        ReviewerType reviewerType,
        decimal? rating = null
    )
    {
        var query = _context.Reviews.Where(r => r.ReviewerType == reviewerType);

        if (reviewerType == ReviewerType.Customer)
        {
            var cr = new ChefRepository(_context);
            var chefs = await cr.ReadAsync(c => c.Account == account);
            Chef chef = chefs.First();

            query = query.Include(r => r.OrderLine).Where(r => r.OrderLine!.Chef == chef);
        }
        else
        {
            var cr = new CustomerRepository(_context);
            var customers = await cr.ReadAsync(c => c.Account == account);
            Customer customer = customers.First();

            query = query
                .Include(r => r.OrderLine)
                .ThenInclude(ol => ol!.OrderTransaction)
                .Where(r => r.OrderLine!.OrderTransaction!.Customer == customer);
        }

        if (rating.HasValue)
        {
            query = query.Where(r => r.ReviewRating == rating.Value);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal?> GetAverageRatingAsync(
        Account account,
        ReviewerType reviewerType,
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.MaxValue;

        var query = _context.Reviews.Where(r => r.ReviewerType == reviewerType);

        if (reviewerType == ReviewerType.Customer)
        {
            var cr = new ChefRepository(_context);
            var chefs = await cr.ReadAsync(c => c.Account == account);
            Chef chef = chefs.First();

            query = query.Include(r => r.OrderLine).Where(r => r.OrderLine!.Chef == chef);
        }
        else
        {
            var cr = new CustomerRepository(_context);
            var customers = await cr.ReadAsync(c => c.Account == account);
            Customer customer = customers.First();

            query = query
                .Include(r => r.OrderLine)
                .ThenInclude(ol => ol!.OrderTransaction)
                .Where(r => r.OrderLine!.OrderTransaction!.Customer == customer);
        }

        query = query
            .Where(r => r.OrderLine!.OrderLineDatetime >= start)
            .Where(r => r.OrderLine!.OrderLineDatetime <= end);

        return await query.AverageAsync(r => r.ReviewRating);
    }
}
