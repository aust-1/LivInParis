using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class StatisticsService(
    ICustomerRepository customerRepo,
    IOrderTransactionRepository transactionRepo
) : IStatisticsService
{
    private readonly ICustomerRepository _customerRepo = customerRepo;
    private readonly IOrderTransactionRepository _transactionRepo = transactionRepo;

    public async Task<IEnumerable<Customer>> GetCustomersAsync(
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? isBanned = null
    ) =>
        await _customerRepo.ReadAsync(c =>
            (!minRating.HasValue || c.CustomerRating >= minRating)
            && (!maxRating.HasValue || c.CustomerRating <= maxRating)
            && (!isBanned.HasValue || c.CustomerIsBanned == isBanned)
        );

    public async Task<
        IEnumerable<(Customer Customer, int OrderCount)>
    > GetCustomersByOrderCountAsync(DateTime? from = null, DateTime? to = null)
    {
        // Simplest: group transaction by customer
        var list = new List<(Customer, int)>();
        foreach (var cust in await _customerRepo.GetAllAsync())
        {
            var count = await _transactionRepo
                .ReadAsync(new Customer { AccountId = cust.AccountId })
                .ContinueWith(t => t.Result.Count());
            list.Add((cust, count));
        }
        return list;
    }

    public Task<IEnumerable<(Customer Customer, decimal TotalSpent)>> GetCustomersBySpendingAsync(
        DateTime? from = null,
        DateTime? to = null
    ) => Task.FromResult<IEnumerable<(Customer, decimal)>>(new List<(Customer, decimal)>());

    public Task<
        IEnumerable<(string CuisineNationality, int OrderCount)>
    > GetCustomerCuisinePreferencesAsync(
        Customer customer,
        DateTime? from = null,
        DateTime? to = null
    ) => Task.FromResult<IEnumerable<(string, int)>>(new List<(string, int)>());

    public Task<IEnumerable<(Chef Chef, int OrderCount)>> GetChefDeliveryCountAsync(
        DateTime? from = null,
        DateTime? to = null
    ) => Task.FromResult<IEnumerable<(Chef, int)>>(new List<(Chef, int)>());

    public Task<IEnumerable<(Chef Chef, decimal TotalValue)>> GetChefDeliveryValueAsync(
        DateTime? from = null,
        DateTime? to = null
    ) => Task.FromResult<IEnumerable<(Chef, decimal)>>(new List<(Chef, decimal)>());

    public Task<IEnumerable<(Dish Dish, int OrderCount)>> GetDeliveryCountPerDishByChefAsync(
        Chef chef,
        DateTime? from = null,
        DateTime? to = null
    ) => Task.FromResult<IEnumerable<(Dish, int)>>(new List<(Dish, int)>());
}
