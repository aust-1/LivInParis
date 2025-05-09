using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="TransactionService"/>.
/// </summary>
public class TransactionService(
    IOrderLineRepository orderLineRepository,
    IOrderTransactionRepository transactionRepository
) : ITransactionService
{
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;
    private readonly IOrderTransactionRepository _transactionRepository = transactionRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<TransactionDto>> GetTransactionsByCustomerAsync(int customerId) =>
        (IEnumerable<TransactionDto>)
            (await _transactionRepository.ReadAsync(customerId)).Select(
                async ot => new TransactionDto
                {
                    Id = ot.TransactionId,
                    CustomerId = ot.CustomerAccountId,
                    TotalPrice = await _transactionRepository.GetOrderTotalPriceAsync(ot),
                    Lines =
                        (IEnumerable<OrderLineDto>)
                            (await _orderLineRepository.ReadAsync(ot)).Select(
                                async ol => new OrderLineDto
                                {
                                    DishId = (
                                        await _orderLineRepository.GetOrderDishAsync(ol)
                                    ).DishId,
                                    DishName = (
                                        await _orderLineRepository.GetOrderDishAsync(ol)
                                    ).DishName,
                                    Status = ol.OrderLineStatus.ToString(),
                                    UnitPrice = (
                                        await _orderLineRepository.GetOrderDishAsync(ol)
                                    ).Price,
                                }
                            ),
                }
            );

    /// <inheritdoc/>
    public async Task<TransactionDto> GetTransactionByIdAsync(int transactionId)
    {
        var transaction =
            await _transactionRepository.GetByIdAsync(transactionId)
            ?? throw new ArgumentException("Transaction not found");
        var totalPrice = await _transactionRepository.GetOrderTotalPriceAsync(transaction);
        var orderLines = await _orderLineRepository.ReadAsync(transaction);
        var lines = await Task.WhenAll(
            orderLines.Select(async ol =>
            {
                var dish = await _orderLineRepository.GetOrderDishAsync(ol);
                return new OrderLineDto
                {
                    DishId = dish.DishId,
                    DishName = dish.DishName,
                    Status = ol.OrderLineStatus.ToString(),
                    UnitPrice = dish.Price,
                };
            })
        );

        return new TransactionDto
        {
            Id = transaction.TransactionId,
            CustomerId = transaction.CustomerAccountId,
            TotalPrice = totalPrice,
            Lines = lines,
        };
    }
}
