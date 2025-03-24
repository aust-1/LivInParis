using System.Diagnostics;
using System.Text;

namespace LivinParis.Models;

/// <summary>
/// Represents a simple graph containing a set of nodes (vertices) and edges.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in each node.
/// </typeparam>
public class Graph<T>
{
    #region Nested Types

    /// <summary>
    /// Encapsulates pathfinding results for a particular node,
    /// including the distance (cost) and its predecessor.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of data stored in the node, mirroring <see cref="Graph{T}"/>.
    /// </typeparam>
    public class PathfindingResult<TU>
    {
        /// <summary>
        /// Gets or sets the distance (cost) to reach this node from the source.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets the predecessor node on the path to this node.
        /// </summary>
        public Node<TU>? Predecessor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathfindingResult{TU}"/> class,
        /// specifying the distance and predecessor.
        /// </summary>
        /// <param name="distance">The initial distance to this node.</param>
        /// <param name="predecessor">The node's predecessor in the shortest path tree.</param>
        public PathfindingResult(double distance, Node<TU>? predecessor)
        {
            Distance = distance;
            Predecessor = predecessor;
        }
    }

    #endregion Nested Types

    #region Fields

    /// <summary>
    /// The sorted set of all nodes in this graph.
    /// </summary>
    private readonly SortedSet<Node<T>> _nodes;

    /// <summary>
    /// The list of edges in this graph.
    /// </summary>
    private readonly List<Edge<T>> _edges;

    /// <summary>
    /// The adjacency list: maps each node to a set of adjacent nodes.
    /// </summary>
    private readonly SortedDictionary<Node<T>, SortedSet<Node<T>>> _adjacencyList;

    /// <summary>
    /// The adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
    /// the weight of the edge from <c>i</c> to <c>j</c>.
    /// </summary>
    private readonly double[,] _adjacencyMatrix;

    /// <summary>
    /// The distance matrix computed via the Roy-Floyd-Warshall algorithm.
    /// </summary>
    private readonly double[,] _distanceMatrix;

    /// <summary>
    /// The order of the graph (number of nodes).
    /// </summary>
    private readonly int _order;

    /// <summary>
    /// The size of the graph (number of edges).
    /// </summary>
    private readonly int _size;

    /// <summary>
    /// The density of the graph.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For an undirected graph, density = (2 * E) / (V * (V - 1)).
    /// </para>
    /// <para>
    /// For a directed graph, density = E / (V * (V - 1)).
    /// </para>
    /// </remarks>
    private readonly double _density;

    /// <summary>
    /// Indicates if the graph is directed (<c>true</c>) or undirected (<c>false</c>).
    /// </summary>
    private readonly bool _isDirected;

    /// <summary>
    /// Indicates if the graph is weighted (<c>true</c>) or unweighted (<c>false</c>).
    /// </summary>
    private readonly bool _isWeighted;

