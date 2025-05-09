using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/customers/{customerId}/cart")]
public class CartController(ICartService cartService) : ControllerBase
{
    private readonly ICartService _cartService = cartService;

    /// <summary>
    /// Gets the shopping cart for a customer.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CartDto>> Get(int customerId)
    {
        var cart = await _cartService.GetCartAsync(customerId);
        return Ok(cart);
    }

    /// <summary>
    /// Adds an item to the cart.
    /// </summary>
    [HttpPost("items/{chefId}")]
    public async Task<IActionResult> Add(int customerId, int chefId)
    {
        await _cartService.AddItemAsync(customerId, chefId);
        return NoContent();
    }

    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    [HttpDelete("items/{chefId}")]
    public async Task<IActionResult> Remove(int customerId, int chefId)
    {
        await _cartService.RemoveItemAsync(customerId, chefId);
        return NoContent();
    }

    /// <summary>
    /// Clears the cart.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Clear(int customerId)
    {
        await _cartService.ClearCartAsync(customerId);
        return NoContent();
    }
}
