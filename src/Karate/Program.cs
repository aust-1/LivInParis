namespace Karate
{
    /// <summary>
    /// Programme principal : lit un fichier .mtx, construit le graphe, effectue BFS/DFS et dessine.
    /// </summary>
    public static class Program
    {
        private static double[,] MtxToAdjacencyMatrix(string fileName)
        {
            double[,] adjacencyMatrix = new double[0, 0];
            string file = "data/" + fileName + ".mtx";
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

        private static SortedDictionary<Node, SortedSet<Node>> MtxToAdjacencyList(string fileName)
        {
            SortedDictionary<Node, SortedSet<Node>> adjacencyList = new();
            string file = "data/" + fileName + ".mtx";
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
                            Node node = Node.GetOrCreateNode($"Node {i}");
                            adjacencyList[node] = new SortedSet<Node>();
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
                        Node sourceNode = Node.GetOrCreateNode(source);
                        Node targetNode = Node.GetOrCreateNode(target);
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

        static void Main(string[] args)
        {
            string fileName = "soc-karate";

            Graph graphe1 = new Graph(MtxToAdjacencyMatrix(fileName));
            Graph graphe2 = new Graph(MtxToAdjacencyList(fileName), true);

            Console.WriteLine("== BFS depuis le noeud 1 ==");
            var bfs1 = graphe1.BFS(1);
            Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs1));

            Console.WriteLine("== BFS depuis le noeud 1 ==");
            var bfs2 = graphe2.BFS(1);
            Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs2));

            Console.WriteLine("== DFS depuis le noeud 1 ==");
            var dfs1 = graphe1.DFSIterative(1);
            Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs1));

            Console.WriteLine("== DFS depuis le noeud 1 ==");
            var dfs2 = graphe2.DFSIterative(1);
            Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs2));

            AfficherSortedSet(graphe1.Nodes);
            AfficherSortedSet(graphe2.Nodes);

            AfficherMatrice(graphe1.AdjacencyMatrix);
            Console.WriteLine();
            AfficherMatrice(graphe2.AdjacencyMatrix);
            Console.WriteLine();

            AfficherListeAdjacence(graphe1.AdjacencyList);
            Console.WriteLine();
            AfficherListeAdjacence(graphe2.AdjacencyList);

            graphe2.DrawGraph("karate2.png");

            graphe1.DrawGraph("karate1.png");

            graphe2.DrawGraphForceLayout("karateForce2.png");

            graphe1.DrawGraphForceLayout("karateForce1.png");

            string[] layout = {"dot", "neato", "fdp", "sfdp", "twopi", "circo"};
            foreach (string l in layout)
            {
                GraphVisualization.ExportToDot(graphe1, "karate.dot", l);
                GraphVisualization.RenderDotFile("karate.dot", "karateDot" + l + ".png");
            }
        }

        static void AfficherSortedSet(SortedSet<Node> set)
        {
            foreach (Node node in set)
            {
                Console.Write(node.Name + " ");
            }
            Console.WriteLine();
        }

        static void AfficherMatrice(double[,] matrice)
        {
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void AfficherListeAdjacence(SortedDictionary<Node, SortedSet<Node>> matrice)
        {
            foreach (var kvp in matrice)
            {
                Console.Write(kvp.Key.Name + " : ");
                foreach (Node node in kvp.Value)
                {
                    Console.Write(node.Name + " ");
                }
                Console.WriteLine();
            }
        }
    }
}