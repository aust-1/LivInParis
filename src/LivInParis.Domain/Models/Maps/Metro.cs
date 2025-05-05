using System.Diagnostics;
using Aspose.Cells;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using Newtonsoft.Json.Linq;

namespace LivInParisRoussilleTeynier.Domain.Models.Maps;

/// <summary>
/// Provides functionality to initialize the metro network and to retrieve the nearest station based on coordinates.
/// </summary>
public static class Metro
{
    #region Fields

    /// <summary>
    /// The directory where the data files are located.
    /// </summary>
    private const string dataDirectory = "../resources/";

    /// <summary>
    /// The base URL for the Nominatim API used to retrieve coordinates from an address.
    /// </summary>
    private const string baseUrl = "https://nominatim.openstreetmap.org/search";

    /// <summary>
    /// The graph representing the metro network.
    /// </summary>
    private static Graph<Station>? _graph;

    #endregion Fields

    #region Initialization

    /// <summary>
    /// Initializes the metro network using data from an Excel file.
    /// The file should contain three sheets: stations, lines, and correspondences.
    /// </summary>
    /// <param name="fileName">The name of the Excel file (without extension).</param>
    public static void InitializeMetro(string fileName)
    {
        var file = dataDirectory + fileName + ".xlsx";
        var wb = new Workbook(file);
        var stations = wb.Worksheets[0].Cells;
        var lines = wb.Worksheets[1].Cells;
        var correspondences = wb.Worksheets[2].Cells;

        for (int i = 1; i <= stations.MaxDataRow; i++)
        {
            var stationId = stations[i, 0].IntValue;
            var lineName = stations[i, 1].StringValue;
            var stationName = stations[i, 2].StringValue;
            var longitude = stations[i, 3].DoubleValue;
            var latitude = stations[i, 4].DoubleValue;

            var station = new Station(lineName, stationName, longitude, latitude);
            new Node<Station>(
                stationId,
                station,
                longitude,
                latitude,
                station.LineColor,
                stationName
            );
        }

        var adjacencyMatrix = new double[Node<Station>.Count, Node<Station>.Count];

        for (int i = 0; i < Node<Station>.Count; i++)
        {
            for (int j = 0; j < Node<Station>.Count; j++)
            {
                adjacencyMatrix[i, j] = double.MaxValue;
            }
            adjacencyMatrix[i, i] = 0;
        }

        for (int i = 1; i <= lines.MaxDataRow; i++)
        {
            var stationId = lines[i, 0].IntValue;
            var station = Node<Station>.GetNode(stationId).Data;

            try
            {
                var preStationId = lines[i, 3].IntValue;
                var preStation = Node<Station>.GetNode(preStationId).Data;
                adjacencyMatrix[stationId, preStationId] = station.GetTimeTo(preStation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"Warning: Unable to set previous station for stationId {stationId} - {ex.Message}"
                );
            }

            try
            {
                var nextStationId = lines[i, 4].IntValue;
                var nextStation = Node<Station>.GetNode(nextStationId).Data;
                adjacencyMatrix[stationId, nextStationId] = station.GetTimeTo(nextStation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"Warning: Unable to set previous station for stationId {stationId} - {ex.Message}"
                );
            }
        }

        for (int i = 1; i <= correspondences.MaxDataRow; i++)
        {
            var stationId1 = correspondences[i, 1].IntValue;
            var stationId2 = correspondences[i, 2].IntValue;
            var correspondenceTime = correspondences[i, 3].DoubleValue;

            adjacencyMatrix[stationId1, stationId2] = correspondenceTime;
            adjacencyMatrix[stationId2, stationId1] = correspondenceTime;
        }

        _graph = new Graph<Station>(adjacencyMatrix);
    }

    #endregion Initialization

    #region Properties

    /// <summary>
    /// Gets the graph representing the metro network.
    /// </summary>
    public static Graph<Station> Graph
    {
        get { return _graph!; }
    }

    #endregion Properties

    /// <summary>
    /// Finds the nearest station to the given coordinates (longitude, latitude).
    /// </summary>
    /// <param name="address">The address of the target point.</param>
    /// <returns>The nearest station.</returns>
    public static async Task<Station> GetNearestStation(string address)
    {
        (double Lon, double Lat)? coordonnees = await GetCoordinatesFromAddress(address);

        return GetNearestStation(coordonnees.Value.Lon, coordonnees.Value.Lat);
    }

    /// <summary>
    /// Finds the nearest station to the given coordinates (longitude, latitude).
    /// </summary>
    /// <param name="address">The address of the target point.</param>
    /// <returns>The nearest station.</returns>
    public static async Task<Station> GetNearestStation(Address address)
    {
        return await GetNearestStation($"{address.AddressNumber} {address.Street}");
    }

    /// <summary>
    /// Finds the nearest station to the given coordinates (longitude, latitude).
    /// </summary>
    /// <param name="longitude">The longitude of the target point in degrees.</param>
    /// <param name="latitude">The latitude of the target point in degrees.</param>
    /// <returns>The nearest station.</returns>
    private static Station GetNearestStation(double longitude, double latitude)
    {
        var nearestStation = Node<Station>.GetNode(0).Data;
        var minDistance = double.MaxValue;

        foreach (var station in _graph!.Nodes.Select(n => n.Data))
        {
            var distance = station.GetDistanceTo(longitude, latitude);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestStation = station;
            }
        }

        return nearestStation;
    }

    /// <summary>
    /// Retrieves the coordinates (longitude, latitude) from the given address using the Nominatim API.
    /// </summary>
    /// <param name="address">The address to search for.</param>
    /// <returns>A tuple containing the longitude and latitude of the address, or null if not found.</returns>
    public static async Task<(double, double)?> GetCoordinatesFromAddress(string address)
    {
        address = address.Replace(" ", "+");
        address = address.Replace("'", "%27");
        address += "+Paris+France";

        string url = $"{baseUrl}?format=json&q={Uri.EscapeDataString(address)}";

        using var client = new HttpClient();
        try
        {
            client.DefaultRequestHeaders.Add("User-Agent", "LivInParis");

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            JArray results = JArray.Parse(content);

            if (results != null && results.Count > 0)
            {
                double lon = Convert.ToDouble(results[0]["lon"]);
                double lat = Convert.ToDouble(results[0]["lat"]);
                return (lon, lat);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during API call: {ex.Message}");
        }

        return null;
    }
}
