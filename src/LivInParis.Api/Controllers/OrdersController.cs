using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Services;
using LivInParisRoussilleTeynier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IOrderLineService _orderService;

    public OrdersController(ICustomerService customerService, IOrderLineService orderService)
    {
        _customerService = customerService;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] OrderRequest request)
    {
        // TODO: map OrderRequest to domain, call _orderService.PlaceOrderAsync
        await _orderService.PlaceOrderAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = request.Id }, null);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<OrderTransaction>>> GetMyOrders()
    {
        // TODO: extract customerId from auth
        var customerId = 1;
        var orders = await _customerService.GetOrdersForCustomerAsync(customerId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderTransaction>> GetById(int id)
    {
        var order = await _orderService.GetOrderDetailAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }
}
