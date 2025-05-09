using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderLineController(IOrderLineService orderLineService) : ControllerBase
{
    private readonly IOrderLineService _orderLineService = orderLineService;

    /// <summary>
    /// Retrieves an order line by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderLineDto>> GetById(int id)
    {
        var line = await _orderLineService.GetOrderLineByIdAsync(id);
        return Ok(line);
    }

    /// <summary>
    /// Cancels an order line.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _orderLineService.CancelOrderLineAsync(id);
        return NoContent();
    }
}
