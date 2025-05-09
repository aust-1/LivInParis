using LivInParisRoussilleTeynier.Domain.Models.Maps;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <summary>
/// Provides graph‐based routing between stations using Dijkstra’s algorithm.
/// </summary>
public class GraphService : IGraphService
{
    /// <inheritdoc/>
    public async Task<RouteDto> GetShortestPathAsync(string fromAddress, string toAddress)
    {
        var start = await Metro.GetNearestStation(fromAddress);
        var end = await Metro.GetNearestStation(toAddress);
        var (nodes, time) = Metro.Graph.GetPath(start, end);

        var segments = new List<StationDto>();
        foreach (var station in nodes.Select(n => n.Data))
        {
            var stationDto = new StationDto
            {
                Name = station.ToString(),
                LongitudeRadians = station.LongitudeRadians,
                LatitudeRadians = station.LatitudeRadians,
            };
            segments.Add(stationDto);
        }

        return new RouteDto { Stations = segments, TotalTime = time };
    }
}
