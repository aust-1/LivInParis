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
    public async Task<IEnumerable<TransactionDto>> GetTransactionsByCustomerAsync(int customerId)
    {
        var transactions = await _transactionRepository.ReadAsync(customerId);
        return await Task.WhenAll(
            transactions.Select(async transaction =>
            {
                var orderLines = await _orderLineRepository.ReadAsync(transaction);
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
                    Id = transaction.TransactionId,
                    CustomerId = transaction.CustomerAccountId,
                    TotalPrice = await _transactionRepository.GetOrderTotalPriceAsync(transaction),
                    Lines = lines,
                };
            })
        );
    }


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
