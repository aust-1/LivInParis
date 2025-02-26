using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text;
using System;
using System.IO;


namespace Karate.Models
{
    /// <summary>
    /// Represents a simple graph containing a set of nodes (vertices) and edges.
    /// </summary>
    public class Graph
    {
        #region Fields

        /// <summary>
        /// A sorted set of all nodes in this graph.
        /// </summary>
        private readonly SortedSet<Node> _nodes;

        /// <summary>
        /// A list of all edges in this graph.
        /// </summary>
        private readonly List<Edge> _edges;

        /// <summary>
        /// Indicates whether the graph is directed.
        /// If <c>false</c>, the graph is undirected.
        /// </summary>
        private readonly bool _isDirected;

        /// <summary>
        /// Adjacency list mapping each node to the set of its adjacent nodes.
        /// </summary>
        private readonly SortedDictionary<Node, SortedSet<Node>> _adjacencyList;

        /// <summary>
        /// Adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
        /// the presence (or weight) of an edge from <c>i</c> to <c>j</c>.
        /// </summary>
        private double[,]? _adjacencyMatrix;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="isDirected">Specifies if the graph is directed (<c>true</c>) or undirected (<c>false</c>).</param>
        public Graph(bool isDirected = false)
        {
            _nodes = new SortedSet<Node>();
            _edges = new List<Edge>();
            _isDirected = isDirected;
            _adjacencyList = new SortedDictionary<Node, SortedSet<Node>>();
            _adjacencyMatrix = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="adjacencyList">A dictionary mapping each node to its set of adjacent nodes.</param>
        /// <param name="isDirected">Specifies if the graph is directed (<c>true</c>) or undirected (<c>false</c>).</param>
        public Graph(SortedDictionary<Node, SortedSet<Node>> adjacencyList, bool isDirected = false)
        {
            _isDirected = isDirected;
            _nodes = new SortedSet<Node>(adjacencyList.Keys);
            _edges = new List<Edge>();
            _adjacencyList = adjacencyList;
            _adjacencyMatrix = new double[_nodes.Count, _nodes.Count];
            foreach (var kvp in adjacencyList)
            {
                foreach (Node neighbor in kvp.Value)
                {
                    _edges.Add(new Edge(kvp.Key, neighbor));
                    _adjacencyMatrix[kvp.Key.Id, neighbor.Id] = 1.0;
                    _adjacencyMatrix[neighbor.Id, kvp.Key.Id] = 1.0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="adjacencyMatrix">A 2D array representing the adjacency matrix of the graph.</param>
        /// <param name="isDirected">Specifies if the graph is directed (<c>true</c>) or undirected (<c>false</c>).</param>
        /// <exception cref="ArgumentException">Thrown if the adjacency matrix is not square.</exception>
        public Graph(double[,] adjacencyMatrix, bool isDirected = false)
        {
            if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
            {
                throw new ArgumentException("Adjacency matrix must be square.");
            }

            _isDirected = isDirected;
            _nodes = new SortedSet<Node>();
            _edges = new List<Edge>();
            _adjacencyList = new SortedDictionary<Node, SortedSet<Node>>();
            _adjacencyMatrix = adjacencyMatrix;

            int n = adjacencyMatrix.GetLength(0);
            for (int i = 1; i <= n; i++)
            {
                Node node = Node.GetOrCreateNode($"Node {i}");
                _nodes.Add(node);
            }

            foreach (Node source in _nodes)
            {
                foreach (Node target in _nodes)
                {
                    double weight = adjacencyMatrix[source.Id, target.Id];
                    if (weight != 0.0)
                    {
                        AddEdge(new Edge(source, target, weight));
                    }
                }
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the adjacency matrix representing the graph. If not yet built, it will be constructed on-demand.
        /// </summary>
        public double[,]? AdjacencyMatrix
        {
            get
            {
                BuildAdjacencyMatrix();
                return _adjacencyMatrix;
            }
        }

        /// <summary>
        /// Gets the adjacency list representing the graph.
        /// </summary>
        public SortedDictionary<Node, SortedSet<Node>> AdjacencyList
        {
            get { return _adjacencyList; }
        }

        /// <summary>
        /// Gets the list of nodes in the graph.
        /// </summary>
        public SortedSet<Node> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Gets the list of edges in the graph.
        /// </summary>
        public List<Edge> Edges
        {
            get { return _edges; }
        }

        /// <summary>
        /// Gets the number of nodes (the 'order' of the graph).
        /// </summary>
        public int Order
        {
            get { return _nodes.Count; }
        }

        /// <summary>
        /// Gets the number of edges (the 'size' of the graph).
        /// </summary>
        public int Size
        {
            get { return _edges.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the graph is directed.
        /// </summary>
        public bool IsDirected
        {
            get { return _isDirected; }
        }

        /// <summary>
        /// Gets a value indicating whether the graph is weighted (i.e., if any edge has a weight different from 1.0).
        /// </summary>
        public bool IsWeighted
        {
            get { return _edges.Any(edge => edge.Weight != 1.0); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(Node node)
        {
            _nodes.Add(node);
            if (!_adjacencyList.ContainsKey(node))
            {
                _adjacencyList[node] = new SortedSet<Node>();
            }
        }

        /// <summary>
        /// Adds an edge between two existing nodes in the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(Edge edge)
        {
            if (_isDirected && _edges.Contains(edge))
            {
                return;
            }

            _edges.Add(edge);

            if (!_adjacencyList.ContainsKey(edge.SourceNode))
            {
                _adjacencyList[edge.SourceNode] = new SortedSet<Node>();
            }
            if (!_adjacencyList.ContainsKey(edge.TargetNode))
            {
                _adjacencyList[edge.TargetNode] = new SortedSet<Node>();
            }

            _adjacencyList[edge.SourceNode].Add(edge.TargetNode);

            if (!_isDirected)
            {
                _adjacencyList[edge.TargetNode].Add(edge.SourceNode);
            }
        }

        /// <summary>
        /// Builds the adjacency matrix for the graph, allocating a new 2D array and setting
        /// the appropriate edge weights.
        /// </summary>
        public void BuildAdjacencyMatrix()
        {
            int n = _nodes.Count;
            _adjacencyMatrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _adjacencyMatrix[i, j] = 0.0;
                }
            }

            foreach (Edge edge in _edges)
            {
                int i = edge.SourceNode.Id;
                int j = edge.TargetNode.Id;
                double w = edge.Weight;

                _adjacencyMatrix[i, j] = w;

                if (!_isDirected)
                {
                    _adjacencyMatrix[j, i] = w;
                }
            }
        }

        #endregion Methods

        #region Traversal

        /// <summary>
        /// Performs a Breadth-First Search (BFS) starting from the specified node.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startNode">The node from which to start the BFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startNode is not in the graph.</exception>
        public List<string> BFS(Node? startNode)
        {

            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
            {
                throw new ArgumentException("Invalid startNode.");
            }

            var result = new List<string>();
            var queue = new Queue<Node>();
            var visited = new HashSet<Node>();

            queue.Enqueue(startNode);
            visited.Add(startNode);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                result.Add(current.Name);

                foreach (Node neighbor in _adjacencyList[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs a Breadth-First Search (BFS) starting from the specified node ID.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start the BFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startId is not in the graph.</exception>
        public List<string> BFS(int startId)
        {
            return BFS(Node.GetOrCreateNode(startId));
        }

        /// <summary>
        /// Performs a Breadth-First Search (BFS) starting from the specified node name.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startName">The name of the node from which to start the BFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startName is not in the graph.</exception>
        public List<string> BFS(string startName)
        {
            return BFS(Node.GetOrCreateNode(startName));
        }

        /// <summary>
        /// Performs a recursive Depth-First Search (DFS) starting from the specified node.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startNode">The node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startNode is not in the graph.</exception>
        public List<string> DFSRecursive(Node? startNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
            {
                throw new ArgumentException("Invalid startNode.");
            }

            var visited = new HashSet<Node>();
            var result = new List<string>();

            DFSUtil(startNode, visited, result);
            return result;
        }

        /// <summary>
        /// Performs a recursive Depth-First Search (DFS) starting from the specified node ID.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startId is not in the graph.</exception>
        public List<string> DFSRecursive(int startId)
        {
            return DFSRecursive(Node.GetOrCreateNode(startId));
        }

        /// <summary>
        /// Performs a recursive Depth-First Search (DFS) starting from the specified node name.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startName">The name of the node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startName is not in the graph.</exception>
        public List<string> DFSRecursive(string startName)
        {
            return DFSRecursive(Node.GetOrCreateNode(startName));
        }

        /// <summary>
        /// Helper method for recursive DFS traversal.
        /// </summary>
        private void DFSUtil(Node node, HashSet<Node> visited, List<string> result)
        {
            visited.Add(node);
            result.Add(node.Name);

            foreach (Node neighbor in _adjacencyList[node])
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited, result);
                }
            }
        }

        /// <summary>
        /// Performs an iterative Depth-First Search (DFS) starting from the specified node.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startNode">The node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startNode is not in the graph.</exception>
        public List<string> DFSIterative(Node? startNode)
        {
            if (startNode == null || !_adjacencyList.ContainsKey(startNode))
            {
                throw new ArgumentException("Invalid startNode.");
            }

            var result = new List<string>();
            var stack = new Stack<Node>();
            var visited = new HashSet<Node>();

            stack.Push(startNode);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    result.Add(current.Name);

                    foreach (Node neighbor in _adjacencyList[current])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs an iterative Depth-First Search (DFS) starting from the specified node ID.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startId is not in the graph.</exception>
        public List<string> DFSIterative(int startId)
        {
            return DFSIterative(Node.GetOrCreateNode(startId));
        }

        /// <summary>
        /// Performs an iterative Depth-First Search (DFS) starting from the specified node name.
        /// Returns the names of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startName">The name of the node from which to start DFS.</param>
        /// <returns>A list of node names in the order they were visited.</returns>
        /// <exception cref="ArgumentException">Thrown if the startName is not in the graph.</exception>
        public List<string> DFSIterative(string startName)
        {
            return DFSIterative(Node.GetOrCreateNode(startName));
        }

        #endregion Traversal

        #region Graph Algorithms

        /// <summary>
        /// Checks if the graph is connected (valid for undirected graphs only).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the graph is connected, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if this method is called on a directed graph.</exception>
        public bool IsConnected()
        {
            if (!_isDirected)
            {
                Node startNode = _nodes.First();
                return BFS(startNode.Id).Count == _nodes.Count;
            }

            throw new InvalidOperationException("Algorithm not implemented for directed graphs.");
        }

        /// <summary>
        /// Detects the presence of a cycle (in an undirected graph) or circuit (in a directed graph).
        /// Currently, the cycle detection is implemented for undirected graphs only.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a cycle is found, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if this method is called on a directed graph.</exception>
        public bool DetectCycleOrCircuit()
        {
            if (!_isDirected)
            {
                var visited = new HashSet<Node>();

                foreach (Node node in _nodes)
                {
                    if (!visited.Contains(node) && DFSDetectCycle(node, null, visited))
                    {
                        return true;
                    }
                }

                return false;
            }

            throw new InvalidOperationException("Algorithm not implemented for directed graphs.");
        }

        /// <summary>
        /// DFS used for cycle detection in an undirected graph.
        /// Use <paramref name="parent"/> = null to indicate no parent for the root node.
        /// </summary>
        /// <param name="current">The current node.</param>
        /// <param name="parent">The parent node, or null if none.</param>
        /// <param name="visited">A set of visited nodes.</param>
        /// <returns>
        /// <c>true</c> if a cycle is detected, <c>false</c> otherwise.
        /// </returns>
        private bool DFSDetectCycle(Node current, Node? parent, HashSet<Node> visited)
        {
            visited.Add(current);

            foreach (Node neighbor in _adjacencyList[current])
            {
                if (!visited.Contains(neighbor))
                {
                    if (DFSDetectCycle(neighbor, current, visited))
                    {
                        return true;
                    }
                }
                else if (parent == null || neighbor != parent)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion Graph Algorithms

        #region Drawing

        /// <summary>
        /// Draws the graph to a PNG image file with nodes arranged in a circular layout.
        /// </summary>
        /// <param name="fileName">The name of the output file (default is "graph.png").</param>
        public void DrawGraph(string fileName = "graph.png")
        {
            const int width = 800;
            const int height = 600;

            int minDimension = Math.Min(width, height);
            int nodeSize = Math.Max(20, Math.Min(40, minDimension / (2 * Math.Max(1, _nodes.Count))));

            using var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var positions = new Dictionary<Node, Point>();
            int n = _nodes.Count;
            double angleStep = 2 * Math.PI / n;
            int radius = 200;
            Point center = new Point(width / 2, height / 2);

            int i = 0;
            foreach (Node node in _nodes)
            {
                double angle = i * angleStep;
                int x = center.X + (int)(radius * Math.Cos(angle));
                int y = center.Y + (int)(radius * Math.Sin(angle));

                x = Math.Max(nodeSize, Math.Min(x, width - nodeSize));
                y = Math.Max(nodeSize, Math.Min(y, height - nodeSize));

                positions[node] = new Point(x, y);
                i++;
            }

            using (var edgePen = new Pen(Color.Gray, 2))
            {
                foreach (var kvp in _adjacencyList)
                {
                    Node source = kvp.Key;
                    foreach (Node target in kvp.Value)
                    {
                        if (!_isDirected && source.Id < target.Id)
                        {
                            g.DrawLine(edgePen, positions[source], positions[target]);
                        }
                        else if (_isDirected)
                        {
                            int arrowSize = (int)(nodeSize * 0.4);
                            double angle = Math.Atan2(
                                positions[target].Y - positions[source].Y,
                                positions[target].X - positions[source].X);

                            float stopX = positions[target].X - (nodeSize / 2 * (float)Math.Cos(angle));
                            float stopY = positions[target].Y - (nodeSize / 2 * (float)Math.Sin(angle));

                            g.DrawLine(edgePen, positions[source], new PointF(stopX, stopY));

                            PointF arrowPoint1 = new PointF(
                                stopX - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                                stopY - arrowSize * (float)Math.Sin(angle - Math.PI / 6));

                            PointF arrowPoint2 = new PointF(
                                stopX - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                                stopY - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

                            g.DrawLine(edgePen, new PointF(stopX, stopY), arrowPoint1);
                            g.DrawLine(edgePen, new PointF(stopX, stopY), arrowPoint2);
                        }
                    }
                }
            }

            using (var nodeBrush = new SolidBrush(Color.LightBlue))
            using (var nodePen = new Pen(Color.Black, 1))
            {
                foreach (Node node in _nodes)
                {
                    Point p = positions[node];

                    Rectangle nodeRect = new Rectangle(
                        p.X - nodeSize / 2,
                        p.Y - nodeSize / 2,
                        nodeSize,
                        nodeSize
                    );

                    if (nodeRect.X >= 0 && nodeRect.Y >= 0 &&
                        nodeRect.Right <= width && nodeRect.Bottom <= height)
                    {
                        g.FillEllipse(nodeBrush, nodeRect);
                        g.DrawEllipse(nodePen, nodeRect);

                        string text = node.Name;
                        using (var font = new Font(SystemFonts.DefaultFont.FontFamily, 8))
                        {
                            SizeF textSize = g.MeasureString(text, font);
                            g.DrawString(text, font, Brushes.Black,
                                p.X - textSize.Width / 2,
                                p.Y - textSize.Height / 2);
                        }
                    }
                }
            }

            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                byte[] bytes = memory.ToArray();
                File.WriteAllBytes(fileName, bytes);
            }
        }

        public void DisplayGraph(string outputImageName = "graph", string layout = "dot")
        {
            string dotFilePath = $"{outputImageName}.dot";
            string outputImagePath = $"{outputImageName}.png";

            ExportToDot(dotFilePath);
            RenderDotFile(dotFilePath, outputImagePath);
        }

        private static void RenderDotFile(string dotFilePath, string outputImagePath)
        {
            if (!File.Exists(dotFilePath))
            {
                Console.WriteLine("Le fichier DOT spécifié n'existe pas.");
                return;
            }

            string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
            if (!File.Exists(graphVizPath))
            {
                Console.WriteLine("GraphViz is not installed on this machine. Install it or use the other method to render the graph.");
                Console.WriteLine("Do you want to install it now? (y/n) (y provides a silent installation with winget)");
                string response = Console.ReadLine();
                if (response.ToLower() == "y")
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (Process processGraphVizInstall = new Process { StartInfo = psi })
                    {
                        processGraphVizInstall.Start();
                        string output = processGraphVizInstall.StandardOutput.ReadToEnd();
                        string error = processGraphVizInstall.StandardError.ReadToEnd();
                        processGraphVizInstall.WaitForExit();

                        Console.WriteLine("Output:\n" + output);
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine("Error:\n" + error);
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = graphVizPath,
                    Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Export the graph to a DOT file.
        /// </summary>
        /// <param name="filePath">The path of the DOT file to generate.</param>
        /// <param name="layout">The layout algorithm to use (default is "dot").</param>
        private void ExportToDot(string filePath, string layout = "dot")
        {
            StringBuilder dotBuilder = new StringBuilder();
            dotBuilder.AppendLine(_isDirected ? "digraph G {" : "graph G {");
            dotBuilder.AppendLine($"    layout={layout};");

            foreach (var node in _nodes)
            {
                dotBuilder.AppendLine($"    \"{node.Name}\";");
            }

            foreach (var edge in _edges)
            {
                if (!_isDirected && edge.SourceNode.Id.CompareTo(edge.TargetNode.Id) > 0)
                {
                    dotBuilder.AppendLine($"    \"{edge.SourceNode.Name}\" {"--"} \"{edge.TargetNode.Name}\";");
                }
                else if (_isDirected)
                {
                    dotBuilder.AppendLine($"    \"{edge.SourceNode.Name}\" {"->"} \"{edge.TargetNode.Name}\";");
                }
            }

            dotBuilder.AppendLine("}");

            File.WriteAllText(filePath, dotBuilder.ToString());
        }

        #endregion Drawing
    }
}