    /// <summary>
    /// Indicates if the graph is connected. This is determined by performing a BFS
    /// from the first node (for directed graphs, it checks if all nodes are reachable
    /// in a single direction).
    /// </summary>
    private readonly bool _isConnected;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph{T}"/> class
    /// using a given adjacency list. Determines directedness by testing
    /// whether the generated adjacency matrix is symmetric.
    /// </summary>
    /// <param name="adjacencyList">
    /// A dictionary mapping each node to its set of adjacent nodes.
    /// </param>
    /// <remarks>
    /// This constructor treats all edges as if they have weight = 1.0.
    /// </remarks>
    public Graph(SortedDictionary<Node<T>, SortedSet<Node<T>>> adjacencyList)
    {
        _nodes = new SortedSet<Node<T>>(adjacencyList.Keys);
        _edges = new List<Edge<T>>();
        _adjacencyList = adjacencyList;

        _adjacencyMatrix = new double[_nodes.Count, _nodes.Count];
        for (int i = 0; i < _nodes.Count; i++)
        {
            for (int j = 0; j < _nodes.Count; j++)
            {
                _adjacencyMatrix[i, j] = double.MaxValue;
            }
            _adjacencyMatrix[i, i] = 0.0;
        }

        foreach (var kvp in adjacencyList)
        {
            var source = kvp.Key;
            foreach (var neighbor in kvp.Value)
            {
                _edges.Add(new Edge<T>(source, neighbor, 1.0, true));
                _adjacencyMatrix[source.Id, neighbor.Id] = 1.0;
            }
        }

        _isDirected = !CheckIfSymmetric(_adjacencyMatrix);
        _distanceMatrix = RoyFloydWarshall();
        _order = _nodes.Count;
        _size = _edges.Count;
        int orientedFactor = _isDirected ? 1 : 2;
        _density = (double)_size * orientedFactor / (_order * (_order - 1));
        _isWeighted = _edges.Any(edge => Math.Abs(edge.Weight - 1.0) > 1e-9);
        _isConnected = BFS(_nodes.First().Id).Count == _nodes.Count;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph{T}"/> class
    /// from a given adjacency matrix. Determines directedness by checking
    /// whether the matrix is symmetric.
    /// </summary>
    /// <param name="adjacencyMatrix">
    /// A square 2D array representing adjacency weights from i to j.
    /// <see cref="double.MaxValue"/> indicates no edge.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the adjacency matrix is not square.
    /// </exception>
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

        int n = _adjacencyMatrix.GetLength(0);

        for (int i = 0; i < n; i++)
        {
            var node = Node<T>.GetNode(i);
            _nodes.Add(node);
            _adjacencyList[node] = new SortedSet<Node<T>>();
        }

        foreach (var source in _nodes)
        {
            foreach (var target in _nodes)
            {
                if (!_isDirected && source.Id > target.Id)
                {
                    continue;
                }

                double weight = _adjacencyMatrix[source.Id, target.Id];
                if (Math.Abs(weight - double.MaxValue) > 1e-9 && Math.Abs(weight) > 1e-9)
                {
                    _edges.Add(new Edge<T>(source, target, weight, _isDirected));
                    _adjacencyList[source].Add(target);

                    if (!_isDirected)
                    {
                        _adjacencyList[target].Add(source);
                    }
                }
            }
        }

        _distanceMatrix = RoyFloydWarshall();
        _order = _nodes.Count;
        _size = _edges.Count;
        int orientedFactor = _isDirected ? 1 : 2;
        _density = (double)_size * orientedFactor / (_order * (_order - 1));
        _isWeighted = _edges.Any(edge => Math.Abs(edge.Weight - 1.0) > 1e-9);
        _isConnected = BFS(_nodes.First().Id).Count == _nodes.Count;
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
    /// Gets the adjacency list representing this graph.
    /// </summary>
    public SortedDictionary<Node<T>, SortedSet<Node<T>>> AdjacencyList
    {
        get { return _adjacencyList; }
    }

    /// <summary>
    /// Gets the adjacency matrix for this graph,
    /// where <see cref="double.MaxValue"/> indicates no edge.
    /// </summary>
    public double[,] AdjacencyMatrix
    {
        get { return _adjacencyMatrix; }
    }

    /// <summary>
    /// Gets the distance matrix for all pairs of nodes,
    /// computed by the Roy-Floyd-Warshall algorithm.
    /// </summary>
    public double[,] DistanceMatrix
    {
        get { return _distanceMatrix; }
    }

    /// <summary>
    /// Gets the number of nodes (the order of the graph).
    /// </summary>
    public int Order
    {
        get { return _order; }
    }

    /// <summary>
    /// Gets the number of edges (the size of the graph).
    /// </summary>
    public int Size
    {
        get { return _size; }
    }

    /// <summary>
    /// Gets the density of the graph.
    /// For directed graphs, density = E / (V*(V-1)).
    /// For undirected graphs, density = (2*E) / (V*(V-1)).
    /// </summary>
    public double Density
    {
        get { return _density; }
    }

    /// <summary>
    /// Indicates whether this graph is directed.
    /// </summary>
    public bool IsDirected
    {
        get { return _isDirected; }
    }

    /// <summary>
    /// Indicates whether this graph is weighted,
    /// i.e., if any edge has a weight different from 1.0.
    /// </summary>
    public bool IsWeighted
    {
        get { return _isWeighted; }
    }

    /// <summary>
    /// Indicates whether this graph is connected,
    /// tested by a BFS from the first node.
    /// </summary>
    /// <remarks>
    /// For directed graphs, checks if all nodes are reachable
    /// in one direction from the first node in <see cref="_nodes"/>.
    /// </remarks>
    public bool IsConnected
    {
        get { return _isConnected; }
    }

    #endregion Properties

    #region Public Methods - Cycle Detection

    /// <summary>
    /// Searches for any cycle in the graph.
    /// Works for both directed and undirected graphs.
    /// Optionally ignores the immediate parent for undirected simple cycles.
    /// </summary>
    /// <param name="simpleCycle">
    /// If <c>true</c> and the graph is undirected, the method will ignore
    /// an edge back to the immediate parent. Defaults to <c>false</c>.
    /// </param>
    /// <returns>
    /// A string describing the detected cycle (IDs and data) if found; otherwise <c>null</c>.
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

    #endregion Public Methods - Cycle Detection

    #region Public Methods - Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node,
    /// which can be an ID (<c>int</c>), a <see cref="Node{T}"/>, or data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="U">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the node is not found in the adjacency list.
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

            if (_adjacencyList.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Performs a recursive Depth-First Search (DFS) from the specified node,
    /// which can be an ID (<c>int</c>), a <see cref="Node{T}"/>, or data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="U">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the node is not found in the adjacency list.
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

    #endregion Public Methods - Traversals

    #region Public Methods - Pathfinding

    /// <summary>
    /// Performs Dijkstra's algorithm from the specified start node,
    /// returning the set of paths to each reachable node.
    /// </summary>
    /// <typeparam name="U">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <returns>
    /// A dictionary mapping each node to a list of nodes representing the path
    /// from <paramref name="start"/> to that node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the node is not in the graph.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> Dijkstra<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        var visited = new HashSet<Node<T>>();

        foreach (var node in _nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        while (visited.Count < _nodes.Count)
        {
            var current = result
                .Where(kvp => !visited.Contains(kvp.Key))
                .OrderBy(kvp => kvp.Value.Distance)
                .FirstOrDefault()
                .Key;

            if (current == null || double.IsPositiveInfinity(result[current].Distance))
            {
                break;
            }

            visited.Add(current);

            if (_adjacencyList.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors.Where(n => !visited.Contains(n)))
                {
                    var newDistance =
                        result[current].Distance + _adjacencyMatrix[current.Id, neighbor.Id];

                    if (newDistance < result[neighbor].Distance)
                    {
                        result[neighbor].Distance = newDistance;
                        result[neighbor].Predecessor = current;
                    }
                }
            }
        }

        return BuildPaths(result);
    }

    /// <summary>
    /// Performs the Bellman-Ford algorithm from the specified start node,
    /// returning the set of paths to each reachable node.
    /// Detects negative-weight cycles.
    /// </summary>
    /// <typeparam name="U">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c>, <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <returns>
    /// A dictionary mapping each node to a list of nodes representing the path
    /// from <paramref name="start"/> to that node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or not present in the graph.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the graph contains a negative-weight cycle.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> BellmanFord<U>(U start)
        where U : notnull
    {
        var startNode = ResolveNode(start);

        if (!_nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        foreach (var node in _nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        bool relaxed = false;
        for (int i = 0; i < _nodes.Count; i++)
        {
            relaxed = false;
            foreach (var edge in _edges)
            {
                var source = edge.SourceNode;
                var target = edge.TargetNode;
                var weight = edge.Weight;

                if (result[source].Distance + weight < result[target].Distance)
                {
                    result[target].Distance = result[source].Distance + weight;
                    result[target].Predecessor = source;
                    relaxed = true;
                }
            }

            if (!relaxed)
            {
                break;
            }
        }

        if (relaxed)
        {
            throw new InvalidOperationException("Graph contains a negative-weight cycle.");
        }

        return BuildPaths(result);
    }

    #endregion Public Methods - Pathfinding

    #region Public Methods - Drawing

    /// <summary>
    /// Exports the graph to a DOT file, then calls GraphViz to generate a PNG image,
    /// finally deleting the DOT file.
    /// </summary>
    /// <param name="outputImageName">
    /// The base file name (without extension) for the output. A timestamp is appended to avoid overwrites.
    /// </param>
    /// <param name="layout">The GraphViz layout to use (e.g. "dot", "neato").</param>
    public void DisplayGraph(string outputImageName = "graph", string layout = "dot")
    {
        string dotFilePath = $"{outputImageName}.dot";
        string outputImagePath =
            $"data/output/{outputImageName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

        ExportToDot(dotFilePath, layout);
        RenderDotFile(dotFilePath, outputImagePath);
        File.Delete(dotFilePath);
    }

    #endregion Public Methods - Drawing

    #region Private Helpers - Roy-Floyd-Warshall

    /// <summary>
    /// Computes the all-pairs shortest path distances using the Roy-Floyd-Warshall algorithm.
    /// </summary>
    /// <returns>
    /// A 2D array of distances, where <c>distance[i, j]</c> is the shortest path cost from i to j.
    /// </returns>
    private double[,] RoyFloydWarshall()
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
                    double pathViaK = distanceMatrix[i, k] + distanceMatrix[k, j];
                    if (pathViaK < distanceMatrix[i, j])
                    {
                        distanceMatrix[i, j] = pathViaK;
                    }
                }
            }
        }

