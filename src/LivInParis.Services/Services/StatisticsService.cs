// using LivInParisRoussilleTeynier.Domain.Models.Order;
// using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
// using LivInParisRoussilleTeynier.Services.Interfaces;

// namespace LivInParisRoussilleTeynier.Services.Services;

// /// <inheritdoc/>
// /// <summary>
// /// Initializes a new instance of <see cref="StatisticsService"/>.
// /// </summary>
// public class StatisticsService(IStatisticsRepository statsRepository) : IStatisticsService
// {
//     private readonly IStatisticsRepository _statsRepository = statsRepository;

//     /// <inheritdoc/>
//     public Task<IEnumerable<ChefDeliveryStatsDto>> GetChefDeliveryStatsAsync() =>
//         _statsRepository.GetDeliveriesByChefAsync();

//     /// <inheritdoc/>
//     public Task<IEnumerable<RevenueByStreetDto>> GetRevenueByStreetAsync() =>
//         _statsRepository.GetRevenueByStreetAsync();

//     /// <inheritdoc/>
//     public Task<decimal> GetAverageOrderPriceAsync() =>
//         _statsRepository.GetAverageOrderPriceAsync();

//     /// <inheritdoc/>
//     public Task<IEnumerable<CuisinePreferenceDto>> GetTopCuisinesAsync(
//         DateTime? from = null,
//         DateTime? to = null
//     ) => _statsRepository.GetTopCuisinesAsync(from, to);
// }

//TODO: add plein de stats
