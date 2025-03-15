namespace Karate
{
    public static class Program
    {
        private const string dataDirectory = "data/";

        static void Main(string[] args)
        {
            string fileName = "soc-karate";

            Graph<int> graph = new Graph<int>(MtxToAdjacencyMatrix(fileName));
            //Graph graph = new Graph(TxtToAdjacencyList(fileName)); //Quel était le bon fichier ?

            Console.WriteLine("=== Informations sur le Graphe ===");
            Console.WriteLine($"Nombre de nœuds : {graph.Order}");
            Console.WriteLine($"Nombre d'arêtes : {graph.Size}");
            Console.WriteLine($"Le graphe est-il orienté ? {graph.IsDirected}");
            Console.WriteLine($"Le graphe est-il pondéré ? {graph.IsWeighted}");
            Console.WriteLine($"Densité du graphe : {graph.Density:F3}");

            Console.WriteLine("\n=== Liste des Nœuds et leurs connexions ===");
            foreach (var node in graph.Nodes)
            {
                Console.Write($"{node.Data} -> ");
                if (graph.AdjacencyList.ContainsKey(node))
                {
                    Console.WriteLine(
                        string.Join(", ", graph.AdjacencyList[node].Select(n => n.Data))
                    );
                }
                else
                {
                    Console.WriteLine("Aucune connexion.");
                }
            }

            // Vérification de la connectivité
            Console.WriteLine("\n=== Vérification de la Connectivité ===");
            bool isConnected = graph.IsConnected;
            Console.WriteLine($"Le graphe est-il connexe ? {isConnected}");

            // Parcours en Largeur d'abord (BFS)
            Console.WriteLine("\n=== BFS depuis le nœud 1 ===");
            var bfs1 = graph.BFS(1);
            Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs1));

            // Parcours en Profondeur d'abord (DFS)
            Console.WriteLine("\n=== DFS depuis le nœud 1 ===");
            var dfs1 = graph.DFS(1);
            Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs1));

            // Détection des cycles
            Console.WriteLine("\n=== Détection de Cycles ===");
            string? cycle = graph.FindAnyCycle(false);
            string? cycleSimple = graph.FindAnyCycle(true);

            Console.WriteLine("Cycle simple détecté : " + (cycleSimple ?? "Aucun"));
            Console.WriteLine("Cycle détecté (sans restrictions) : " + (cycle ?? "Aucun"));

            // Dessin du graphe
            graph.DisplayGraph("dot_graph");
            graph.DisplayGraph("circo_graph", "circo");

            Console.WriteLine("\nGraphiques générés et sauvegardés dans le dossier data/output.");
        }

        private static double[,] MtxToAdjacencyMatrix(string fileName)
        {
            double[,] adjacencyMatrix = new double[0, 0];
            string file = dataDirectory + fileName + ".mtx";
            StreamReader? sReader = null;

            try
            {
                sReader = new StreamReader(file);
                string? headerLine;
                while ((headerLine = sReader.ReadLine()) != null)
                {
                    if (!headerLine.StartsWith('%') && !string.IsNullOrWhiteSpace(headerLine))
                    {
                        string[] parts = headerLine.Split(' ');
                        int numNodes = Convert.ToInt32(parts[0]);
                        adjacencyMatrix = new double[numNodes, numNodes];
                        break;
                    }
                }

                string? line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (!line.StartsWith('%') && !string.IsNullOrWhiteSpace(line))
                    {
                        string[] parts = line.Split(' ');
                        int source = Convert.ToInt32(parts[0]) - 1;
                        int target = Convert.ToInt32(parts[1]) - 1;
                        adjacencyMatrix[source, target] = 1;
                        adjacencyMatrix[target, source] = 1;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file was not found: " + file);
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading the file.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while reading the file.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sReader?.Close();
            }

            return adjacencyMatrix;
        }

        private static SortedDictionary<Node<int>, SortedSet<Node<int>>> MtxToAdjacencyList(
            string fileName
        )
        {
            SortedDictionary<Node<int>, SortedSet<Node<int>>> adjacencyList = new();
            string file = dataDirectory + fileName + ".mtx";
            StreamReader? sReader = null;

            try
            {
                sReader = new StreamReader(file);
                string? headerLine;
                while ((headerLine = sReader.ReadLine()) != null)
                {
                    if (!headerLine.StartsWith('%') && !string.IsNullOrWhiteSpace(headerLine))
                    {
                        string[] parts = headerLine.Split(' ');
                        int numNodes = Convert.ToInt32(parts[0]);
                        for (int i = 1; i <= numNodes; i++)
                        {
                            Node<int> node = Node<int>.GetOrCreateNode(i);
                            adjacencyList[node] = new SortedSet<Node<int>>();
                        }
                        break;
                    }
                }

                string? line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (!line.StartsWith('%') && !string.IsNullOrWhiteSpace(line))
                    {
                        string[] parts = line.Split(' ');
                        int source = Convert.ToInt32(parts[0]) - 1;
                        int target = Convert.ToInt32(parts[1]) - 1;
                        Node<int> sourceNode = Node<int>.GetOrCreateNode(source);
                        Node<int> targetNode = Node<int>.GetOrCreateNode(target);
                        adjacencyList[sourceNode].Add(targetNode);
                        adjacencyList[targetNode].Add(sourceNode);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file was not found: " + file);
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading the file.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while reading the file.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sReader?.Close();
            }

            return adjacencyList;
        }

        private static double[,] TxtToAdjacencyMatrix(string fileName)
        {
            double[,] adjacencyMatrix = new double[0, 0];
            string file = dataDirectory + fileName + ".txt";
            List<string> liens = new();

            StreamReader? sReader = null;
            try
            {
                sReader = new StreamReader(file);

                string? line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line.Replace(":", "")))
                    {
                        string[] parts = line.Split(
                            new[] { '(', ',', ')' },
                            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                        );

                        Node<int> sourceNode = Node<int>.GetOrCreateNode(Convert.ToInt32(parts[0]));
                        Node<int> targetNode = Node<int>.GetOrCreateNode(Convert.ToInt32(parts[1]));
                        int weight = Convert.ToInt32(parts[2]);

                        liens.Add($"{sourceNode.Id} {targetNode.Id} {weight}");
                    }
                }

                int numNodes = Node<int>.Count;
                adjacencyMatrix = new double[numNodes, numNodes];
                foreach (string lien in liens)
                {
                    string[] parts = lien.Split(' ');
                    int source = Convert.ToInt32(parts[0]);
                    int target = Convert.ToInt32(parts[1]);
                    int weight = Convert.ToInt32(parts[2]);
                    adjacencyMatrix[source, target] = weight;
                    adjacencyMatrix[target, source] = weight;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file was not found: " + file);
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading the file.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while reading the file.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sReader?.Close();
            }

            return adjacencyMatrix;
        }

        private static SortedDictionary<Node<int>, SortedSet<Node<int>>> TxtToAdjacencyList(
            string fileName
        )
        {
            SortedDictionary<Node<int>, SortedSet<Node<int>>> adjacencyList = new();
            string file = dataDirectory + fileName + ".txt";
            StreamReader? sReader = null;

            try
            {
                sReader = new StreamReader(file);
                string? line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line.Replace(":", "")))
                    {
                        string[] parts = line.Split(
                            new[] { '(', ',', ')' },
                            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                        );
                        int source = Convert.ToInt32(parts[0]);
                        int target = Convert.ToInt32(parts[1]);

                        Node<int> sourceNode = Node<int>.GetOrCreateNode(source);
                        Node<int> targetNode = Node<int>.GetOrCreateNode(target);

                        if (!adjacencyList.ContainsKey(sourceNode))
                        {
                            adjacencyList[sourceNode] = new SortedSet<Node<int>>();
                        }
                        if (!adjacencyList.ContainsKey(targetNode))
                        {
                            adjacencyList[targetNode] = new SortedSet<Node<int>>();
                        }

                        adjacencyList[sourceNode].Add(targetNode);
                        adjacencyList[targetNode].Add(sourceNode);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file was not found: " + file);
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading the file.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while reading the file.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sReader?.Close();
            }

            return adjacencyList;
        }
    }
}
