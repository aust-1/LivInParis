using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Aspose.Cells;

namespace LivinParis.Tests;

[TestClass]
public class GraphOptimisation
{
    private const string dataDirectory = "data/";

    private static SortedDictionary<
        Node<Station>,
        SortedDictionary<Node<Station>, double>
    > XlsxToAdjacencyList(string fileName)
    {
        CultureInfo culture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        var adjacencyList =
            new SortedDictionary<Node<Station>, SortedDictionary<Node<Station>, double>>();

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
            var commune = stations[i, 5].StringValue;
            var insee = stations[i, 6].IntValue;

            var station = new Station(lineName, stationName, longitude, latitude, commune, insee);
            var node = new Node<Station>(
                stationId,
                station,
                new VisualizationParameters(longitude, latitude, station.ColorLine, stationName)
            );
            adjacencyList[node] = new SortedDictionary<Node<Station>, double>();
        }

        for (int i = 1; i <= lines.MaxDataRow; i++)
        {
            var stationId = lines[i, 0].IntValue;
            var station = Node<Station>.GetNode(stationId);

            try
            {
                var preStationId = lines[i, 3].IntValue;
                var preStation = Node<Station>.GetNode(preStationId);
                adjacencyList[station].Add(preStation, station.Data.GetTimeTo(preStation.Data));
            }
            catch (Exception) { }

            try
            {
                var nextStationId = lines[i, 4].IntValue;
                var nextStation = Node<Station>.GetNode(nextStationId);
                adjacencyList[station].Add(nextStation, station.Data.GetTimeTo(nextStation.Data));
            }
            catch (Exception) { }
        }

        for (int i = 1; i <= correspondences.MaxDataRow; i++)
        {
            var stationId = correspondences[i, 1].IntValue;
            var correspondenceId = correspondences[i, 2].IntValue;
            var correspondenceTime = correspondences[i, 3].DoubleValue;

            var node = Node<Station>.GetNode(stationId);
            var correspondence = Node<Station>.GetNode(correspondenceId);

            adjacencyList[node].Add(correspondence, correspondenceTime);
            adjacencyList[correspondence].Add(node, correspondenceTime);
        }

        return adjacencyList;
    }

    private static double[,] XlsxToAdjacencyMatrix(string fileName)
    {
        CultureInfo culture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

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
            var commune = stations[i, 5].StringValue;
            var insee = stations[i, 6].IntValue;

            var station = new Station(lineName, stationName, longitude, latitude, commune, insee);
            new Node<Station>(
                stationId,
                station,
                new VisualizationParameters(longitude, latitude, station.ColorLine, stationName)
            );
        }

        var adjacencyMatrix = new double[Node<Station>.Count, Node<Station>.Count];

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
            catch (Exception) { }

            try
            {
                var nextStationId = lines[i, 4].IntValue;
                var nextStation = Node<Station>.GetNode(nextStationId).Data;
                adjacencyMatrix[stationId, nextStationId] = station.GetTimeTo(nextStation);
            }
            catch (Exception) { }
        }

        for (int i = 1; i <= correspondences.MaxDataRow; i++)
        {
            var stationId1 = correspondences[i, 1].IntValue;
            var stationId2 = correspondences[i, 2].IntValue;
            var correspondenceTime = correspondences[i, 3].DoubleValue;

            adjacencyMatrix[stationId1, stationId2] = correspondenceTime;
            adjacencyMatrix[stationId2, stationId1] = correspondenceTime;
        }

        return adjacencyMatrix;
    }

    [TestInitialize]
    [TestCleanup]
    public void Clean()
    {
        Node<Station>.Clean();
    }

    #region Temps en fonction de l'initialisation

    [TestMethod]
    public void InitTimeByAdjacencyList()
    {
        long total = 0;
        for (int i = 0; i < 20; i++)
        {
            Node<Station>.Clean();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var adjacencyList = XlsxToAdjacencyList("MetroParis");
            new Graph<Station>(adjacencyList);
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;
            Debug.WriteLine(elapsedMilliseconds + " ms");
            total += elapsedMilliseconds;
        }
        Debug.WriteLine("Moyenne: " + total / 20 + " ms");
    }

    [TestMethod]
    public void InitTimeByAdjacencyMatrix()
    {
        long total = 0;
        for (int i = 0; i < 20; i++)
        {
            Node<Station>.Clean();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var adjacencyMatrix = XlsxToAdjacencyMatrix("MetroParis");
            new Graph<Station>(adjacencyMatrix);
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;
            Debug.WriteLine(elapsedMilliseconds + " ms");
            total += elapsedMilliseconds;
        }
        Debug.WriteLine("Moyenne: " + total / 20 + " ms");
    }

    #endregion Temps en fonction des méthodes de gestion de données

    #region Temps en fonction des méthodes de pathfinding

    [TestMethod]
    [DataRow(0, 330)]
    public void PathfindingTimeByDijkstra(int startMin, int startMax)
    {
        long total = 0;
        Graph<Station> graph = new Graph<Station>(XlsxToAdjacencyMatrix("MetroParis"));
        for (int start = startMin; start <= startMax; start++)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = graph.Dijkstra(start);
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;
            total += elapsedMilliseconds;
        }
        Debug.WriteLine("Moyenne: " + total / (startMax - startMin + 1) + " ms");
        Debug.WriteLine("Total: " + total + " ms");
    }

    [TestMethod]
    [DataRow(0, 330)]
    public void PathfindingTimeByBellmanFord(int startMin, int startMax)
    {
        long total = 0;
        Graph<Station> graph = new Graph<Station>(XlsxToAdjacencyMatrix("MetroParis"));
        for (int start = startMin; start <= startMax; start++)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = graph.BellmanFord(start);
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;
            total += elapsedMilliseconds;
        }
        Debug.WriteLine("Moyenne: " + total / (startMax - startMin + 1) + " ms");
        Debug.WriteLine("Total: " + total + " ms");
    }

    [TestMethod]
    [DataRow(30)]
    public void PathfindingTimeByRoyFloydWarshall(int iterations)
    {
        long total = 0;
        Graph<Station> graph = new Graph<Station>(XlsxToAdjacencyMatrix("MetroParis"));
        for (int i = 0; i < iterations; i++)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = graph.RoyFloydWarshall();
            sw.Stop();
            long elapsedMilliseconds = sw.ElapsedMilliseconds;
            total += elapsedMilliseconds;
            Debug.WriteLine(elapsedMilliseconds + " ms");
        }
        Debug.WriteLine("Moyenne: " + total / iterations + " ms");
    }

    #endregion Temps en fonction des méthodes de pathfinding
}
