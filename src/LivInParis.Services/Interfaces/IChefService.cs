namespace LivInParisRoussilleTeynier.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LivInParisRoussilleTeynier.Domain.Models.Order;

public interface IChefService
{
    Task<IEnumerable<OrderLine>> GetIncomingOrdersAsync(int chefId);
    Task AcceptOrderAsync(int orderLineId);
    Task RejectOrderAsync(int orderLineId);
    Task<IEnumerable<OrderLine>> GetDeliveriesAsync(int chefId);
    Task<OrderLine?> GetDeliveryDetailAsync(int deliveryId);
}
