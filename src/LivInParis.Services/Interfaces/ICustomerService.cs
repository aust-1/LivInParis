using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<OrderTransaction>> GetOrdersForCustomerAsync(int accountId);
}
