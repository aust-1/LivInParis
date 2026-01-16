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
    public async Task<IEnumerable<OrderLineDto>> GetIncomingOrdersAsync(int chefId)
    {
        var orders = await _orderLineRepository.ReadAsync(
            chefId: chefId,
            status: OrderLineStatus.Pending
        );

        return await Task.WhenAll(
            orders.Select(async orderLine =>
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
    }

    /// <inheritdoc/>
    public async Task AcceptOrderAsync(int orderId)
    {
        var orderLine =
            await _orderLineRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order line not found", nameof(orderId));
        orderLine.OrderLineStatus = OrderLineStatus.Preparing;
        _orderLineRepository.Update(orderLine);
        await _orderLineRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RejectOrderAsync(int orderId)
    {
        var orderLine =
            await _orderLineRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order line not found", nameof(orderId));
        orderLine.OrderLineStatus = OrderLineStatus.Canceled;
        _orderLineRepository.Update(orderLine);
        await _orderLineRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateOrderStatusAsync(int orderId, OrderStatusDto statusDto)
    {
        if (string.IsNullOrWhiteSpace(statusDto.Status))
        {
            throw new ArgumentException("Status is required", nameof(statusDto));
        }

        if (!Enum.TryParse<OrderLineStatus>(statusDto.Status, true, out var status))
        {
            throw new ArgumentException("Invalid status", nameof(statusDto));
        }

        var orderLine =
            await _orderLineRepository.GetByIdAsync(orderId)
            ?? throw new ArgumentException("Order line not found", nameof(orderId));
        orderLine.OrderLineStatus = status;
        _orderLineRepository.Update(orderLine);
        await _orderLineRepository.SaveChangesAsync();
    }

}
