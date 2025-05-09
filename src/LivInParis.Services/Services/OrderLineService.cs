using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="OrderLineService"/>.
/// </summary>
public class OrderLineService(IOrderLineRepository orderLineRepository) : IOrderLineService
{
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;

    /// <inheritdoc/>
    public async Task<OrderLineDto> GetOrderLineByIdAsync(int orderLineId)
    {
        var orderLine =
            await _orderLineRepository.GetByIdAsync(orderLineId)
            ?? throw new ArgumentException("Order line not found");
        var dish = await _orderLineRepository.GetOrderDishAsync(orderLine);
        return new OrderLineDto
        {
            DishId = dish.DishId,
            DishName = dish.DishName,
            Status = orderLine.OrderLineStatus.ToString(),
            UnitPrice = dish.Price,
        };
    }

    /// <inheritdoc/>
    public async Task CancelOrderLineAsync(int orderLineId)
    {
        var orderLine =
            await _orderLineRepository.GetByIdAsync(orderLineId)
            ?? throw new ArgumentException("Order line not found");
        orderLine.OrderLineStatus = OrderLineStatus.Canceled;
        _orderLineRepository.Update(orderLine);
    }
}
