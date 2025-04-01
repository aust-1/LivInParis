using LivinParis.Models.Maps.Helpers;

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
    /// The adjacency list that maps each node to its reachable neighbors and the weights of the edges.
    /// </summary>
    private readonly SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> _adjacencyList;

    /// <summary>
    /// The adjacency matrix: a 2D array where <c>_adjacencyMatrix[i, j]</c> indicates
    /// the weight of the edge from <c>i</c> to <c>j</c>.
    /// </summary>
    private readonly double[,] _adjacencyMatrix;

    /// <summary>
    /// Maps each node to its corresponding index in the adjacency matrix.
    /// </summary>
    private readonly SortedDictionary<Node<T>, int> _nodeIndexMap;

    /// <summary>
    /// Indicates if the graph is directed (<c>true</c>) or undirected (<c>false</c>).
    /// </summary>
    private readonly bool _isDirected;

    /// <summary>
    /// Indicates if the graph is weighted (<c>true</c>) or unweighted (<c>false</c>).
    /// </summary>
    private readonly bool _isWeighted;

    /// <summary>
    /// Indicates whether the graph is connected based on a single DFS from the first node.
    /// </summary>
    private readonly bool _isConnected;

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

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Constructs a new instance of the <see cref="Graph{T}"/> class from an adjacency list, automatically inferring directedness by checking matrix symmetry.
    /// Treats all edges as having the weights specified in the dictionary. If no weight is provided, assumes weight = 1.0.
    /// </summary>
    /// <param name="adjacencyList">
    /// A dictionary mapping each node to a nested dictionary of adjacent nodes and their edge weights.
    /// </param>
    public Graph(SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> adjacencyList)
    {
        _nodes = new SortedSet<Node<T>>(adjacencyList.Keys);
        _edges = new List<Edge<T>>();
        _adjacencyList = adjacencyList;
        _order = _nodes.Count;

        _nodeIndexMap = new SortedDictionary<Node<T>, int>();
        int counter = 0;
        foreach (var node in _adjacencyList.Keys)
        {
            _nodeIndexMap[node] = counter;
            counter++;
        }

        _adjacencyMatrix = new double[_order, _order];
        for (int i = 0; i < _order; i++)
        {
            for (int j = 0; j < _order; j++)
            {
                _adjacencyMatrix[i, j] = double.MaxValue;
            }
            _adjacencyMatrix[i, i] = 0.0;
        }

        foreach (var kvp in _adjacencyList)
        {
            foreach (var neighbor in kvp.Value)
            {
                _adjacencyMatrix[_nodeIndexMap[kvp.Key], _nodeIndexMap[neighbor.Key]] =
                    neighbor.Value;
            }
        }

        _isDirected = !IsMatrixSymmetric(_adjacencyMatrix);

        BuildEdgeFromList();

        _size = _edges.Count + _edges.Count(e => !e.IsDirected);

        _density = _isDirected
            ? (double)_size / (_order * (_order - 1))
            : 2.0 * _size / (_order * (_order - 1));

        _isWeighted = _edges.Any(e => Math.Abs(e.Weight - 1.0) > 1e-9);
        _isConnected = GetStronglyConnectedComponents().Count == 1;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="Graph{T}"/> class
    // from an adjacency matrix, automatically inferring directedness by checking matrix symmetry.
    /// </summary>
    /// <param name="adjacencyMatrix">
    /// A square 2D array representing adjacency weights from i to j.
    /// <see cref="double.MaxValue"/> indicates no edge.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided matrix is not square.
    /// </exception>
    public Graph(double[,] adjacencyMatrix)
    {
        if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
        {
            throw new ArgumentException("Adjacency matrix must be square.");
        }

        _isDirected = !IsMatrixSymmetric(adjacencyMatrix);
        _nodes = new SortedSet<Node<T>>();
        _edges = new List<Edge<T>>();
        _adjacencyList = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();
        _adjacencyMatrix = adjacencyMatrix;
        _nodeIndexMap = new SortedDictionary<Node<T>, int>();
        _order = _adjacencyMatrix.GetLength(0);

        for (int i = 0; i < _order; i++)
        {
            var node = Node<T>.GetNode(i);
            _nodes.Add(node);
            _adjacencyList[node] = new SortedDictionary<Node<T>, double>();
            _nodeIndexMap[node] = i;
        }

        BuildEdgesFromMatrix();

        _size = _edges.Count + _edges.Count(e => !e.IsDirected);

        _density = _isDirected
            ? (double)_size / (_order * (_order - 1))
            : (2.0 * _size) / (_order * (_order - 1));

        _isWeighted = _edges.Any(e => Math.Abs(e.Weight - 1.0) > 1e-9);
        _isConnected = GetStronglyConnectedComponents().Count == 1;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the set of all nodes in the graph.
    /// </summary>
    public SortedSet<Node<T>> Nodes
    {
        get { return _nodes; }
    }

    /// <summary>
    /// Gets the list of all edges in the graph.
    /// </summary>
    public List<Edge<T>> Edges
    {
        get { return _edges; }
    }

    /// <summary>
    /// Gets the adjacency list representing the graph.
    /// </summary>
    public SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> AdjacencyList
    {
        get { return _adjacencyList; }
    }

    /// <summary>
    /// Gets the adjacency matrix for the graph, where <see cref="double.MaxValue"/> indicates no edge.
    /// </summary>
    public double[,] AdjacencyMatrix
    {
        get { return _adjacencyMatrix; }
    }

    /// <summary>
    /// Gets the mapping of each node to its index in the adjacency matrix.
    /// </summary>
    public SortedDictionary<Node<T>, int> NodeIndexMap
    {
        get { return _nodeIndexMap; }
    }

    /// <summary>
    /// Gets the order of the graph (number of nodes).
    /// </summary>
    public int Order
    {
        get { return _order; }
    }

    // /// <summary>
    // /// Gets the size of the graph (number of edges).
    // /// </summary>
    // public int Size
    // {
    //     get { return _size; }
    // }

    // /// <summary>
    // /// Gets the density of the graph.
    // /// For a directed graph, density = E / (V * (V - 1)).
    // /// For an undirected graph, density = (2 * E) / (V * (V - 1)).
    // /// </summary>
    // public double Density
    // {
    //     get { return _density; }
    // }

    /// <summary>
    /// Indicates whether the graph is directed.
    /// </summary>
    public bool IsDirected
    {
        get { return _isDirected; }
    }

    // /// <summary>
    // /// Indicates whether the graph is weighted (i.e., any edge has a weight different from 1.0).
    // /// </summary>
    // public bool IsWeighted
    // {
    //     get { return _isWeighted; }
    // }

    // /// <summary>
    // /// Indicates whether the graph is connected based on a single DFS from the first vertex.

    // /// <remarks>
    // /// For directed graphs, checks if the graph is strongly connected
    // /// (i.e., checks if all nodes are reachable in both directions from the first node in <see cref="_nodes"/>).
    // /// </remarks>
    // /// </summary>
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
    /// A list of nodes forming the detected cycle if found; otherwise an empty list.
    /// </returns>
    public List<Node<T>> DetectAnyCycle(bool simpleCycle = false)
    {
        return CycleDetector<T>.FindAnyCycle(this, simpleCycle);
    }

    #endregion Public Methods - Cycle Detection

    #region Public Methods - Strongly Connected Components

    /// <summary>
    /// Computes the strongly connected components (SCCs) of the graph.
    /// </summary>
    /// <returns>
    /// A list of smaller or equal graphs, each representing a strongly connected component.
    /// </returns>
    public List<Graph<T>> GetStronglyConnectedComponents()
    {
        return CycleDetector<T>.GetStronglyConnectedComponents(this);
    }

    #endregion Public Methods - Strongly Connected Components

    #region Public Methods - Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node or identifier.
    /// </summary>
    /// <typeparam name="TU">
    /// Can be int (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/> (the data in a node).
    /// </typeparam>
    /// <param name="start">The starting point for the BFS.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the corresponding node is not found.
    /// </exception>
    public List<Node<T>> PerformBreadthFirstSearch<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.BFS(this, start);
    }

    /// <summary>
    /// Performs a recursive Depth-First Search (DFS) starting from the specified node or identifier.
    /// </summary>
    /// <typeparam name="TU">
    /// Can be int (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/> (the data in a node).
    /// </typeparam>
    /// <param name="start">The starting point for the DFS.</param>
    /// <param name="inverted">If <c>true</c>, the graph traversal order is reversed.</param>
    /// <returns>A list of visited nodes in the order they are discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the corresponding node is not found.
    /// </exception>
    public List<Node<T>> PerformDepthFirstSearch<TU>(TU start, bool inverted = false)
        where TU : notnull
    {
        return GraphAlgorithms<T>.DFS(this, start, inverted);
    }

    #endregion Public Methods - Traversals

    #region Public Methods - Pathfinding

    //TODO: Méthode graph.Dist(Node<T> a, Node<T> b)

    //TODO: Méthode de visualisation des chemins, sous graphe. Renvoi un sous graphe

    /// <summary>
    /// Runs Dijkstra's algorithm from a specified node or identifier,
    /// returning the shortest path to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
    /// Can be int (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/> (the data in a node).
    /// </typeparam>
    /// <param name="start">The starting point for Dijkstra's algorithm.</param>
    /// <returns>
    /// A dictionary mapping each reachable node to a list of nodes representing the path taken
    // from <paramref name="start"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the corresponding node is not found.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> ComputeDijkstra<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.Dijkstra(this, start);
    }

    /// <summary>
    /// Runs the Bellman-Ford algorithm from a specified node or identifier.
    /// Detects negative-weight cycles if they exist.
    /// </summary>
    /// <typeparam name="TU">
    /// Can be int (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/> (the data in a node).
    /// </typeparam>
    /// <param name="start">The starting point for Bellman-Ford's algorithm.</param>
    /// <returns>
    /// A dictionary mapping each reachable node to a list of nodes representing the path taken
    // from <paramref name="start"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or if the corresponding node is not found.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a negative-weight cycle is detected.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> ComputeBellmanFordPaths<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.BellmanFord(this, start);
    }

    //TODO: Distance + chemins. Pas d'attribut distance_matrix mais méthode de recherche de pcc dans pathfinding. Lzay computation ??

    /// <summary>
    /// Computes the shortest paths between all pairs of nodes using the Floyd-Warshall algorithm.
    /// </summary>
    /// <returns>
    /// A 2D array of paths where each element contains the sequence of nodes from i to j.
    /// </returns>
    public List<Node<T>>[,] ComputeFloydWarshall()
    {
        return GraphAlgorithms<T>.RoyFloydWarshall(this);
    }

    #endregion Public Methods - Pathfinding

    #region Public Methods - Rendering

    /// <summary>
    /// Exports the graph to a DOT file, invokes GraphViz to create a PNG image, and then removes the DOT file.
    /// </summary>
    /// <param name="outputImageName">
    /// The base file name for the generated image (without extension). A timestamp is appended to avoid overwriting.
    /// </param>
    /// <param name="layout">The GraphViz layout to use (e.g. "dot", "fdp", "neato", ...).</param>
    /// <param name="shape">The shape of the nodes (e.g. "circle", "square", "triangle", ...).</param>
    public void DisplayGraph(
        string outputImageName = "graph",
        string layout = "neato",
        string nodeShape = "point"
    )
    {
        Visualization<T>.DisplayGraph(this, outputImageName, layout, nodeShape);
    }

    #endregion Public Methods - Rendering

    #region Private Methods

    /// <summary>
    /// Checks if a given matrix is symmetric (within a small epsilon), implying an undirected graph.
    /// </summary>
    /// <param name="matrix">The matrix to verify for symmetry.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="matrix"/> is symmetric; otherwise <c>false</c>.
    /// </returns>
    private static bool IsMatrixSymmetric(double[,] matrix)
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

    /// <summary>
    /// Builds edges from the adjacency list and populates the _edge list.
    /// </summary>
    private void BuildEdgeFromList()
    {
        foreach (var i in _nodeIndexMap.Values)
        {
            foreach (var j in _nodeIndexMap.Values)
            {
                double weight = _adjacencyMatrix[i, j];
                if (Math.Abs(weight - double.MaxValue) < 1e-9 || i == j)
                {
                    continue;
                }

                var source = _nodeIndexMap.First(kvp => kvp.Value == i).Key;
                var target = _nodeIndexMap.First(kvp => kvp.Value == j).Key;
                bool directed = Math.Abs(weight - _adjacencyMatrix[j, i]) > 1e-9;
                if (directed || i < j)
                {
                    string color = "#000000";

                    if (
                        source.VisualizationParameters.Color == target.VisualizationParameters.Color
                        && !Equals(
                            source.VisualizationParameters.Label,
                            target.VisualizationParameters.Label
                        )
                    )
                    {
                        color = source.VisualizationParameters.Color;
                    }

                    _edges.Add(new Edge<T>(source, target, weight, directed, color));
                }
            }
        }
    }

    /// <summary>
    /// Builds edges from the adjacency matrix and populates the _edgeList.
    /// Also populates the adjacency dictionary for convenience.
    /// </summary>
    private void BuildEdgesFromMatrix()
    {
        foreach (var source in _nodes)
        {
            foreach (var target in _nodes)
            {
                double weight = _adjacencyMatrix[source.Id, target.Id];
                if (
                    (!_isDirected && source.Id > target.Id)
                    || Math.Abs(weight - double.MaxValue) < 1e-9
                    || source == target
                )
                {
                    continue;
                }

                bool directed = Math.Abs(weight - _adjacencyMatrix[target.Id, source.Id]) > 1e-9;
                if (directed || source.Id < target.Id)
                {
                    string color = "#000000";
                    if (
                        source.VisualizationParameters.Color == target.VisualizationParameters.Color
                        && !Equals(
                            source.VisualizationParameters.Label,
                            target.VisualizationParameters.Label
                        )
                    )
                    {
                        color = source.VisualizationParameters.Color;
                    }

                    _edges.Add(new Edge<T>(source, target, weight, directed, color));
                }

                _adjacencyList[source].Add(target, weight);

                if (!_isDirected)
                {
                    _adjacencyList[target].Add(source, weight);
                }
            }
        }
    }

    #endregion Private Methods
}
