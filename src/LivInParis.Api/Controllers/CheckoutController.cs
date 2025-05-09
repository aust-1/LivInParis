using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController(ICheckoutService checkoutService) : ControllerBase
{
    private readonly ICheckoutService _checkoutService = checkoutService;

    /// <summary>
    /// Places an order based on the customer's cart.
    /// </summary>
    [HttpPost("customers/{customerId}")]
    public async Task<ActionResult<TransactionDto>> PlaceOrder(
        int customerId,
        [FromBody] CheckoutDto dto
    )
    {
        var transaction = await _checkoutService.PlaceOrderAsync(customerId, dto);
        return CreatedAtAction(
            nameof(TransactionController.GetById),
            "Transaction",
            new { id = transaction.Id },
            transaction
        );
    }
}
