using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class ChefService(IOrderLineRepository orderLineRepo) : IChefService
{
    private readonly IOrderLineRepository _orderLineRepo = orderLineRepo;

    public Task<IEnumerable<OrderLine>> GetIncomingOrdersAsync(int chefId)
    {
        var chef = new Chef { AccountId = chefId };
        return _orderLineRepo.ReadAsync(chef: chef, status: OrderLineStatus.Pending);
    }

    public async Task AcceptOrderAsync(int orderLineId)
    {
        var ol = await _orderLineRepo.GetByIdAsync(orderLineId);
        if (ol != null)
        {
            ol.OrderLineStatus = OrderLineStatus.Prepared;
            _orderLineRepo.Update(ol);
            await _orderLineRepo.SaveChangesAsync();
        }
    }

    public async Task RejectOrderAsync(int orderLineId)
    {
        var ol = await _orderLineRepo.GetByIdAsync(orderLineId);
        if (ol != null)
        {
            ol.OrderLineStatus = OrderLineStatus.Canceled;
            _orderLineRepo.Update(ol);
            await _orderLineRepo.SaveChangesAsync();
        }
    }

    public Task<IEnumerable<OrderLine>> GetDeliveriesAsync(int chefId)
    {
        var chef = new Chef { AccountId = chefId };
        return _orderLineRepo.ReadAsync(chef: chef, status: OrderLineStatus.Delivered);
    }

    public Task<OrderLine?> GetDeliveryDetailAsync(int deliveryId)
    {
        return _orderLineRepo.GetByIdAsync(deliveryId);
    }
}
