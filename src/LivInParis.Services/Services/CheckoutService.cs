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
        cart.TransactionDatetime = DateTime.UtcNow;
        _transactionRepository.Update(cart);

        var orderLines = await _orderLineRepository.ReadAsync(cart);
        foreach (var orderLine in orderLines)
        {
            orderLine.OrderLineStatus = OrderLineStatus.Pending;
            orderLine.AddressId = checkoutDto.AddressId;
            orderLine.OrderLineDatetime = DateTime.UtcNow;
            _orderLineRepository.Update(orderLine);
        }

        await _transactionRepository.SaveChangesAsync();
        await _orderLineRepository.SaveChangesAsync();

        var lines = await Task.WhenAll(
            orderLines.Select(async orderLine =>
            {
                var dish = await _orderLineRepository.GetOrderDishAsync(orderLine);
                return new OrderLineDto
                {
                    DishId = dish.DishId,
                    DishName = dish.DishName,
                    Status = orderLine.OrderLineStatus.ToString(),
                    UnitPrice = dish.Price,
                };
            })
        );

        return new TransactionDto
        {
            Id = cart.TransactionId,
            CustomerId = customerId,
            Lines = lines,
            TotalPrice = await _transactionRepository.GetOrderTotalPriceAsync(cart),
        };

    }
}
