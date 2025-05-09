using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/chefs/{chefId}/orders")]
public class IncomingOrdersController(IIncomingOrderService incomingService) : ControllerBase
{
    private readonly IIncomingOrderService _incomingService = incomingService;

    /// <summary>
    /// Retrieves pending incoming orders for a chef.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderLineDto>>> GetAll(int chefId)
    {
        var orders = await _incomingService.GetIncomingOrdersAsync(chefId);
        return Ok(orders);
    }

    /// <summary>
    /// Accepts an incoming order.
    /// </summary>
    [HttpPost("{orderId}/accept")]
    public async Task<IActionResult> Accept(int orderId)
    {
        await _incomingService.AcceptOrderAsync(orderId);
        return NoContent();
    }

    /// <summary>
    /// Rejects an incoming order.
    /// </summary>
    [HttpPost("{orderId}/reject")]
    public async Task<IActionResult> Reject(int orderId)
    {
        await _incomingService.RejectOrderAsync(orderId);
        return NoContent();
    }

    /// <summary>
    /// Updates order status.
    /// </summary>
    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] OrderStatusDto dto)
    {
        await _incomingService.UpdateOrderStatusAsync(orderId, dto);
        return NoContent();
    }
}
