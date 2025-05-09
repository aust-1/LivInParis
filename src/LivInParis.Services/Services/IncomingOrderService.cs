using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="IncomingOrderService"/>.
/// </summary>
public class IncomingOrderService(IOrderLineRepository orderLineRepository) : IIncomingOrderService
{
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderLineDto>> GetIncomingOrdersAsync(int chefId) =>
        (IEnumerable<OrderLineDto>)
            (
                await _orderLineRepository.ReadAsync(
                    chefId: chefId,
                    status: OrderLineStatus.Pending
                )
            ).Select(async ol => new OrderLineDto
            {
                DishId = (await _orderLineRepository.GetOrderDishAsync(ol)).DishId,
                DishName = (await _orderLineRepository.GetOrderDishAsync(ol)).DishName,
                Status = ol.OrderLineStatus.ToString(),
                UnitPrice = (await _orderLineRepository.GetOrderDishAsync(ol)).Price,
            });

    /// <inheritdoc/>
    public Task AcceptOrderAsync(int orderId)
    {
        var orderLine = _orderLineRepository.GetByIdAsync(orderId).Result;
        orderLine!.OrderLineStatus = OrderLineStatus.Preparing;
        _orderLineRepository.Update(orderLine);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task RejectOrderAsync(int orderId)
    {
        var orderLine = _orderLineRepository.GetByIdAsync(orderId).Result;
        orderLine!.OrderLineStatus = OrderLineStatus.Canceled;
        _orderLineRepository.Update(orderLine);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task UpdateOrderStatusAsync(int orderId, OrderStatusDto statusDto)
    {
        var orderLine = _orderLineRepository.GetByIdAsync(orderId).Result;
        orderLine!.OrderLineStatus = Enum.Parse<OrderLineStatus>(statusDto.Status!);
        _orderLineRepository.Update(orderLine);
        return Task.CompletedTask;
    }
}
