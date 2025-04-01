using System.Globalization;
using Aspose.Cells;

namespace LivinParis
{
    public static class Program
    {
        private const string dataDirectory = "data/";

        static void Main(string[] args)
        {
            // string fileName = "soc-karate";

            // Graph<int> graph = new Graph<int>(MtxToAdjacencyMatrix(fileName));
            // //Graph graph = new Graph(TxtToAdjacencyList(fileName)); //Quel était le bon fichier ?

            // Console.WriteLine("=== Informations sur le Graphe ===");
            // Console.WriteLine($"Nombre de nœuds : {graph.Order}");
            // Console.WriteLine($"Nombre d'arêtes : {graph.Size}");
            // Console.WriteLine($"Le graphe est-il orienté ? {graph.IsDirected}");
            // Console.WriteLine($"Le graphe est-il pondéré ? {graph.IsWeighted}");
            // Console.WriteLine($"Densité du graphe : {graph.Density:F3}");

            // Console.WriteLine("\n=== Liste des Nœuds et leurs connexions ===");
            // foreach (var node in graph.Nodes)
            // {
            //     Console.Write($"{node.Data} -> ");
            //     if (graph.AdjacencyList.ContainsKey(node))
            //     {
            //         Console.WriteLine(
            //             string.Join(", ", graph.AdjacencyList[node].Select(n => n.Data))
            //         );
            //     }
            //     else
            //     {
            //         Console.WriteLine("Aucune connexion.");
            //     }
            // }

            // // Vérification de la connectivité
            // Console.WriteLine("\n=== Vérification de la Connectivité ===");
            // bool isConnected = graph.IsConnected;
            // Console.WriteLine($"Le graphe est-il connexe ? {isConnected}");

            // // Parcours en Largeur d'abord (BFS)
            // Console.WriteLine("\n=== BFS depuis le nœud 1 ===");
            // var bfs1 = graph.BFS(1);
            // Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs1));

            // // Parcours en Profondeur d'abord (DFS)
            // Console.WriteLine("\n=== DFS depuis le nœud 1 ===");
            // var dfs1 = graph.DFS(1);
            // Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs1));

            // // Détection des cycles
            // Console.WriteLine("\n=== Détection de Cycles ===");
            // string? cycle = graph.FindAnyCycle(false);
            // string? cycleSimple = graph.FindAnyCycle(true);

            // Console.WriteLine("Cycle simple détecté : " + (cycleSimple ?? "Aucun"));
            // Console.WriteLine("Cycle détecté (sans restrictions) : " + (cycle ?? "Aucun"));

            // // Dessin du graphe
            // graph.DisplayGraph("dot_graph");
            // graph.DisplayGraph("circo_graph", "circo");

            // Console.WriteLine("\nGraphiques générés et sauvegardés dans le dossier data/output.");
            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Graph<Station> graph = new Graph<Station>(XlsxToAdjacencyMatrix("metro/MetroParis"));
        }

        private static SortedDictionary<
            Node<Station>,
            SortedDictionary<Node<Station>, double>
        > XlsxToAdjacencyList(string fileName)
        {
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

                var station = new Station(
                    lineName,
                    stationName,
                    longitude,
                    latitude,
                    commune,
                    insee
                );
                var node = new Node<Station>(
                    stationId,
                    station,
                    longitude,
                    latitude,
                    station.ColorLine,
                    stationName
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
                    adjacencyList[station]
                        .Add(nextStation, station.Data.GetTimeTo(nextStation.Data));
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

                var station = new Station(
                    lineName,
                    stationName,
                    longitude,
                    latitude,
                    commune,
                    insee
                );
                new Node<Station>(
                    stationId,
                    station,
                    longitude,
                    latitude,
                    station.ColorLine,
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
    }
}
