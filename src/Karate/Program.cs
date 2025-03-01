using System.Runtime.CompilerServices;

namespace Karate
{
    public static class Program
    {
        private const string dataDirectory = "data/";

        static void Main(string[] args)
        {
            string fileName = "soc-karate";

            Graph test = new Graph(TxtToAdjacencyList(fileName));

            Console.WriteLine("== BFS depuis le noeud 1 ==");
            var bfs1 = test.BFS(1);
            Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs1));

            Console.WriteLine("== DFS depuis le noeud 1 ==");
            var dfs1 = test.DFSIterative(1);
            Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs1));

            test.DrawGraph("karate1");

            Console.WriteLine(test.FindAnyCycle(true));
            Console.WriteLine(test.FindAnyCycle(false));
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

        private static SortedDictionary<Node, SortedSet<Node>> MtxToAdjacencyList(string fileName)
        {
            SortedDictionary<Node, SortedSet<Node>> adjacencyList = new();
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
                            Node node = Node.GetOrCreateNode(i);
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

        private static double[,] TtxToAdjacencyMatrix(string fileName)
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

                        Node sourceNode = Node.GetOrCreateNode(parts[0]);
                        Node targetNode = Node.GetOrCreateNode(parts[1]);
                        int weight = Convert.ToInt32(parts[2]);

                        liens.Add($"{sourceNode.Id} {targetNode.Id} {weight}");
                    }
                }

                int numNodes = Node.Count;
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

        private static SortedDictionary<Node, SortedSet<Node>> TxtToAdjacencyList(string fileName)
        {
            SortedDictionary<Node, SortedSet<Node>> adjacencyList = new();
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
                        string source = parts[0];
                        string target = parts[1];

                        Node sourceNode = Node.GetOrCreateNode(source);
                        Node targetNode = Node.GetOrCreateNode(target);

                        if (!adjacencyList.ContainsKey(sourceNode))
                        {
                            adjacencyList[sourceNode] = new SortedSet<Node>();
                        }
                        if (!adjacencyList.ContainsKey(targetNode))
                        {
                            adjacencyList[targetNode] = new SortedSet<Node>();
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
