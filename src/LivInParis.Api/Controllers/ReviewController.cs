using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;

    /// <summary>
    /// Gets reviews for an account.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(
        [FromQuery] int accountId,
        [FromQuery] string reviewerType,
        [FromQuery] decimal? rating
    )
    {
        var reviews = await _reviewService.GetReviewsAsync(accountId, reviewerType, rating);
        return Ok(reviews);
    }

    /// <summary>
    /// Creates a review for an order line.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto)
    {
        var review = await _reviewService.CreateReviewAsync(dto);
        return CreatedAtAction(nameof(GetReviews), new { review.Id }, review);
    }
}
