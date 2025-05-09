using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;

    /// <summary>
    /// Retrieves all reviews for a given chef.
    /// </summary>
    [HttpGet("chef/{chefId}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByChef(int chefId)
    {
        var reviews = await _reviewService.GetReviewsForChefAsync(chefId);
        return Ok(reviews);
    }

    /// <summary>
    /// Retrieves all reviews made by a given customer.
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByCustomer(int customerId)
    {
        var reviews = await _reviewService.GetReviewsByCustomerAsync(customerId);
        return Ok(reviews);
    }
}
