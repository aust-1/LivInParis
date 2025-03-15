using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Karate.Models;

/// <summary>
/// Represents a simple graph containing a set of nodes (vertices) and edges.
/// </summary>
public class Graph<T>
{
    #region Fields

    /// <summary>
    /// A sorted set of all nodes in this graph.
    /// </summary>
    private readonly SortedSet<Node<T>> _nodes;

    /// <summary>
    /// A list of all edges in this graph.
    /// </summary>
    private readonly List<Edge<T>> _edges;

    /// <summary>
    /// Indicates whether the graph is directed. If <c>false</c>, the graph is undirected.
    /// </summary>
    private readonly bool _isDirected;

    /// <summary>
    /// Adjacency list mapping each node to the set of its adjacent nodes.
    /// </summary>
    private readonly SortedDictionary<Node<T>, SortedSet<Node<T>>> _adjacencyList;

    /// <summary>
    /// Adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
    /// the presence (or weight) of an edge from <c>i</c> to <c>j</c>.
    /// </summary>
    private double[,] _adjacencyMatrix;

    /// <summary>
    /// Distance matrix for the graph, computed using Floyd-Warshall algorithm.
    /// </summary>
    private double[,]? _distanceMatrix;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph"/> class from a given adjacency list.
    /// Automatically determines whether the graph is directed based on the symmetry of the adjacency matrix.
    /// </summary>
    /// <param name="adjacencyList">A dictionary mapping each node to its set of adjacent nodes.</param>
    /// <remarks>
    /// This constructor infers <see cref="_isDirected"/> by checking if the adjacency matrix is symmetric.
    /// If it is not symmetric, the graph is assumed to be directed.
    /// </remarks>
    public Graph(SortedDictionary<Node<T>, SortedSet<Node<T>>> adjacencyList)
    {
        _nodes = new SortedSet<Node<T>>(adjacencyList.Keys);
        _edges = new List<Edge<T>>();
        _adjacencyList = adjacencyList;
        _adjacencyMatrix = new double[_nodes.Count, _nodes.Count];

        foreach (var kvp in adjacencyList)
        {
            var source = kvp.Key;
            foreach (var neighbor in kvp.Value)
            {
                _edges.Add(new Edge<T>(source, neighbor));
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
        _nodes = new SortedSet<Node<T>>();
        _edges = new List<Edge<T>>();
        _adjacencyList = new SortedDictionary<Node<T>, SortedSet<Node<T>>>();
        _adjacencyMatrix = adjacencyMatrix;

        int n = adjacencyMatrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            var node = Node<T>.GetNode(i);
            _nodes.Add(node);
        }

        foreach (var source in _nodes)
        {
            foreach (var target in _nodes)
            {
                double weight = adjacencyMatrix[source.Id, target.Id];
                if (!Equals(weight, 0.0))
                {
                    var edge = new Edge<T>(source, target, weight, _isDirected);

                    if (_isDirected && _edges.Contains(edge))
                    {
                        continue;
                    }

                    _edges.Add(edge);

                    if (!_adjacencyList.ContainsKey(edge.SourceNode))
                    {
                        _adjacencyList[edge.SourceNode] = new SortedSet<Node<T>>();
                    }
                    if (!_adjacencyList.ContainsKey(edge.TargetNode))
                    {
                        _adjacencyList[edge.TargetNode] = new SortedSet<Node<T>>();
                    }

                    _adjacencyList[edge.SourceNode].Add(edge.TargetNode);

                    if (!_isDirected)
                    {
                        _adjacencyList[edge.TargetNode].Add(edge.SourceNode);
                    }
                }
            }
        }
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the set of all nodes in this graph.
    /// </summary>
    public SortedSet<Node<T>> Nodes
    {
        get { return _nodes; }
    }

    /// <summary>
    /// Gets the collection of edges in this graph.
    /// </summary>
    public List<Edge<T>> Edges
    {
        get { return _edges; }
    }

    /// <summary>
    /// Gets the adjacency list representing the graph.
    /// </summary>
    public SortedDictionary<Node<T>, SortedSet<Node<T>>> AdjacencyList
    {
        get { return _adjacencyList; }
    }

    /// <summary>
    /// Gets the adjacency matrix representing the graph.
    /// </summary>
    public double[,] AdjacencyMatrix
    {
        get { return _adjacencyMatrix; }
    }

    /// <summary>
    /// Gets the distance matrix for the graph, computed using the Floyd-Warshall algorithm.
    /// If not yet computed, it will be calculated on-demand.
    /// </summary>
    public double[,] DistanceMatrix
    {
        get
        {
            _distanceMatrix = RoyFloydWarshall();
            return _distanceMatrix;
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
        var visited = new HashSet<Node<T>>();
        var recStack = new HashSet<Node<T>>();
        var parentMap = new Dictionary<Node<T>, Node<T>>();

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

    #region Shortest Paths

    public SortedDictionary<Node<T>, KeyValuePair<double, Node<T>>> Dijkstra<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, KeyValuePair<double, Node<T>>>();
        var visited = new HashSet<Node<T>>();

        foreach (var node in _nodes)
        {
            result[node] = new KeyValuePair<double, Node<T>>(double.MaxValue, null);
        }

        result[startNode] = new KeyValuePair<double, Node<T>>(0, startNode);

        while (visited.Count < _nodes.Count)
        {
            var current = result
                .Where(kvp => !visited.Contains(kvp.Key))
                .OrderBy(kvp => kvp.Value.Key)
                .First()
                .Key;

            visited.Add(current);

            foreach (var neighbor in _adjacencyList[current].Where(node => !visited.Contains(node)))
            {
                var newDistance = result[current].Key + _adjacencyMatrix[current.Id, neighbor.Id];
                if (newDistance < result[neighbor].Key)
                {
                    result[neighbor] = new KeyValuePair<double, Node<T>>(newDistance, current);
                }
            }
        }

        return result;
    }

    public SortedDictionary<Node<T>, KeyValuePair<double, Node<T>>> BellmanFord<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, KeyValuePair<double, Node<T>>>();

        foreach (var node in _nodes)
        {
            result[node] = new KeyValuePair<double, Node<T>>(double.MaxValue, null);
        }

        result[startNode] = new KeyValuePair<double, Node<T>>(0, startNode);

        for (int i = 0; i < _nodes.Count - 1; i++)
        {
            foreach (var edge in _edges)
            {
                var source = edge.SourceNode;
                var target = edge.TargetNode;
                var weight = edge.Weight;

                if (result[source].Key + weight < result[target].Key)
                {
                    result[target] = new KeyValuePair<double, Node<T>>(
                        result[source].Key + weight,
                        source
                    );
                }
            }
        }

        foreach (var edge in _edges)
        {
            var source = edge.SourceNode;
            var target = edge.TargetNode;
            var weight = edge.Weight;

            if (result[source].Key + weight < result[target].Key)
            {
                throw new InvalidOperationException("Graph contains a negative-weight cycle.");
            }
        }

        return result;
    }

    public double[,] RoyFloydWarshall()
    {
        int n = _nodes.Count;
        double[,] distanceMatrix = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distanceMatrix[i, j] = _adjacencyMatrix[i, j];
            }
        }

        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (distanceMatrix[i, k] + distanceMatrix[k, j] < distanceMatrix[i, j])
                    {
                        distanceMatrix[i, j] = distanceMatrix[i, k] + distanceMatrix[k, j];
                    }
                }
            }
        }

        return distanceMatrix;
    }

    #endregion Shortest Paths

    #region Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node.
    /// Returns the visited nodes in the order they were discovered.
    /// Supports input types: <see cref="Node"/>, <see cref="int"/> (Node ID), and <see cref="T"/> (Node Data).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the start node, which can be <see cref="Node"/>, <see cref="int"/>, or <see cref="T"/>.
    /// </typeparam>
    /// <param name="start">The node or identifier from which to start BFS.</param>
    /// <returns>A list of node in the order they were visited.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the start node type is invalid or the start node is not in the graph.
    /// </exception>
    public List<Node<T>> BFS<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new List<Node<T>>();
        var queue = new Queue<Node<T>>();
        var visited = new HashSet<Node<T>>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            foreach (var neighbor in _adjacencyList[current])
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
    /// Returns the visited nodes in the order they were discovered.
    /// Supports input types: <see cref="Node"/>, <see cref="int"/> (Node ID), and <see cref="T"/> (Node Data).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the start node, which can be <see cref="Node"/>, <see cref="int"/>, or <see cref="T"/>.
    /// </typeparam>
    /// <param name="start">The node or identifier from which to start DFS.</param>
    /// <returns>A list of node in the order they were visited.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the start node type is invalid or the start node is not in the graph.
    /// </exception>
    public List<Node<T>> DFS<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var visited = new HashSet<Node<T>>();
        var result = new List<Node<T>>();

        DFSUtil(startNode, visited, result);
        return result;
    }

    #endregion Traversals

    #region Drawing

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
    /// Supported types: <see cref="Node"/>, <see cref="int"/>, <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <param name="start">The node or ID/data to convert to a <see cref="Node"/>.</param>
    /// <returns>The corresponding <see cref="Node"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the type is unsupported.</exception>
    private static Node<T> ResolveNode<U>(U start)
        where U : notnull
    {
        return start switch
        {
            Node<T> node => node,
            int id => Node<T>.GetNode(id),
            T data => Node<T>.GetOrCreateNode(data),
            _ => throw new ArgumentException(
                "Unsupported type. Must be Node, int (Id), or T (Data)."
            ),
        };
    }

    #region Cycle Detection Helpers

    /// <summary>
    /// Attempts to find a cycle in a directed graph using DFS with recursion stack tracking.
    /// </summary>
    private bool TryFindCycleDirected(
        Node<T> current,
        HashSet<Node<T>> visited,
        HashSet<Node<T>> recStack,
        Dictionary<Node<T>, Node<T>> parentMap,
        out List<Node<T>> cycle,
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
        Node<T> current,
        HashSet<Node<T>> visited,
        Dictionary<Node<T>, Node<T>> parentMap,
        Node<T>? parent,
        out List<Node<T>> cycle,
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
    private static List<Node<T>> ReconstructCycle(
        Node<T> current,
        Node<T> neighbor,
        Dictionary<Node<T>, Node<T>> parentMap
    )
    {
        var cycle = new List<Node<T>>();
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
    /// Returns a textual representation of the cycle, including IDs and Datas.
    /// </summary>
    private static string CycleToString(List<Node<T>> cycle)
    {
        StringBuilder sbId = new StringBuilder();
        StringBuilder sbName = new StringBuilder();

        sbId.Append("Cycle by Id: <");
        sbName.Append("\nCycle by Data: ");

        foreach (var node in cycle)
        {
            sbId.Append(node.Id).Append(", ");
            sbName.Append(node.Data).Append(" -> ");
        }

        sbId.Append(cycle[0].Id).Append('>');
        sbName.Append(cycle[0].Data);

        return sbId.Append(sbName).ToString();
    }

    #endregion Cycle Detection Helpers

    #region DFS Helpers

    /// <summary>
    /// Recursively visits nodes for a Depth-First Search, storing visited nodes in a result list.
    /// </summary>
    private void DFSUtil(Node<T> node, HashSet<Node<T>> visited, List<Node<T>> result)
    {
        visited.Add(node);
        result.Add(node);

        if (_adjacencyList.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
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
            dotBuilder.AppendLine($"    \"{node.Data}\";");
        }

        foreach (var edge in _edges)
        {
            if (!_isDirected && edge.SourceNode.Id.CompareTo(edge.TargetNode.Id) > 0)
            {
                dotBuilder.AppendLine(
                    $"    \"{edge.SourceNode.Data}\" -- \"{edge.TargetNode.Data}\";"
                );
            }
            else if (_isDirected)
            {
                dotBuilder.AppendLine(
                    $"    \"{edge.SourceNode.Data}\" -> \"{edge.TargetNode.Data}\";"
                );
            }
        }

        dotBuilder.AppendLine("}");
        File.WriteAllText(filePath, dotBuilder.ToString());
    }

    #endregion Private Helpers
}