        return distanceMatrix;
    }

    #endregion Private Helpers - Roy-Floyd-Warshall

    #region Private Helpers - Cycle Detection

    /// <summary>
    /// Attempts to find a cycle in a directed graph using DFS and a recursion stack.
    /// </summary>
    /// <param name="current">The current node being explored.</param>
    /// <param name="visited">A set of visited nodes.</param>
    /// <param name="recStack">A recursion stack storing the current path.</param>
    /// <param name="parentMap">A map to reconstruct the cycle path if found.</param>
    /// <param name="cycle">
    /// Outputs the list of nodes forming a cycle, or <c>null</c> if no cycle is found.
    /// </param>
    /// <param name="simpleCycle">
    /// <c>true</c> in undirected graphs to ignore edges back to the immediate parent.
    /// Not strictly relevant for directed graphs.
    /// </param>
    /// <returns><c>true</c> if a cycle is detected; otherwise <c>false</c>.</returns>
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
    /// </summary>
    /// <param name="current">The current node being explored.</param>
    /// <param name="visited">A set of visited nodes.</param>
    /// <param name="parentMap">A map to reconstruct the cycle if found.</param>
    /// <param name="parent">The node's parent in the DFS tree (if any).</param>
    /// <param name="cycle">
    /// Outputs the list of nodes forming a cycle, or <c>null</c> if no cycle is found.
    /// </param>
    /// <param name="simpleCycle">
    /// If <c>true</c>, ignore edges back to the immediate parent to detect only "simple" cycles.
    /// </param>
    /// <returns><c>true</c> if a cycle is detected; otherwise <c>false</c>.</returns>
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
    /// Reconstructs a cycle path from two meeting nodes in a DFS recursion stack.
    /// </summary>
    /// <param name="current">The node that discovered the cycle.</param>
    /// <param name="neighbor">The previously visited node in the cycle.</param>
    /// <param name="parentMap">A dictionary linking each node to its parent.</param>
    /// <returns>A list of nodes forming the cycle.</returns>
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
    /// Builds a string representation of a cycle, listing node IDs and data.
    /// </summary>
    /// <param name="cycle">The nodes forming the cycle.</param>
    /// <returns>A string describing the cycle's IDs and data.</returns>
    private static string CycleToString(List<Node<T>> cycle)
    {
        var sbId = new StringBuilder();
        var sbData = new StringBuilder();

        sbId.Append("Cycle by Id: <");
        sbData.Append("\nCycle by Data: ");

        foreach (var node in cycle)
        {
            sbId.Append(node.Id).Append(", ");
            sbData.Append(node.Data).Append(" -> ");
        }

        sbId.Append(cycle[0].Id).Append('>');
        sbData.Append(cycle[0].Data);

        return sbId.Append(sbData).ToString();
    }

    #endregion Private Helpers - Cycle Detection

    #region Private Helpers - DFS

    /// <summary>
    /// A helper method for performing a recursive DFS from a specified node.
    /// </summary>
    /// <param name="node">The node where DFS is currently happening.</param>
    /// <param name="visited">A set of nodes that have been visited already.</param>
    /// <param name="result">The list where visited nodes are accumulated.</param>
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

    #endregion Private Helpers - DFS

    #region Private Helpers - Pathfinding

    /// <summary>
    /// Builds the resulting path list for each node given a dictionary
    /// of <see cref="PathfindingResult{T}"/>.
    /// </summary>
    /// <param name="results">
    /// A dictionary from each node to its pathfinding result
    /// (distance and predecessor).
    /// </param>
    /// <returns>
    /// A dictionary mapping each node to a list representing its path
    /// from the start node to that node.
    /// </returns>
    private SortedDictionary<Node<T>, List<Node<T>>> BuildPaths(
        SortedDictionary<Node<T>, PathfindingResult<T>> results
    )
    {
        var paths = new SortedDictionary<Node<T>, List<Node<T>>>();

        foreach (var kvp in results)
        {
            var node = kvp.Key;
            var path = new List<Node<T>>();
            var current = node;

            while (current != null)
            {
                path.Add(current);
                current = results[current].Predecessor;
            }
            path.Reverse();
            paths[node] = path;
        }

        return paths;
    }

    #endregion Private Helpers - Pathfinding

    #region Private Helpers - GraphViz

    /// <summary>
    /// Renders a DOT file into a PNG image using GraphViz,
    /// installing GraphViz via winget if not found.
    /// </summary>
    /// <param name="dotFilePath">The path to the DOT file.</param>
    /// <param name="outputImagePath">The path of the resulting PNG image.</param>
    /// <exception cref="FileNotFoundException">Thrown if the DOT file is missing.</exception>
    private static void RenderDotFile(string dotFilePath, string outputImagePath)
    {
        if (!File.Exists(dotFilePath))
        {
            throw new FileNotFoundException("DOT file not found.", dotFilePath);
        }

        const string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
        if (!File.Exists(graphVizPath))
        {
            Console.WriteLine(
                "GraphViz is not installed on this machine. Please install it or use another method.\n"
                    + "GraphViz can be installed via winget:\n"
                    + "  winget install -e --id Graphviz.Graphviz"
            );
            Console.WriteLine(
                "Install now? (y/n) ('y' will attempt a silent installation via winget.)"
            );

            string? response = Console.ReadLine();
            if (response?.ToLower() == "y")
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using var installProcess = new Process { StartInfo = psi };
                installProcess.Start();
                string output = installProcess.StandardOutput.ReadToEnd();
                string error = installProcess.StandardError.ReadToEnd();
                installProcess.WaitForExit();

                Console.WriteLine("Output:\n" + output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error:\n" + error);
                }
            }
            else
            {
                return;
            }
        }

        using var process = new Process
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
    /// Exports the current graph to a DOT file for visualization with GraphViz.
    /// </summary>
    /// <param name="filePath">The path to the DOT file.</param>
    /// <param name="layout">The GraphViz layout algorithm (e.g., "dot" or "neato").</param>
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
            if (!_isDirected && edge.SourceNode.Id > edge.TargetNode.Id)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -- \"{edge.TargetNode.Data}\"");
            }
            else if (_isDirected)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -> \"{edge.TargetNode.Data}\"");
            }

            if (_isWeighted)
            {
                dotBuilder.Append($" [label=\"{edge.Weight}\"]");
            }
            dotBuilder.AppendLine($" [color=\"{edge.RGBColor}\"];");
        }

        dotBuilder.AppendLine("}");
        File.WriteAllText(filePath, dotBuilder.ToString());
    }

    #endregion Private Helpers - GraphViz

    #region Private Helpers - Node Resolution

    /// <summary>
    /// Converts the given <paramref name="start"/> object into a <see cref="Node{T}"/>.
    /// Supported types are:
    /// - <see cref="Node{T}"/> (node object),
    /// - <c>int</c> (node ID),
    /// - <typeparamref name="T"/> (node data).
    /// </summary>
    /// <typeparam name="U">The type of the <paramref name="start"/> parameter.</typeparam>
    /// <param name="start">An integer ID, a node, or a data value.</param>
    /// <returns>The corresponding node object in this graph.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is an unsupported type or the resulting node is invalid.
    /// </exception>
    private static Node<T> ResolveNode<U>(U start)
        where U : notnull
    {
        return start switch
        {
            Node<T> nodeObj => nodeObj,
            int id => Node<T>.GetNode(id),
            T data => Node<T>.GetOrCreateNode(data),
            _ => throw new ArgumentException(
                "Unsupported type for node resolution. Must be Node<T>, int, or T."
            ),
        };
    }

    #endregion Private Helpers - Node Resolution

    #region Private Helpers - Symmetry Check

    /// <summary>
    /// Checks whether the given adjacency matrix is symmetric,
    /// implying an undirected graph.
    /// </summary>
    /// <param name="matrix">The adjacency matrix to check.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="matrix"/> is symmetric; otherwise <c>false</c>.
    /// </returns>
    private static bool CheckIfSymmetric(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(matrix[i, j] - matrix[j, i]) > 1e-9)
                {
                    return false;
                }
            }
        }
        return true;
    }

    #endregion Private Helpers - Symmetry Check
}
