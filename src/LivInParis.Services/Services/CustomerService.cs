using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _custRepo;
    private readonly IOrderTransactionRepository _transRepo;

    public CustomerService(ICustomerRepository custRepo, IOrderTransactionRepository transRepo)
    {
        _custRepo = custRepo;
        _transRepo = transRepo;
    }

    public Task<IEnumerable<Customer>> GetAllAsync() => _custRepo.GetAllAsync();

    public async Task<IEnumerable<OrderTransaction>> GetOrdersForCustomerAsync(int accountId)
    {
        var customer = await _custRepo.GetByIdAsync(accountId);
        return await _transRepo.ReadAsync(customer: customer);
    }
}
