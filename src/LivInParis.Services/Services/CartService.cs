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
        var items = (
            await _orderLineRepository.ReadAsync(customerId, status: OrderLineStatus.InCart)
        ).Select(async ol => new OrderLineDto
        {
            DishId = (await _orderLineRepository.GetOrderDishAsync(ol)).DishId,
            DishName = (await _orderLineRepository.GetOrderDishAsync(ol)).DishName,
            Status = ol.OrderLineStatus.ToString(),
            UnitPrice = (await _orderLineRepository.GetOrderDishAsync(ol)).Price,
        });

        return new CartDto { Items = (IList<CartItemDto>)items };
    }

    /// <inheritdoc/>
    public async Task AddItemAsync(int customerId, int chefId)
    {
        var currentTransaction =
            await _transactionRepository.GetCurrentTransactionAsync(customerId)
            ?? new OrderTransaction { CustomerAccountId = customerId, TransactionDatetime = null };

        var individualAddress = await _individualRepository.GetByIdAsync(customerId);

        var orderLine = new OrderLine
        {
            TransactionId = currentTransaction.TransactionId,
            ChefAccountId = chefId,
            OrderLineStatus = OrderLineStatus.InCart,
            OrderLineDatetime = DateTime.Now,
            AddressId = individualAddress!.AddressId,
        };

        await _orderLineRepository.AddAsync(orderLine);
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
    }
}
//FIXME: la cata
