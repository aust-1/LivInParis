using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services.Interfaces;

public interface IOrderLineService
{
    Task PlaceOrderAsync(OrderRequest request);
    Task<OrderTransaction?> GetOrderDetailAsync(int orderId);
}
