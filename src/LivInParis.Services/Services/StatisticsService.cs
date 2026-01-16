using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="StatisticsService"/>.
/// </summary>
public class StatisticsService(
    IChefRepository chefRepository,
    ICustomerRepository customerRepository,
    IOrderLineRepository orderLineRepository
) : IStatisticsService
{
    private readonly IChefRepository _chefRepository = chefRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<ChefDeliveryStatsDto>> GetChefDeliveryStatsAsync()
    {
        var items = await _chefRepository.GetDeliveryCountByChefAsync();
        return items.Select(entry => new ChefDeliveryStatsDto
        {
            ChefId = entry.Chef.ChefAccountId,
            ChefName = entry.Chef.Account?.AccountUserName,
            DeliveryCount = entry.OrderCount,
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RevenueByStreetDto>> GetRevenueByStreetAsync()
    {
        var delivered = await _orderLineRepository.ReadAsync(status: OrderLineStatus.Delivered);
        var items = await Task.WhenAll(
            delivered.Select(async orderLine =>
            {
                var dish = await _orderLineRepository.GetOrderDishAsync(orderLine);
                return new { orderLine.AddressId, dish.Price };
            })
        );

        var revenueByAddress = items
            .GroupBy(item => item.AddressId)
            .Select(group => new { AddressId = group.Key, Revenue = group.Sum(x => x.Price) })
            .ToDictionary(row => row.AddressId, row => row.Revenue);

        var deliveredWithAddress = await _orderLineRepository.ReadAsync(
            status: OrderLineStatus.Delivered
        );

        return deliveredWithAddress
            .GroupBy(orderLine => orderLine.AddressId)
            .Select(group => new RevenueByStreetDto
            {
                Street = group.FirstOrDefault()?.Address?.Street ?? "Unknown",
                Revenue = revenueByAddress.GetValueOrDefault(group.Key, 0m),
            });
    }

    /// <inheritdoc/>
    public Task<decimal> GetAverageOrderPriceAsync() =>
        _orderLineRepository.GetAverageOrderPriceAsync(status: OrderLineStatus.Delivered);

    /// <inheritdoc/>
    public async Task<IEnumerable<CuisinePreferenceDto>> GetTopCuisinesAsync(
        DateTime? from = null,
        DateTime? to = null
    )
    {
        var customers = await _customerRepository.ReadAsync();
        var items = await Task.WhenAll(
            customers.Select(async customer =>
            {
                var prefs = await _customerRepository.GetCustomerCuisinePreferencesAsync(
                    customer,
                    from,
                    to
                );
                return prefs.Select(pref => new CuisinePreferenceDto
                {
                    Cuisine = pref.CuisineNationality,
                    OrderCount = pref.OrderCount,
                });
            })
        );

        return items.SelectMany(x => x)
            .GroupBy(x => x.Cuisine)
            .Select(group => new CuisinePreferenceDto
            {
                Cuisine = group.Key,
                OrderCount = group.Sum(x => x.OrderCount),
            })
            .OrderByDescending(x => x.OrderCount)
            .Take(10);
    }
}

