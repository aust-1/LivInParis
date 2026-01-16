using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="CartService"/>.
/// </summary>
public class CartService(
    IOrderLineRepository orderLineRepository,
    IOrderTransactionRepository transactionRepository,
    IIndividualRepository individualRepository
) : ICartService
{
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;
    private readonly IOrderTransactionRepository _transactionRepository = transactionRepository;
    private readonly IIndividualRepository _individualRepository = individualRepository;

    /// <inheritdoc/>
    public async Task<CartDto> GetCartAsync(int customerId)
    {
        var orderLines = await _orderLineRepository.ReadAsync(
            customerId: customerId,
            status: OrderLineStatus.InCart
        );

        var items = await Task.WhenAll(
            orderLines.Select(async orderLine =>
            {
                var dish = await _orderLineRepository.GetOrderDishAsync(orderLine);
                return new CartItemDto
                {
                    DishId = dish.DishId,
                    DishName = dish.DishName,
                    UnitPrice = dish.Price,
                };
            })
        );

        return new CartDto { CustomerId = customerId, Items = items.ToList() };
    }

    /// <inheritdoc/>
    public async Task AddItemAsync(int customerId, int chefId)
    {
        var currentTransaction = await _transactionRepository.GetCurrentTransactionAsync(
            customerId
        );
        if (currentTransaction == null)
        {
            currentTransaction = new OrderTransaction
            {
                CustomerAccountId = customerId,
                TransactionDatetime = null,
            };
            await _transactionRepository.AddAsync(currentTransaction);
            await _transactionRepository.SaveChangesAsync();
        }

        var individualAddress = await _individualRepository.GetByIdAsync(customerId);
        if (individualAddress == null)
        {
            throw new ArgumentException("Customer address not found", nameof(customerId));
        }

        var orderLine = new OrderLine
        {
            TransactionId = currentTransaction.TransactionId,
            ChefAccountId = chefId,
            OrderLineStatus = OrderLineStatus.InCart,
            OrderLineDatetime = DateTime.UtcNow,
            AddressId = individualAddress.AddressId,
        };

        await _orderLineRepository.AddAsync(orderLine);
        await _orderLineRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveItemAsync(int customerId, int chefId)
    {
        var orderline = await _orderLineRepository.ReadAsync(
            customerId: customerId,
            chefId: chefId,
            status: OrderLineStatus.InCart
        );

        foreach (var ol in orderline)
        {
            _orderLineRepository.Delete(ol);
        }

        await _orderLineRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(int customerId)
    {
        var orderline = await _orderLineRepository.ReadAsync(
            customerId: customerId,
            status: OrderLineStatus.InCart
        );
        foreach (var ol in orderline)
        {
            _orderLineRepository.Delete(ol);
        }

        await _orderLineRepository.SaveChangesAsync();
    }
}


