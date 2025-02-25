using System.Drawing;

namespace Karate.Models
{
    /// <summary>
    /// Represents a graph containing a set of nodes (vertices) and edges,
    /// along with adjacency structures for traversal and visualization.
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
        /// Adjacency list mapping each node's ID to the set of adjacent node IDs.
        /// Key: Node ID
        /// Value: A set of IDs of adjacent nodes
        /// </summary>
        private readonly SortedDictionary<int, SortedSet<int>> _adjacencyList;

        /// <summary>
        /// Adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
        /// the presence (or weight) of an edge from <c>i</c> to <c>j</c>.
        /// </summary>
        private double[,]? _adjacencyMatrix;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="isDirected">Specifies if the graph is directed (<c>true</c>) or undirected (<c>false</c>).</param>
        public Graph(bool isDirected = false)
        {
            _nodes = new SortedSet<Node>();
            _edges = new List<Edge>();
            _isDirected = isDirected;
            _adjacencyList = new SortedDictionary<int, SortedSet<int>>();
            _adjacencyMatrix = null;
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
            if (!_adjacencyList.ContainsKey(node.Id))
            {
                _adjacencyList[node.Id] = new SortedSet<int>();
            }
        }

        /// <summary>
        /// Adds an edge between two existing nodes in the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);

            int sourceId = edge.SourceNode.Id;
            int targetId = edge.TargetNode.Id;

            if (!_adjacencyList.ContainsKey(sourceId))
            {
                _adjacencyList[sourceId] = new SortedSet<int>();
            }
            if (!_adjacencyList.ContainsKey(targetId))
            {
                _adjacencyList[targetId] = new SortedSet<int>();
            }

            _adjacencyList[sourceId].Add(targetId);

