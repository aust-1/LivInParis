using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="ReviewService"/>.
/// </summary>
public class ReviewService(IReviewRepository reviewRepository, IAccountRepository accountRepository)
    : IReviewService
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly IAccountRepository _accountRepository = accountRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<ReviewDto>> GetReviewsAsync(
        int accountId,
        string reviewerType,
        decimal? rating = null
    )
    {
        var account =
            await _accountRepository.GetByIdAsync(accountId)
            ?? throw new ArgumentException("Account not found", nameof(accountId));
        var type = ParseReviewerType(reviewerType);
        var reviews = await _reviewRepository.ReadAsync(account, type, rating);

        return reviews.Select(review => new ReviewDto
        {
            Id = review.ReviewId,
            OrderLineId = review.OrderLineId,
            ReviewerType = review.ReviewerType.ToString(),
            Rating = review.ReviewRating,
            Comment = review.Comment,
            ReviewDate = review.ReviewDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
        });
    }

    /// <inheritdoc/>
    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto)
    {
        var type = ParseReviewerType(dto.ReviewerType);
        var review = new Review
        {
            OrderLineId = dto.OrderLineId,
            ReviewerType = type,
            ReviewRating = dto.Rating,
            Comment = dto.Comment,
            ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow),
        };

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();

        return new ReviewDto
        {
            Id = review.ReviewId,
            OrderLineId = review.OrderLineId,
            ReviewerType = review.ReviewerType.ToString(),
            Rating = review.ReviewRating,
            Comment = review.Comment,
            ReviewDate = review.ReviewDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
        };
    }

    private static ReviewerType ParseReviewerType(string? reviewerType)
    {
        if (string.IsNullOrWhiteSpace(reviewerType))
        {
            throw new ArgumentException("ReviewerType is required", nameof(reviewerType));
        }

        if (!Enum.TryParse<ReviewerType>(reviewerType, true, out var parsed))
        {
            throw new ArgumentException("Invalid reviewer type", nameof(reviewerType));
        }

        return parsed;
    }
}

