using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="CheckoutService"/>.
/// </summary>
public class CheckoutService(
    IOrderLineRepository orderLineRepository,
    IOrderTransactionRepository transactionRepository
) : ICheckoutService
{
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;
    private readonly IOrderTransactionRepository _transactionRepository = transactionRepository;

    /// <inheritdoc/>
    public async Task<TransactionDto> PlaceOrderAsync(int customerId, CheckoutDto checkoutDto)
    {
        var cart =
            await _transactionRepository.GetCurrentTransactionAsync(customerId)
            ?? throw new ArgumentException("No cart found for the customer.");
        cart.TransactionDatetime = DateTime.Now;
        _transactionRepository.Update(cart);

        var orderLines = await _orderLineRepository.ReadAsync(cart);
        foreach (var orderLine in orderLines)
        {
            orderLine.OrderLineStatus = OrderLineStatus.Pending;
            orderLine.AddressId = checkoutDto.AddressId;
            orderLine.OrderLineDatetime = DateTime.Now;
            _orderLineRepository.Update(orderLine);
        }

        return new TransactionDto
        {
            Id = cart.TransactionId,
            CustomerId = customerId,
            Lines =
            [
                .. orderLines.Select(ol => new OrderLineDto
                {
                    DishId = _orderLineRepository.GetOrderDishAsync(ol).Result.DishId,
                    DishName = _orderLineRepository.GetOrderDishAsync(ol).Result.DishName,
                    Status = ol.OrderLineStatus.ToString(),
                    UnitPrice = _orderLineRepository.GetOrderDishAsync(ol).Result.Price,
                }),
            ],
            TotalPrice = await _transactionRepository.GetOrderTotalPriceAsync(cart),
        };
    }
}
