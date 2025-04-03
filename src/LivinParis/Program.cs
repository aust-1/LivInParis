using System.Threading.Tasks;

namespace LivinParisRoussilleTeynier
{
    public static class Program
    {
        private const string dataDirectory = "data/";

        static async Task Main(string[] args)
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


            // var metro = new Metro("metro/MetroParis");

            // var st = await metro.GetNearestStation("68 avenue des Champs Elysées");

            // Console.WriteLine($"La station la plus proche est : {st}");

            // var djresult = metro.Graph.GetPartialGraphByDijkstra(st);

            // djresult.DisplayGraph("dijkstraresult", "dot", fontsize: 9);
            // metro.Graph.DisplayGraph();

            // var generator = new ProxyGenerator();
            // var interceptor = new ConnectionInterceptor();

            // // Crée un proxy autour de MonService
            // var service = generator.CreateClassProxy<AccountRepository>(interceptor);

            // // Appels
            // service.GetAccounts(9);
        }
    }
}
