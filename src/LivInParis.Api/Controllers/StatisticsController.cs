using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController(IStatisticsService statsService) : ControllerBase
{
    private readonly IStatisticsService _statsService = statsService;

    /// <summary>
    /// Gets delivery count per chef.
    /// </summary>
    [HttpGet("chef-deliveries")]
    public async Task<ActionResult<IEnumerable<ChefDeliveryStatsDto>>> GetChefDeliveries()
    {
        var stats = await _statsService.GetChefDeliveryStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Gets revenue grouped by street.
    /// </summary>
    [HttpGet("revenue-by-street")]
    public async Task<ActionResult<IEnumerable<RevenueByStreetDto>>> GetRevenueByStreet()
    {
        var stats = await _statsService.GetRevenueByStreetAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Gets average order price.
    /// </summary>
    [HttpGet("average-order-price")]
    public async Task<ActionResult<decimal>> GetAverageOrderPrice()
    {
        var avg = await _statsService.GetAverageOrderPriceAsync();
        return Ok(avg);
    }

    /// <summary>
    /// Gets top cuisines in a period.
    /// </summary>
    [HttpGet("top-cuisines")]
    public async Task<ActionResult<IEnumerable<CuisinePreferenceDto>>> GetTopCuisines(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to
    )
    {
        var stats = await _statsService.GetTopCuisinesAsync(from, to);
        return Ok(stats);
    }
}
