using System.Diagnostics;
using System.Text;

//HACK: refactor

namespace LivinParis.Models.Maps;

/// <summary>
/// Represents a simple graph containing a set of nodes (vertices) and edges.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in each node.
/// </typeparam>
public class Graph<T>
    where T : notnull
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
        where TU : notnull
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

    //TODO: rajouter poids et diamètre du graphe
    //QUESTION: Gestion des propriétés

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
    private readonly SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> _adjacencyList;

    /// <summary>
    /// The adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
    /// the weight of the edge from <c>i</c> to <c>j</c>.
    /// </summary>
    private readonly double[,] _adjacencyMatrix;

    /// <summary>
    /// A mapping from nodes to their corresponding coordinates in the adjacency matrix.
    /// </summary>
    private readonly SortedDictionary<Node<T>, int> _correspondingCoordinates;

    /// <summary>
    /// The distance matrix computed via the Roy-Floyd-Warshall algorithm.
    /// </summary>
    private readonly double[,] _distanceMatrix;

    /// <summary>
    /// The path matrix computed via the Roy-Floyd-Warshall algorithm.
    /// </summary>
    private readonly List<Node<T>>[,] _pathMatrix;

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
    public Graph(SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> adjacencyList)
    {
        _nodes = new SortedSet<Node<T>>(adjacencyList.Keys);
        _edges = new List<Edge<T>>();
        _adjacencyList = adjacencyList;

        var _correspondingCoordinates = new SortedDictionary<Node<T>, int>();
        int counter = 0;
        foreach (var node in adjacencyList.Keys)
        {
            _correspondingCoordinates[node] = counter;
            counter++;
        }

        _adjacencyMatrix = new double[_nodes.Count, _nodes.Count];
        foreach (var i in _correspondingCoordinates.Values)
        {
            foreach (var j in _correspondingCoordinates.Values)
            {
                _adjacencyMatrix[i, j] = double.MaxValue;
            }
            _adjacencyMatrix[i, i] = 0.0;
        }

        foreach (var kvp in adjacencyList)
        {
            foreach (var neighbor in kvp.Value)
            {
                _adjacencyMatrix[
                    _correspondingCoordinates[kvp.Key],
                    _correspondingCoordinates[neighbor.Key]
                ] = neighbor.Value;
            }
        }

        _isDirected = !CheckIfSymmetric(_adjacencyMatrix);

        foreach (var i in _correspondingCoordinates.Values)
        {
            foreach (var j in _correspondingCoordinates.Values)
            {
                double weight = _adjacencyMatrix[i, j];
                if (Math.Abs(weight - double.MaxValue) > 1e-9 && Math.Abs(weight) > 1e-9)
                {
                    bool isDirected = Math.Abs(weight - _adjacencyMatrix[j, i]) > 1e-9;

                    if (isDirected || i < j)
                    {
                        var color = "#000000";
                        var source = _correspondingCoordinates.First(kvp => kvp.Value == i).Key;
                        var target = _correspondingCoordinates.First(kvp => kvp.Value == j).Key;

                        if (
                            source.VisualizationParameters.Color
                                == target.VisualizationParameters.Color
                            && source.Data.ToString() != target.Data.ToString()
                        )
                        {
                            color = source.VisualizationParameters.Color;
                        }
                        _edges.Add(new Edge<T>(source, target, weight, isDirected, color));
                    }
                }
            }
        }

        //_pathMatrix = RoyFloydWarshall();
        _order = _nodes.Count;
        _size = _edges.Count + _edges.Count(e => !e.IsDirected);
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
        _adjacencyList = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();
        _adjacencyMatrix = adjacencyMatrix;
        _correspondingCoordinates = new SortedDictionary<Node<T>, int>();

        int n = _adjacencyMatrix.GetLength(0);

        for (int i = 0; i < n; i++)
        {
            var node = Node<T>.GetNode(i);
            _nodes.Add(node);
            _adjacencyList[node] = new SortedDictionary<Node<T>, double>();
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
                    bool isDirected =
                        Math.Abs(weight - _adjacencyMatrix[target.Id, source.Id]) > 1e-9;

                    if (isDirected || source.Id < target.Id)
                    {
                        var color = "#000000";
                        if (
                            source.VisualizationParameters.Color
                                == target.VisualizationParameters.Color
                            && source.Data.ToString() != target.Data.ToString()
                        )
                        {
                            color = source.VisualizationParameters.Color;
                        }
                        _edges.Add(new Edge<T>(source, target, weight, isDirected, color));
                    }

                    _adjacencyList[source].Add(target, weight);

                    if (!_isDirected)
                    {
                        _adjacencyList[target].Add(source, weight);
                    }
                }
            }
        }

        foreach (var node in _nodes)
        {
            _correspondingCoordinates.Add(node, node.Id);
        }

        //_pathMatrix = RoyFloydWarshall();
        _order = _nodes.Count;
        _size = _edges.Count + _edges.Count(e => !e.IsDirected);
        int orientedFactor = _isDirected ? 1 : 2;
        _density = (double)_size * orientedFactor / (_order * (_order - 1));
        _isWeighted = _edges.Any(edge => Math.Abs(edge.Weight - 1.0) > 1e-9);
        _isConnected = BFS(_nodes.First().Id).Count == _nodes.Count;
    }

    #endregion Constructors

    #region Properties

    // /// <summary>
    // /// Gets the set of all nodes in this graph.
    // /// </summary>
    // public SortedSet<Node<T>> Nodes
    // {
    //     get { return _nodes; }
    // }

    // /// <summary>
    // /// Gets the collection of edges in this graph.
    // /// </summary>
    // public List<Edge<T>> Edges
    // {
    //     get { return _edges; }
    // }

    // /// <summary>
    // /// Gets the adjacency list representing this graph.
    // /// </summary>
    // public SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> AdjacencyList
    // {
    //     get { return _adjacencyList; }
    // }

    // /// <summary>
    // /// Gets the adjacency matrix for this graph,
    // /// where <see cref="double.MaxValue"/> indicates no edge.
    // /// </summary>
    // public double[,] AdjacencyMatrix
    // {
    //     get { return _adjacencyMatrix; }
    // }

    // /// <summary>
    // /// Gets the distance matrix for all pairs of nodes,
    // /// computed by the Roy-Floyd-Warshall algorithm.
    // /// </summary>
    // public double[,] DistanceMatrix
    // {
    //     get { return _distanceMatrix; }
    // }

    // /// <summary>
    // /// Gets the number of nodes (the order of the graph).
    // /// </summary>
    // public int Order
    // {
    //     get { return _order; }
    // }

    // /// <summary>
    // /// Gets the number of edges (the size of the graph).
    // /// </summary>
    // public int Size
    // {
    //     get { return _size; }
    // }

    // /// <summary>
    // /// Gets the density of the graph.
    // /// For directed graphs, density = E / (V*(V-1)).
    // /// For undirected graphs, density = (2*E) / (V*(V-1)).
    // /// </summary>
    // public double Density
    // {
    //     get { return _density; }
    // }

    // /// <summary>
    // /// Indicates whether this graph is directed.
    // /// </summary>
    // public bool IsDirected
    // {
    //     get { return _isDirected; }
    // }

    // /// <summary>
    // /// Indicates whether this graph is weighted,
    // /// i.e., if any edge has a weight different from 1.0.
    // /// </summary>
    // public bool IsWeighted
    // {
    //     get { return _isWeighted; }
    // }

    // /// <summary>
    // /// Indicates whether this graph is connected,
    // /// tested by a BFS from the first node.
    // /// </summary>
    // /// <remarks>
    // /// For directed graphs, checks if all nodes are reachable
    // /// in one direction from the first node in <see cref="_nodes"/>.
    // /// </remarks>
    // public bool IsConnected
    // {
    //     get { return _isConnected; }
    // }

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

    #region Public Methods - SCC Detection

    public List<Graph<T>> GetStronglyConnectedComponents()
    {
        var result = new List<Graph<T>>();
        var visited = new HashSet<Node<T>>();

        while (visited.Count < _order)
        {
            var startNode = _nodes.Where(n => !visited.Contains(n)).First();
            var adjacencyList = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();
            var successors = DFS(startNode, false);
            var predecessor = DFS(startNode, true);

            foreach (var node in successors.Where(n => predecessor.Contains(n)))
            {
                visited.Add(node);
                adjacencyList[node] = new SortedDictionary<Node<T>, double>();
            }

            foreach (var edge in _edges)
            {
                if (
                    adjacencyList.Keys.Contains(edge.SourceNode)
                    && adjacencyList.Keys.Contains(edge.TargetNode)
                )
                {
                    adjacencyList[edge.SourceNode].Add(edge.TargetNode, edge.Weight);
                    if (!edge.IsDirected)
                    {
                        adjacencyList[edge.TargetNode].Add(edge.SourceNode, edge.Weight);
                    }
                }
            }

            var scc = new Graph<T>(adjacencyList);
            result.Add(scc);
        }

        return result;
    }

    #endregion Public Methods - SCC Detection

    #region Public Methods - Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node,
    /// which can be an ID (<c>int</c>), a <see cref="Node{T}"/>, or data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the node is not found in the adjacency list.
    /// </exception>
    public List<Node<T>> BFS<TU>(TU start)
        where TU : notnull
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
                foreach (var neighbor in neighbors.Keys)
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
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/>;
    /// can be <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/>.
    /// </typeparam>
    /// <param name="start">The starting node or identifier.</param>
    /// <param name="inverted">If <c>true</c>, traverses the graph in reverse order.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the node is not found in the adjacency list.
    /// </exception>
    public List<Node<T>> DFS<TU>(TU start, bool inverted = false)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!_adjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var visited = new HashSet<Node<T>>();
        var result = new List<Node<T>>();

        DFSUtil(startNode, visited, result, inverted);
        return result;
    }

    #endregion Public Methods - Traversals

    #region Public Methods - Pathfinding

    //TODO: Méthode graph.Dist(Node<T> a, Node<T> b)

    //TODO: Méthode de visualisation des chemins, sous graphe. Renvoi un sous graphe

    /// <summary>
    /// Performs Dijkstra's algorithm from the specified start node,
    /// returning the set of paths to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
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
    public SortedDictionary<Node<T>, List<Node<T>>> Dijkstra<TU>(TU start)
        where TU : notnull
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
                foreach (var neighbor in neighbors.Keys.Where(n => !visited.Contains(n)))
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
    /// <typeparam name="TU">
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
    public SortedDictionary<Node<T>, List<Node<T>>> BellmanFord<TU>(TU start)
        where TU : notnull
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

                if (!edge.IsDirected && result[target].Distance + weight < result[source].Distance)
                {
                    result[source].Distance = result[target].Distance + weight;
                    result[source].Predecessor = target;
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

    //TODO: Distance + chemins. Pas d'attribut distance_matrix mais méthode de recherche de pcc dans pathfinding. Lzay computation ??

    /// <summary>
    /// Computes the all-pairs shortest path distances using the Roy-Floyd-Warshall algorithm.
    /// </summary>
    /// <returns>
    /// A 2D array of distances, where <c>distance[i, j]</c> is the shortest path cost from i to j.
    /// </returns>
    public List<Node<T>>[,] RoyFloydWarshall()
    {
        int n = _nodes.Count;
        var distanceMatrix = new double[n, n];
        var pathMatrix = new List<Node<T>>[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distanceMatrix[i, j] = _adjacencyMatrix[i, j];
                pathMatrix[i, j] = new List<Node<T>>();
                if (Math.Abs(distanceMatrix[i, j] - double.MaxValue) > 1e-9 && i != j)
                {
                    pathMatrix[i, j].Add(Node<T>.GetNode(i));
                    pathMatrix[i, j].Add(Node<T>.GetNode(j));
                }
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
                        pathMatrix[i, j] = new List<Node<T>>(pathMatrix[i, k]);
                        pathMatrix[i, j].AddRange(pathMatrix[k, j]);
                        pathMatrix[i, j].Add(Node<T>.GetNode(j));
                    }
                }
            }
        }

        return pathMatrix;
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
    /// <param name="layout">The GraphViz layout to use (e.g. "dot", "fdp", "neato", ...).</param>
    /// <param name="shape">The shape of the nodes (e.g. "circle", "square", "triangle", ...).</param>
    public void DisplayGraph(
        string outputImageName = "graph",
        string layout = "neato",
        string shape = "point"
    )
    {
        string dotFilePath = $"{outputImageName}.dot";
        string outputImagePath =
            $"data/output/{outputImageName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

        ExportToDot(dotFilePath, layout, shape);
        RenderDotFile(dotFilePath, outputImagePath);
        File.Delete(dotFilePath);
    }

    #endregion Public Methods - Drawing

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
            foreach (var neighbor in neighbors.Keys)
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
            foreach (var neighbor in neighbors.Keys)
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
    /// <param name="inverted">If <c>true</c>, traverses the graph in reverse order.</param>
    private void DFSUtil(
        Node<T> node,
        HashSet<Node<T>> visited,
        List<Node<T>> result,
        bool inverted
    )
    {
        visited.Add(node);
        result.Add(node);

        if (inverted)
        {
            foreach (
                var predecessor in _adjacencyList
                    .Where(kvp => kvp.Value.ContainsKey(node))
                    .Select(kvp => kvp.Key)
            )
            {
                if (!visited.Contains(predecessor))
                {
                    DFSUtil(predecessor, visited, result, inverted);
                }
            }
        }
        else
        {
            if (_adjacencyList.TryGetValue(node, out var neighbors))
            {
                foreach (var neighbor in neighbors.Keys)
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFSUtil(neighbor, visited, result, inverted);
                    }
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
    private static SortedDictionary<Node<T>, List<Node<T>>> BuildPaths(
        SortedDictionary<Node<T>, PathfindingResult<T>> results
    )
    {
        var paths = new SortedDictionary<Node<T>, List<Node<T>>>();

        foreach (var node in results.Keys)
        {
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
    /// <param name="layout">The GraphViz layout algorithm (e.g., "dot", "fdp", "neato", ...).</param>
    /// <param name="shape">The shape of the nodes (e.g., "circle", "rectangle", "diamond", ...).</param>
    private void ExportToDot(string filePath, string layout, string shape)
    {
        var dotBuilder = new StringBuilder();
        var clusters = new List<string>();

        dotBuilder.AppendLine(_isDirected ? "digraph G {" : "graph G {");
        dotBuilder.AppendLine($"    layout={layout};");
        dotBuilder.AppendLine("    ratio=0.6438356164;");
        dotBuilder.AppendLine($"    node [shape={shape}, fontsize=\"10\"];");

        foreach (var node in _nodes)
        {
            dotBuilder.Append($"    \"{node.Data}\" [{node.VisualizationParameters}");
            if (clusters.Contains(node.VisualizationParameters.Label))
            {
                dotBuilder.AppendLine(", penwidth=4");
            }
            else
            {
                dotBuilder.Append($", xlabel=\"{node.VisualizationParameters.Label}\"");
                clusters.Add(node.VisualizationParameters.Label);
            }
            dotBuilder.AppendLine("];");
        }

        dotBuilder.AppendLine();

        foreach (var edge in _edges.Where(e => e.RGBColor != "#000000"))
        {
            if (!_isDirected && edge.SourceNode.Id > edge.TargetNode.Id)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -- \"{edge.TargetNode.Data}\"");
            }
            else if (_isDirected)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -> \"{edge.TargetNode.Data}\"");
                if (!edge.IsDirected)
                {
                    dotBuilder.Append(" [dir=both]");
                }
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
    /// <typeparam name="TU">The type of the <paramref name="start"/> parameter.</typeparam>
    /// <param name="start">An integer ID, a node, or a data value.</param>
    /// <returns>The corresponding node object in this graph.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is an unsupported type or the resulting node is invalid.
    /// </exception>
    private static Node<T> ResolveNode<TU>(TU start)
        where TU : notnull
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
