using Aspose.Cells;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LivInParisRoussilleTeynier
{
    public static class Program
    {
        private const string dataDirectory = "../resources/";

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

            // var metro = new Metro("MetroParis");

            // var st = await metro.GetNearestStation("36 avenue Foch");

            // Console.WriteLine($"La station la plus proche est : {st}");

            // var djresult = metro.Graph.GetPartialGraphByDijkstra(st);

            // djresult.DisplayGraph("dijkstraresult", "dot", fontsize: 9);
            // metro.Graph.DisplayGraph();

            // metro.Graph.ComputeWelshPowell();
            // metro.Graph.DisplayGraph("welshpowell", "fdp", penwidth: 15f, fontsize: 9);

            // Window.Open();
            // new MainMenuPage().Display();
            // Window.Close();

            // 1. Construire le host console
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    (hostCtx, services) =>
                    {
                        services.AddDbContext<LivInParisContext>();
                    }
                )
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await DatabaseSeeder.SeedFromExcelAsync(dataDirectory, services);
            }

            Console.WriteLine("Seed terminé. Appuyez sur une touche pour quitter.");
            Console.ReadKey();
        }

        private static void LoadPeuplement()
        {
            var file = dataDirectory + "Peuplement" + ".xlsx";
            var wb = new Workbook(file);
            var addresses = wb.Worksheets[0].Cells;
            var accounts = wb.Worksheets[1].Cells;
            var chefs = wb.Worksheets[2].Cells;
            var companies = wb.Worksheets[3].Cells;
            var contains = wb.Worksheets[4].Cells;
            var customers = wb.Worksheets[5].Cells;
            var dishes = wb.Worksheets[6].Cells;
            var individuals = wb.Worksheets[7].Cells;
            var ingredients = wb.Worksheets[8].Cells;
            var menuProposals = wb.Worksheets[9].Cells;
            var orderLines = wb.Worksheets[10].Cells;
            var orderTransactions = wb.Worksheets[11].Cells;
            var reviews = wb.Worksheets[12].Cells;

            for (int i = 1; i <= addresses.MaxDataRow; i++)
            {
                var addressNumber = addresses[i, 0].IntValue;
                var street = addresses[i, 1].StringValue;

                var nearestStation = Metro.GetNearestStation($"{addressNumber} {street}").Result;

                new Address
                {
                    AddressNumber = addressNumber,
                    Street = street,
                    NearestStation = nearestStation,
                };
            }

            for (int i = 1; i <= accounts.MaxDataRow; i++)
            {
                var email = accounts[i, 0].StringValue;
                var password = accounts[i, 1].StringValue;

                new Account { AccountEmail = email, AccountPassword = password };
            }

            for (int i = 1; i <= chefs.MaxDataRow; i++)
            {
                var accountId = chefs[i, 0].IntValue;
                var chefRating = Convert.ToDecimal(chefs[i, 1].DoubleValue);
                var ChefIsBanned = chefs[i, 2].BoolValue;
                var addressId = chefs[i, 3].IntValue;

                new Chef
                {
                    AccountId = accountId,
                    ChefRating = chefRating,
                    ChefIsBanned = ChefIsBanned,
                    AddressId = addressId,
                };
            }

            for (int i = 1; i <= customers.MaxDataRow; i++)
            {
                var accountId = customers[i, 0].IntValue;
                var customerRating = Convert.ToDecimal(customers[i, 1].DoubleValue);
                var customerIsBanned = customers[i, 2].BoolValue;

                new Customer
                {
                    AccountId = accountId,
                    CustomerRating = customerRating,
                    CustomerIsBanned = customerIsBanned,
                };
            }
        }
    }
}
