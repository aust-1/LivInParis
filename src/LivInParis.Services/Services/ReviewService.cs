using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="ReviewService"/>.
/// </summary>
public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    /// <inheritdoc/>
    public async Task<List<ReviewDto>> GetReviewsForChefAsync(int chefId)
    {
        var reviews = await _reviewRepository.ReadAsync(chefId, ReviewerType.Customer);
        return reviews
            .Select(r => new ReviewDto
            {
                Id = r.ReviewId,
                ChefId = chefId,
                CustomerId = r.OrderLine!.OrderTransaction!.CustomerAccountId,
                Rating = r.ReviewRating,
                Comment = r.Comment,
                CreatedAt = r.OrderLine!.OrderLineDatetime,
            })
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<List<ReviewDto>> GetReviewsByCustomerAsync(int customerId)
    {
        var reviews = await _reviewRepository.ReadAsync(customerId, ReviewerType.Chef);
        return reviews
            .Select(r => new ReviewDto
            {
                Id = r.ReviewId,
                ChefId = r.OrderLine!.ChefAccountId,
                CustomerId = customerId,
                Rating = r.ReviewRating,
                Comment = r.Comment,
                CreatedAt = r.OrderLine!.OrderLineDatetime,
            })
            .ToList();
    }
}