            if (!_isDirected)
            {
                _adjacencyList[targetId].Add(sourceId);
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
        /// Performs a Breadth-First Search (BFS) starting from the specified node ID.
        /// Returns the IDs of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start the BFS.</param>
        /// <returns>A list of node IDs in the order they were visited.</returns>
        public List<int> BFS(int startId)
        {
            var result = new List<int>();
            var queue = new Queue<int>();
            var visited = new HashSet<int>();

            queue.Enqueue(startId);
            visited.Add(startId);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                result.Add(current);

                foreach (int neighbor in _adjacencyList[current])
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
        /// Performs a recursive Depth-First Search (DFS) starting from the specified node ID.
        /// Returns the IDs of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start DFS.</param>
        /// <returns>A list of node IDs in the order they were visited.</returns>
        public List<int> DFSRecursive(int startId)
        {
            var visited = new HashSet<int>();
            var result = new List<int>();

            DFSUtil(startId, visited, result);
            return result;
        }

        /// <summary>
        /// Performs a recursive DFS starting from a node specified by its name.
        /// </summary>
        /// <param name="startName">The name of the start node.</param>
        /// <returns>A list of visited node IDs in the order they were discovered.</returns>
        /// <exception cref="ArgumentException">Thrown if the node name does not exist.</exception>
        public List<int> DFSRecursive(string startName)
        {
            int startId = Node.GetIdFromName(startName);
            if (startId == -1)
            {
                throw new ArgumentException($"Node '{startName}' not found.");
            }
            return DFSRecursive(startId);
        }

        /// <summary>
        /// Helper method for recursive DFS traversal.
        /// </summary>
        private void DFSUtil(int nodeId, HashSet<int> visited, List<int> result)
        {
            visited.Add(nodeId);
            result.Add(nodeId);

            foreach (int neighbor in _adjacencyList[nodeId])
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited, result);
                }
            }
        }

        /// <summary>
        /// Performs an iterative Depth-First Search (DFS) starting from the specified node ID.
        /// Returns the IDs of the visited nodes in the order they were discovered.
        /// </summary>
        /// <param name="startId">The ID of the node from which to start DFS.</param>
        /// <returns>A list of node IDs in the order they were visited.</returns>
        public List<int> DFSIterative(int startId)
        {
            var result = new List<int>();
            var stack = new Stack<int>();
            var visited = new HashSet<int>();

            stack.Push(startId);

            while (stack.Count > 0)
            {
                int current = stack.Pop();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    result.Add(current);

                    foreach (int neighbor in _adjacencyList[current])
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
        /// Performs an iterative DFS starting from a node specified by its name.
        /// </summary>
        /// <param name="startName">The name of the start node.</param>
        /// <returns>A list of visited node IDs in the order they were discovered.</returns>
        /// <exception cref="ArgumentException">Thrown if the node name does not exist.</exception>
        public List<int> DFSIterative(string startName)
        {
            int startId = Node.GetIdFromName(startName);
            if (startId == -1)
            {
                throw new ArgumentException($"Node '{startName}' not found.");
            }
            return DFSIterative(startId);
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
                int n = _nodes.Count;
                return BFS(0).Count == n;
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
        public bool DetectCycleOrCircuit()
        {
            var visited = new HashSet<int>();

            foreach (int nodeId in _nodes.Select(node => node.Id))
            {
                if (!visited.Contains(nodeId) && DFSDetectCycle(nodeId, -1, visited))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// DFS used for cycle detection in an undirected graph.
        /// Use <paramref name="parent"/> = -1 to indicate no parent for the root node.
        /// </summary>
        /// <param name="current">The current node ID.</param>
        /// <param name="parent">The parent node ID, or -1 if none.</param>
        /// <param name="visited">A set of visited node IDs.</param>
        /// <returns>
        /// <c>true</c> if a cycle is detected, <c>false</c> otherwise.
        /// </returns>
        private bool DFSDetectCycle(int current, int parent, HashSet<int> visited)
        {
            visited.Add(current);

            foreach (int neighbor in _adjacencyList[current])
            {
                if (!visited.Contains(neighbor))
                {
                    if (DFSDetectCycle(neighbor, current, visited))
                    {
                        return true;
                    }
                }
                else if (neighbor != parent)
                {
                    // If we find a visited neighbor that is not the parent, we have a cycle
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
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                // TODO : Placer les noeuds dans un cercle ou un layout spécifique
                // Ex: calculer la position (x, y) de chaque noeud
                int n = _nodes.Count;
                double angleStep = 2 * Math.PI / n;
                int radius = 200;
                Point center = new Point(width / 2, height / 2);

                var positions = new Dictionary<int, Point>();
                int i = 0;
                foreach (int nodeId in _nodes.Select(node => node.Id))
                {
                    double angle = i * angleStep;
                    int x = center.X + (int)(radius * Math.Cos(angle));
                    int y = center.Y + (int)(radius * Math.Sin(angle));
                    positions[nodeId] = new Point(x, y);
                    i++;
                }

                // Draw edges
                using Pen edgePen = new Pen(Color.Gray, 2);
                foreach (var kvp in _adjacencyList)
                {
                    int sourceId = kvp.Key;
                    foreach (int targetId in kvp.Value)
                    {
                        // In undirected graphs, avoid drawing the same edge twice
                        if (!_isDirected && sourceId < targetId)
                        {
                            g.DrawLine(edgePen, positions[sourceId], positions[targetId]);
                        }
                        else if (_isDirected)
                        {
                            // Draw arrow for directed edge
                            g.DrawLine(edgePen, positions[sourceId], positions[targetId]);

                            const int arrowSize = 5;
                            double angle = Math.Atan2(
                                positions[targetId].Y - positions[sourceId].Y,
                                positions[targetId].X - positions[sourceId].X);

                            PointF arrowPoint1 = new PointF(
                                (float)(positions[targetId].X - arrowSize * Math.Cos(angle - Math.PI / 6)),
                                (float)(positions[targetId].Y - arrowSize * Math.Sin(angle - Math.PI / 6)));

                            PointF arrowPoint2 = new PointF(
                                (float)(positions[targetId].X - arrowSize * Math.Cos(angle + Math.PI / 6)),
                                (float)(positions[targetId].Y - arrowSize * Math.Sin(angle + Math.PI / 6)));

                            g.DrawLine(edgePen, positions[targetId], arrowPoint1);
                            g.DrawLine(edgePen, positions[targetId], arrowPoint2);
                        }
                    }
                }

                // Draw nodes
                using Brush nodeBrush = Brushes.LightBlue;
                foreach (int nodeId in _nodes.Select(node => node.Id))
                {
                    Point p = positions[nodeId];
                    int size = 20;
                    g.FillEllipse(nodeBrush, p.X - size / 2, p.Y - size / 2, size, size);
                    g.DrawEllipse(Pens.Black, p.X - size / 2, p.Y - size / 2, size, size);

                    // Draw the node's ID
                    string text = nodeId.ToString();
                    SizeF textSize = g.MeasureString(text, SystemFonts.DefaultFont);
                    g.DrawString(text, SystemFonts.DefaultFont, Brushes.Black,
                                 p.X - textSize.Width / 2, p.Y - textSize.Height / 2);
                }

                // Save the image
                bitmap.Save(fileName);
            }
        }

        #endregion Drawing

        #region MTX File Import

        /// <summary>
        /// Reads a .mtx file and constructs a graph (directed or undirected).
        /// Expected format:
        ///   % ... (comments)
        ///   34 34 78   (numRows, numCols, numEdges)
        ///   2 1
        ///   3 1
        ///   ...
        /// </summary>
        /// <param name="filePath">The path to the .mtx file.</param>
        /// <param name="isDirected">Indicates whether the graph is directed.</param>
        /// <returns>A new <see cref="Graph"/> constructed from the file.</returns>
        public static Graph ReadMtxFile(string filePath, bool isDirected)
        {
            Graph graph = new Graph(isDirected);
            bool firstLineFound = false;
            int numNodes = 0, expectedEdgeCount = 0, edgesReadCount = 0;

            var nodesDict = new Dictionary<int, Node>();

            foreach (string line in File.ReadLines(filePath))
            {
                if (line.StartsWith('%') || string.IsNullOrWhiteSpace(line))
                    continue;

                if (!firstLineFound)
                {
                    (numNodes, expectedEdgeCount) = ParseHeaderLine(line);
                    firstLineFound = true;
                    InitializeNodeDictionary(numNodes, nodesDict, graph);
                }
                else
                {
                    if (TryParseEdge(line, nodesDict, out Edge? edge))
                    {
                        graph.AddEdge(edge);
                        edgesReadCount++;
                    }
                }
            }

            Console.WriteLine($".mtx file read: {numNodes} nodes, {edgesReadCount} edges (expected: {expectedEdgeCount}).");
            return graph;
        }

        /// <summary>
        /// Parses the first non-comment line of the .mtx file to extract the number of nodes and edges.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>A tuple of (numNodes, expectedEdgeCount).</returns>
        /// <exception cref="Exception">Thrown if the line format is invalid.</exception>
        private static (int, int) ParseHeaderLine(string line)
        {
            string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
            {
                throw new Exception("Invalid .mtx format: incomplete header line.");
            }

            int numNodes = int.Parse(parts[0]);
            int expectedEdges = int.Parse(parts[2]);
            return (numNodes, expectedEdges);
        }

        /// <summary>
        /// Initializes the internal dictionary of nodes and adds them to the graph.
        /// </summary>
        /// <param name="numNodes">The number of nodes to create.</param>
        /// <param name="nodesDict">A dictionary from integer IDs (1-based) to <see cref="Node"/> objects.</param>
        /// <param name="graph">The graph to which nodes will be added.</param>
        private static void InitializeNodeDictionary(int numNodes, Dictionary<int, Node> nodesDict, Graph graph)
        {
            for (int i = 1; i <= numNodes; i++)
            {
                Node node = new Node($"Node{i}");
                nodesDict[i] = node;
                graph.AddNode(node);
            }
        }

        /// <summary>
        /// Attempts to parse an edge (source, target) from a line in the .mtx file.
        /// </summary>
        /// <param name="line">The line containing the edge data.</param>
        /// <param name="nodesDict">Dictionary of existing node IDs to <see cref="Node"/> objects.</param>
        /// <param name="edge">The resulting <see cref="Edge"/> if parsing is successful, or <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if parsing succeeded; <c>false</c> otherwise.</returns>
        private static bool TryParseEdge(string line, Dictionary<int, Node> nodesDict, out Edge? edge)
        {
            edge = null;
            string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;

            int sourceId = int.Parse(parts[0]);
            int targetId = int.Parse(parts[1]);

            if (!nodesDict.ContainsKey(sourceId) || !nodesDict.ContainsKey(targetId))
            {
                Console.WriteLine($"Warning: ID {sourceId} or {targetId} is out of range [1..{nodesDict.Count}].");
                return false;
            }

            edge = new Edge(nodesDict[sourceId], nodesDict[targetId]);
            return true;
        }

        #endregion MTX File Import
    }
}