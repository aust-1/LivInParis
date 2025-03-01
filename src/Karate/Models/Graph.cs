using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Karate.Models;

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
    /// Indicates whether the graph is directed. If <c>false</c>, the graph is undirected.
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
    /// Initializes a new instance of the <see cref="Graph"/> class, optionally as directed or undirected.
    /// </summary>
    /// <param name="isDirected">
    /// <c>true</c> if the graph should be directed; <c>false</c> for an undirected graph.
    /// </param>
    public Graph(bool isDirected = false)
    {
        _nodes = new SortedSet<Node>();
        _edges = new List<Edge>();
        _isDirected = isDirected;
        _adjacencyList = new SortedDictionary<Node, SortedSet<Node>>();
        _adjacencyMatrix = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph"/> class from a given adjacency list.
    /// Automatically determines whether the graph is directed based on the symmetry of the adjacency matrix.
    /// </summary>
    /// <param name="adjacencyList">A dictionary mapping each node to its set of adjacent nodes.</param>
    /// <remarks>
    /// This constructor infers <see cref="_isDirected"/> by checking if the adjacency matrix is symmetric.
    /// If it is not symmetric, the graph is assumed to be directed.
    /// </remarks>
    public Graph(SortedDictionary<Node, SortedSet<Node>> adjacencyList)
    {
        _nodes = new SortedSet<Node>(adjacencyList.Keys);
        _edges = new List<Edge>();
        _adjacencyList = adjacencyList;
        _adjacencyMatrix = new double[_nodes.Count, _nodes.Count];

        foreach (var kvp in adjacencyList)
        {
            Node source = kvp.Key;
            foreach (Node neighbor in kvp.Value)
            {
                _edges.Add(new Edge(source, neighbor));
                _adjacencyMatrix[source.Id, neighbor.Id] = 1.0;
            }
        }

        _isDirected = !CheckIfSymmetric(_adjacencyMatrix);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph"/> class from a given adjacency matrix.
    /// Automatically determines whether the graph is directed by checking for symmetry.
    /// </summary>
    /// <param name="adjacencyMatrix">A 2D array representing the adjacency matrix of the graph.</param>
    /// <exception cref="ArgumentException">Thrown if the adjacency matrix is not square.</exception>
    public Graph(double[,] adjacencyMatrix)
    {
        if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
        {
            throw new ArgumentException("Adjacency matrix must be square.");
        }

        _isDirected = !CheckIfSymmetric(adjacencyMatrix);
        _nodes = new SortedSet<Node>();
        _edges = new List<Edge>();
        _adjacencyList = new SortedDictionary<Node, SortedSet<Node>>();
        _adjacencyMatrix = adjacencyMatrix;

        int n = adjacencyMatrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            Node node = Node.GetOrCreateNode(i);
            _nodes.Add(node);
        }

        foreach (Node source in _nodes)
        {
            foreach (Node target in _nodes)
            {
                double weight = adjacencyMatrix[source.Id, target.Id];
                if (!Equals(weight, 0.0))
                {
                    AddEdge(new Edge(source, target, weight, _isDirected));
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
    /// Gets the set of all nodes in this graph.
    /// </summary>
    public SortedSet<Node> Nodes
    {
        get { return _nodes; }
    }

    /// <summary>
    /// Gets the collection of edges in this graph.
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
    /// Gets the density of the graph, computed as (2 * E) / (V * (V - 1)) for undirected,
    /// or (E) / (V * (V - 1)) for directed graphs.
    /// </summary>
    public double Density
    {
        get
        {
            int orientedFactor = _isDirected ? 1 : 2;
            return (double)Size * orientedFactor / (Order * (Order - 1));
        }
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
        get { return _edges.Any(edge => !Equals(edge.Weight, 1.0)); }
    }

    /// <summary>
    /// Checks if the graph is connected. (Uses BFS on the first node.)
    /// </summary>
    /// <remarks>
    /// For directed graphs, 'connected' may be ambiguous
    /// (strongly vs. weakly connected). Here, it checks if BFS from the
    /// first node can reach all nodes.
    /// </remarks>
    public bool IsConnected
    {
        get { return BFS(_nodes.First().Id).Count == _nodes.Count; }
    }

    #endregion Properties

    #region Public Methods

    /// <summary>
    /// Adds a node to the graph. If the node is new, it is also added to the adjacency list.
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
    /// <param name="edge">The edge to add, containing source, target, and (optionally) weight.</param>
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
    /// Builds or rebuilds the adjacency matrix for the graph by allocating
    /// a new 2D array and setting the appropriate edge weights.
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

    /// <summary>
    /// Finds any cycle in the graph, either in a directed or undirected manner.
    /// Optionally restricts detection to "simple" cycles in an undirected graph
    /// (i.e., ignoring the immediate parent).
    /// </summary>
    /// <param name="simpleCycle">
    /// If <c>true</c> (and the graph is undirected), ignore the direct parent to avoid
    /// trivial cycles. Default is <c>false</c>.
    /// </param>
    /// <returns>
    /// A string describing the cycle (both by node ID and name), or <c>null</c> if no cycle is found.
    /// </returns>
    public string? FindAnyCycle(bool simpleCycle = false)
    {
        var visited = new HashSet<Node>();
        var recStack = new HashSet<Node>();
        var parentMap = new Dictionary<Node, Node>();

        foreach (var node in _nodes)
        {
            if (!visited.Contains(node))
            {
                if (_isDirected)
                {
                    if (
                        TryFindCycleDirected(
                            node,
                            visited,
                            recStack,
                            parentMap,
                            out var cycle,
                            simpleCycle
                        )
                    )
                    {
                        return CycleToString(cycle);
                    }
                }
                else
                {
                    if (
                        TryFindCycleUndirected(
                            node,
                            visited,
                            parentMap,
                            null,
                            out var cycle,
                            simpleCycle
                        )
                    )
                    {
                        return CycleToString(cycle);
                    }
                }
            }
        }
        return null;
    }
    #endregion Public Methods

    #region Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node.
    /// Returns the names of the visited nodes in the order they were discovered.
    /// Supports input types: <see cref="Node"/>, <see cref="int"/> (Node ID), and <see cref="string"/> (Node Name).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the start node, which can be <see cref="Node"/>, <see cref="int"/>, or <see cref="string"/>.
    /// </typeparam>
    /// <param name="start">The node or identifier from which to start BFS.</param>
    /// <returns>A list of node names in the order they were visited.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the start node type is invalid or the start node is not in the graph.
    /// </exception>
    public List<string> BFS<T>(T start)
        where T : notnull
    {
        Node startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
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
    /// Performs a recursive Depth-First Search (DFS) starting from the specified node.
    /// Returns the names of the visited nodes in the order they were discovered.
    /// Supports input types: <see cref="Node"/>, <see cref="int"/> (Node ID), and <see cref="string"/> (Node Name).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the start node, which can be <see cref="Node"/>, <see cref="int"/>, or <see cref="string"/>.
    /// </typeparam>
    /// <param name="start">The node or identifier from which to start DFS.</param>
    /// <returns>A list of node names in the order they were visited.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the start node type is invalid or the start node is not in the graph.
    /// </exception>
    public List<string> DFSRecursive<T>(T start)
        where T : notnull
    {
        Node startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var visited = new HashSet<Node>();
        var result = new List<string>();

        DFSUtil(startNode, visited, result);
        return result;
    }

    /// <summary>
    /// Performs an iterative Depth-First Search (DFS) starting from the specified node.
    /// Returns the names of the visited nodes in the order they were discovered.
    /// Supports input types: <see cref="Node"/>, <see cref="int"/> (Node ID), and <see cref="string"/> (Node Name).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the start node, which can be <see cref="Node"/>, <see cref="int"/>, or <see cref="string"/>.
    /// </typeparam>
    /// <param name="start">The node or identifier from which to start DFS.</param>
    /// <returns>A list of node names in the order they were visited.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the start node type is invalid or the start node is not in the graph.
    /// </exception>
    public List<string> DFSIterative<T>(T start)
        where T : notnull
    {
        Node startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
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

    #endregion Traversals

    #region Drawing

#pragma warning disable CA1416
    /// <summary>
    /// Draws the graph to a PNG image file with nodes arranged in a circular layout.
    /// </summary>
    /// <param name="fileName">The base name of the output file (default is "graph").</param>
    /// <remarks>
    /// The final image is saved under <c>data/output/</c> with a timestamp
    /// to avoid overwriting existing files.
    /// </remarks>
    public void DrawGraph(string fileName = "graph")
    {
        const int width = 800;
        const int height = 600;

        string filePath = $"data/output/{fileName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

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

            x = Math.Clamp(x, nodeSize, width - nodeSize);
            y = Math.Clamp(y, nodeSize, height - nodeSize);

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
                            positions[target].X - positions[source].X
                        );

                        float stopX =
                            positions[target].X - (nodeSize / 2f * (float)Math.Cos(angle));
                        float stopY =
                            positions[target].Y - (nodeSize / 2f) * (float)Math.Sin(angle);

                        g.DrawLine(edgePen, positions[source], new PointF(stopX, stopY));

                        PointF arrowPoint1 = new PointF(
                            stopX - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                            stopY - arrowSize * (float)Math.Sin(angle - Math.PI / 6)
                        );

                        PointF arrowPoint2 = new PointF(
                            stopX - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                            stopY - arrowSize * (float)Math.Sin(angle + Math.PI / 6)
                        );

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
                var nodeRect = new Rectangle(
                    p.X - nodeSize / 2,
                    p.Y - nodeSize / 2,
                    nodeSize,
                    nodeSize
                );

                g.FillEllipse(nodeBrush, nodeRect);
                g.DrawEllipse(nodePen, nodeRect);

                string text = node.Name;
                using var font = new Font(SystemFonts.DefaultFont.FontFamily, 8);
                SizeF textSize = g.MeasureString(text, font);

                g.DrawString(
                    text,
                    font,
                    Brushes.Black,
                    p.X - textSize.Width / 2,
                    p.Y - textSize.Height / 2
                );
            }
        }

        using (var memory = new MemoryStream())
        {
            bitmap.Save(memory, ImageFormat.Png);
            byte[] bytes = memory.ToArray();
            File.WriteAllBytes(filePath, bytes);
        }
    }

#pragma warning restore CA1416

    /// <summary>
    /// Exports the graph to a DOT file, then uses GraphViz to generate a PNG image,
    /// and finally deletes the DOT file.
    /// </summary>
    /// <param name="outputImageName">
    /// The base name for the output. The final file path is timestamped to avoid overwrites.
    /// </param>
    /// <param name="layout">The GraphViz layout to use (e.g., "dot", "neato", etc.).</param>
    public void DisplayGraph(string outputImageName = "graph", string layout = "dot")
    {
        string dotFilePath = $"{outputImageName}.dot";
        string outputImagePath =
            $"data/output/{outputImageName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

        ExportToDot(dotFilePath, layout);
        RenderDotFile(dotFilePath, outputImagePath);
        File.Delete(dotFilePath);
    }

    #endregion Drawing

    #region Private Helpers

    /// <summary>
    /// Checks if the given matrix is symmetric along the main diagonal.
    /// </summary>
    /// <param name="matrix">The adjacency matrix to check.</param>
    /// <returns><c>true</c> if the matrix is symmetric; otherwise <c>false</c>.</returns>
    private static bool CheckIfSymmetric(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (!Equals(matrix[i, j], matrix[j, i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Resolves a given object into a <see cref="Node"/>.
    /// Supported types: <see cref="Node"/>, <see cref="int"/>, <see cref="string"/>.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <param name="start">The node or ID/name to convert to a <see cref="Node"/>.</param>
    /// <returns>The corresponding <see cref="Node"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the type is unsupported.</exception>
    private static Node ResolveNode<T>(T start)
        where T : notnull
    {
        return start switch
        {
            Node node => node,
            int id => Node.GetOrCreateNode(id),
            string name => Node.GetOrCreateNode(name),
            _ => throw new ArgumentException(
                "Invalid start node type. Accepted types: Node, int, string."
            ),
        };
    }

    #region Cycle Detection Helpers

    /// <summary>
    /// Attempts to find a cycle in a directed graph using DFS with recursion stack tracking.
    /// </summary>
    private bool TryFindCycleDirected(
        Node current,
        HashSet<Node> visited,
        HashSet<Node> recStack,
        Dictionary<Node, Node> parentMap,
        out List<Node> cycle,
        bool simpleCycle = false
    )
    {
        visited.Add(current);
        recStack.Add(current);

        if (_adjacencyList.TryGetValue(current, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    parentMap[neighbor] = current;
                    if (
                        TryFindCycleDirected(
                            neighbor,
                            visited,
                            recStack,
                            parentMap,
                            out cycle,
                            simpleCycle
                        )
                    )
                    {
                        return true;
                    }
                }
                else if (recStack.Contains(neighbor))
                {
                    cycle = ReconstructCycle(current, neighbor, parentMap);
                    return true;
                }
            }
        }

        recStack.Remove(current);
        cycle = null!;
        return false;
    }

    /// <summary>
    /// Attempts to find a cycle in an undirected graph using DFS.
    /// If <paramref name="simpleCycle"/> is <c>true</c>, immediate parent edges are ignored.
    /// </summary>
    private bool TryFindCycleUndirected(
        Node current,
        HashSet<Node> visited,
        Dictionary<Node, Node> parentMap,
        Node? parent,
        out List<Node> cycle,
        bool simpleCycle = false
    )
    {
        visited.Add(current);

        if (_adjacencyList.TryGetValue(current, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (neighbor.Equals(parent) && simpleCycle)
                {
                    continue;
                }
                else if (!visited.Contains(neighbor))
                {
                    parentMap[neighbor] = current;
                    if (
                        TryFindCycleUndirected(
                            neighbor,
                            visited,
                            parentMap,
                            current,
                            out cycle,
                            simpleCycle
                        )
                    )
                    {
                        return true;
                    }
                }
                else
                {
                    cycle = ReconstructCycle(current, neighbor, parentMap);
                    return true;
                }
            }
        }

        cycle = null!;
        return false;
    }

    /// <summary>
    /// Reconstructs the cycle path from a meeting point of <paramref name="current"/> and <paramref name="neighbor"/>.
    /// </summary>
    private static List<Node> ReconstructCycle(
        Node current,
        Node neighbor,
        Dictionary<Node, Node> parentMap
    )
    {
        var cycle = new List<Node>();
        var temp = current;
        while (!temp.Equals(neighbor))
        {
            cycle.Add(temp);
            temp = parentMap[temp];
        }
        cycle.Add(neighbor);
        cycle.Reverse();
        return cycle;
    }

    /// <summary>
    /// Returns a textual representation of the cycle, including IDs and Names.
    /// </summary>
    private static string CycleToString(List<Node> cycle)
    {
        StringBuilder sbId = new StringBuilder();
        StringBuilder sbName = new StringBuilder();

        sbId.Append("Cycle by Id: <");
        sbName.Append("\nCycle by Name: ");

        foreach (Node node in cycle)
        {
            sbId.Append(node.Id).Append(", ");
            sbName.Append(node.Name).Append(" -> ");
        }

        sbId.Append(cycle[0].Id).Append('>');
        sbName.Append(cycle[0].Name);

        return sbId.Append(sbName).ToString();
    }

    #endregion Cycle Detection Helpers

    #region DFS Helpers

    /// <summary>
    /// Recursively visits nodes for a Depth-First Search, storing visited nodes in a result list.
    /// </summary>
    private void DFSUtil(Node node, HashSet<Node> visited, List<string> result)
    {
        visited.Add(node);
        result.Add(node.Name);

        if (_adjacencyList.TryGetValue(node, out var neighbors))
        {
            foreach (Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited, result);
                }
            }
        }
    }

    #endregion DFS Helpers

    /// <summary>
    /// Renders a DOT file to a PNG image using GraphViz.
    /// If GraphViz is not found, prompts the user for an installation via winget.
    /// </summary>
    /// <param name="dotFilePath">Path to the DOT file to render.</param>
    /// <param name="outputImagePath">Path to the output PNG image.</param>
    /// <exception cref="FileNotFoundException">Thrown if the DOT file is not found.</exception>
    private static void RenderDotFile(string dotFilePath, string outputImagePath)
    {
        if (!File.Exists(dotFilePath))
        {
            throw new FileNotFoundException("DOT file not found.", dotFilePath);
        }

        string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
        if (!File.Exists(graphVizPath))
        {
            Console.WriteLine(
                "GraphViz is not installed on this machine. Install it or use another method to render the graph.\n"
                    + "GraphViz can be installed via winget using:\n"
                    + "winget install -e --id Graphviz.Graphviz"
            );
            Console.WriteLine(
                "Install now? (y/n) ('y' will attempt a silent installation via winget.)"
            );
            string? response = Console.ReadLine();
            if (response?.ToLower() == "y")
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
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

        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = graphVizPath,
                Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Exports the current graph to a DOT file for GraphViz usage.
    /// </summary>
    /// <param name="filePath">The path of the DOT file to be created.</param>
    /// <param name="layout">The layout algorithm to set in the DOT file (default is "dot").</param>
    private void ExportToDot(string filePath, string layout = "dot")
    {
        var dotBuilder = new StringBuilder();
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
                dotBuilder.AppendLine(
                    $"    \"{edge.SourceNode.Name}\" -- \"{edge.TargetNode.Name}\";"
                );
            }
            else if (_isDirected)
            {
                dotBuilder.AppendLine(
                    $"    \"{edge.SourceNode.Name}\" -> \"{edge.TargetNode.Name}\";"
                );
            }
        }

        dotBuilder.AppendLine("}");
        File.WriteAllText(filePath, dotBuilder.ToString());
    }

    #endregion Private Helpers
}
