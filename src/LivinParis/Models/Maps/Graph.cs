using LivinParisRoussilleTeynier.Models.Maps.Helpers;

namespace LivinParisRoussilleTeynier.Models.Maps;

/// <summary>
/// Represents a generic graph containing nodes and edges,
/// supporting directed or undirected structures with optional weights.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in each node. Must be non-null.
/// </typeparam>
public class Graph<T>
    where T : notnull
{
    #region Fields

    //TODO: rajouter diamètre du graphe
    //QUESTION: Gestion des propriétés

    /// <summary>
    /// The set of nodes in this graph, sorted by their IDs.
    /// </summary>
    private readonly SortedSet<Node<T>> _nodes;

    /// <summary>
    /// The list of edges in this graph.
    /// </summary>
    private readonly List<Edge<T>> _edges;

    /// <summary>
    /// The adjacency list mapping each node to its reachable neighbors and the corresponding edge weights.
    /// </summary>
    private readonly SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> _adjacencyList;

    /// <summary>
    /// The adjacency matrix, where <c>_adjacencyMatrix[i, j]</c> indicates the weight of the edge from <c>i</c> to <c>j</c>.
    /// </summary>
    private readonly double[,] _adjacencyMatrix;

    /// <summary>
    /// A mapping from each node to its index in the adjacency matrix.
    /// </summary>
    private readonly SortedDictionary<Node<T>, int> _nodeIndexMap;

    /// <summary>
    /// Indicates whether this graph is directed (<c>true</c>) or undirected (<c>false</c>).
    /// </summary>
    private readonly bool _isDirected;

    /// <summary>
    /// Indicates whether this graph is weighted (<c>true</c>) or unweighted (<c>false</c>).
    /// If any edge has a weight other than 1.0, the graph is considered weighted.
    /// </summary>
    private readonly bool _isWeighted;

    /// <summary>
    /// Indicates whether the graph is connected, based on a single DFS from the first node
    /// or a check of strongly connected components (for directed graphs).
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
    /// The weight of the graph, which is the sum of all edge weights.
    /// </summary>
    private readonly double _weight;

    /// <summary>
    /// The density of the graph.
    /// For a directed graph, density = E / (V * (V - 1)).
    /// For an undirected graph, density = (2 * E) / (V * (V - 1)).
    /// </summary>
    private readonly double _density;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph{T}"/> class from an adjacency list,
    /// automatically determining directedness by checking matrix symmetry.
    /// </summary>
    /// <param name="adjacencyList">
    /// A dictionary mapping each node to a dictionary of its neighbors and the corresponding edge weights.
    /// If no weight is specified, the default assumption is 1.0 (applied in your dictionary).
    /// </param>
    public Graph(SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> adjacencyList)
    {
        _nodes = new SortedSet<Node<T>>(adjacencyList.Keys);
        _edges = [];
        _adjacencyList = adjacencyList;
        _order = _nodes.Count;
        _nodeIndexMap = [];
        int counter = 0;
        foreach (var node in _adjacencyList.Keys)
        {
            _nodeIndexMap[node] = counter++;
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
            var source = kvp.Key;
            foreach (var neighbor in kvp.Value)
            {
                _adjacencyMatrix[_nodeIndexMap[source], _nodeIndexMap[neighbor.Key]] =
                    neighbor.Value;
            }
        }

        _isDirected = !IsMatrixSymmetric(_adjacencyMatrix);

        BuildEdgesFromList();

        _size = _edges.Count + _edges.Count(e => !e.IsDirected);

        _density = _isDirected
            ? (double)_size / (_order * (_order - 1))
            : (2.0 * _size) / (_order * (_order - 1));

        _isWeighted = _edges.Any(e => Math.Abs(e.Weight - 1.0) > 1e-9);
        _isConnected = PerformDepthFirstSearch(_nodes.First()).Count == _order;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph{T}"/> class from an adjacency matrix,
    /// automatically determining directedness by checking matrix symmetry.
    /// </summary>
    /// <param name="adjacencyMatrix">
    /// A square 2D array representing adjacency weights from i to j.
    /// Use <see cref="double.MaxValue"/> to indicate no edge.
    /// </param>
    /// <exception cref="ArgumentException">Thrown if the provided matrix is not square.</exception>
    public Graph(double[,] adjacencyMatrix)
    {
        if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
        {
            throw new ArgumentException("Adjacency matrix must be square.");
        }

        _isDirected = !IsMatrixSymmetric(adjacencyMatrix);
        _nodes = [];
        _edges = [];
        _adjacencyList = [];
        _adjacencyMatrix = adjacencyMatrix;
        _nodeIndexMap = [];
        _order = _adjacencyMatrix.GetLength(0);

        for (int i = 0; i < _order; i++)
        {
            var node = Node<T>.GetNode(i);
            _nodes.Add(node);
            _adjacencyList[node] = [];
            _nodeIndexMap[node] = i;
        }

        BuildEdges();

        _size = _edges.Count + _edges.Count(e => !e.IsDirected);
        _weight = _edges.Sum(e => e.Weight) + _edges.Where(e => !e.IsDirected).Sum(e => e.Weight);

        _density = _isDirected
            ? (double)_size / (_order * (_order - 1))
            : 2.0 * _size / (_order * (_order - 1));

        _isWeighted = _edges.Any(e => Math.Abs(e.Weight - 1.0) > 1e-9);
        _isConnected = PerformDepthFirstSearch(_nodes.First()).Count == _order;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the set of nodes in this graph.
    /// </summary>
    public SortedSet<Node<T>> Nodes
    {
        get { return _nodes; }
    }

    /// <summary>
    /// Gets the list of edges in the graph.
    /// </summary>
    public List<Edge<T>> Edges
    {
        get { return _edges; }
    }

    /// <summary>
    /// Gets the adjacency list, mapping each node to a dictionary of its neighbors and their weights.
    /// </summary>
    public SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> AdjacencyList
    {
        get { return _adjacencyList; }
    }

    /// <summary>
    /// Gets the adjacency matrix for the graph,
    /// where <see cref="double.MaxValue"/> indicates no edge.
    /// </summary>
    public double[,] AdjacencyMatrix
    {
        get { return _adjacencyMatrix; }
    }

    /// <summary>
    /// Gets the mapping of each node to its row/column index in <see cref="AdjacencyMatrix"/>.
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

    /// <summary>
    /// Gets the size of the graph (number of edges).
    /// </summary>
    public int Size
    {
        get { return _size; }
    }

    /// <summary>
    /// Gets the density of the graph.
    /// For a directed graph, density = E / (V * (V - 1)).
    /// For an undirected graph, density = (2 * E) / (V * (V - 1)).
    /// </summary>
    public double Density
    {
        get { return _density; }
    }

    /// <summary>
    /// Gets the total weight of the graph,
    /// </summary>
    public double Weight
    {
        get { return _weight; }
    }

    /// <summary>
    /// Indicates whether the graph is directed.
    /// </summary>
    public bool IsDirected
    {
        get { return _isDirected; }
    }

    /// <summary>
    /// Indicates whether the graph is weighted (i.e., any edge has a weight different from 1.0).
    /// </summary>
    public bool IsWeighted
    {
        get { return _isWeighted; }
    }

    /// <summary>
    /// Indicates whether the graph is connected based on its strongly connected components.
    /// </summary>
    public bool IsConnected
    {
        get { return _isConnected; }
    }

    #endregion Properties

    #region Public Methods - SCC (Strongly Connected Components)

    /// <summary>
    /// Computes and returns the strongly connected components (SCCs) of the graph.
    /// </summary>
    /// <returns>A list of subgraphs, each representing a strongly connected component.</returns>
    public List<Graph<T>> GetStronglyConnectedComponents()
    {
        return CycleDetector<T>.GetStronglyConnectedComponents(this);
    }

    #endregion Public Methods - SCC

    #region Public Methods - Cycle Detection

    /// <summary>
    /// Searches for any cycle in the graph, for either directed or undirected structures.
    /// Optionally ignores the immediate parent edge for a simple cycle in an undirected graph.
    /// </summary>
    /// <param name="simpleCycle">
    /// If <c>true</c> and the graph is undirected, edges to the immediate parent are ignored
    /// to detect only "simple" cycles.
    /// </param>
    /// <returns>
    /// A list of nodes forming the first cycle found, or an empty list if no cycle is found.
    /// </returns>
    public List<Node<T>> DetectAnyCycle(bool simpleCycle = false)
    {
        return CycleDetector<T>.FindAnyCycle(this, simpleCycle);
    }

    #endregion Public Methods - Cycle Detection

    #region Public Methods - Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node or identifier.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The node or identifier from which to begin BFS.</param>
    /// <returns>A list of nodes in the order they were discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="start"/> is invalid or not found in the graph.
    /// </exception>
    public List<Node<T>> PerformBreadthFirstSearch<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.BFS(this, start);
    }

    /// <summary>
    /// Performs a Depth-First Search (DFS) starting from the specified node or identifier.
    /// If <paramref name="inverted"/> is <c>true</c>, traverses in reverse order.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The node or identifier from which to begin DFS.</param>
    /// <param name="inverted">If <c>true</c>, the graph traversal order is reversed.</param>
    /// <returns>A list of visited nodes in the order they were discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or not found in the graph.
    /// </exception>
    public List<Node<T>> PerformDepthFirstSearch<TU>(TU start, bool inverted = false)
        where TU : notnull
    {
        return GraphAlgorithms<T>.DFS(this, start, inverted);
    }

    #endregion Public Methods - Traversals

    #region Public Methods - Pathfinding

    /// <summary>
    /// Executes Dijkstra's algorithm from the specified node or identifier,
    /// returning the shortest path to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The starting node or identifier for the Dijkstra's algorithm.</param>
    /// <returns>
    /// A dictionary mapping each reachable node to a list of nodes representing the path taken
    /// from <paramref name="start"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> ComputeDijkstra<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.Dijkstra(this, start);
    }

    /// <summary>
    /// Executes the Bellman-Ford algorithm from the specified node or identifier,
    /// returning the paths to each reachable node and detecting negative-weight cycles if present.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The starting node or identifier for the Bellman-Ford algorithm.</param>
    /// <returns>
    /// A dictionary mapping each reachable node to a list of nodes representing the path taken
    /// from <paramref name="start"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the graph contains a negative-weight cycle.
    /// </exception>
    public SortedDictionary<Node<T>, List<Node<T>>> ComputeBellmanFord<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.BellmanFord(this, start);
    }

    /// <summary>
    /// Executes Dijkstra's algorithm from the specified node or identifier,
    /// returning the shortest path to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The starting node or identifier for the Dijkstra's algorithm.</param>
    /// <returns>
    /// A graph representing the shortest paths from <paramref name="start"/> to each reachable node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    public Graph<T> GetPartialGraphByDijkstra<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.GetPartialGraphByDijkstra(this, start);
    }

    //TODO: A*

    /// <summary>
    /// Executes the Bellman-Ford algorithm from the specified node or identifier,
    /// returning the paths to each reachable node and detecting negative-weight cycles if present.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="start">The starting node or identifier for the Bellman-Ford algorithm.</param>
    /// <returns>
    /// A graph representing the shortest paths from <paramref name="start"/> to each reachable node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the graph contains a negative-weight cycle.
    /// </exception>
    public Graph<T> GetPartialGraphByBellmanFord<TU>(TU start)
        where TU : notnull
    {
        return GraphAlgorithms<T>.GetPartialGraphByBellmanFord(this, start);
    }

    //TODO: Distance + chemins. Pas d'attribut distance_matrix mais méthode de recherche de pcc dans pathfinding. Lzay computation ??

    /// <summary>
    /// Executes the Roy-Floyd-Warshall algorithm to compute shortest paths
    /// between all pairs of nodes in the graph.
    /// </summary>
    /// <returns>
    /// A 2D array of lists, where each element is the path from node i to j.
    /// </returns>
    public List<Node<T>>[,] ComputeRoyFloydWarshall()
    {
        return GraphAlgorithms<T>.RoyFloydWarshall(this);
    }

    #endregion Public Methods - Pathfinding

    #region Public Methods - Rendering

    /// <summary>
    /// Exports the graph to a DOT file, invokes GraphViz to generate a PNG image,
    /// then removes the DOT file. The node shape and layout algorithm can be specified.
    /// </summary>
    /// <param name="outputImageName">The base name of the output image file (no extension). A timestamp is appended to avoid overwriting.</param>
    /// <param name="layout">The GraphViz layout algorithm (e.g. "dot", "neato", "fdp"). Default is "neato".</param>
    /// <param name="nodeShape">The shape to use for the nodes (e.g. "point", "circle"). Default is "point".</param>
    /// <param name="fontsize">The font size for node labels. Default is 10.0f.</param>
    public void DisplayGraph(
        string outputImageName = "graph",
        string layout = "neato",
        string nodeShape = "point",
        float fontsize = 10.0f
    )
    {
        Visualization<T>.DisplayGraph(this, outputImageName, layout, nodeShape, fontsize);
    }

    #endregion Public Methods - Rendering

    #region Private Methods

    /// <summary>
    /// Determines whether the given adjacency matrix is symmetric,
    /// implying an undirected graph if <c>true</c>.
    /// </summary>
    /// <param name="matrix">A square matrix of edge weights.</param>
    /// <returns><c>true</c> if the matrix is symmetric; otherwise <c>false</c>.</returns>
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

    #endregion Private Helpers - Matrix Symmetry

    #region Private Helpers - Edge Building

    /// <summary>
    /// Builds edges from the adjacency list into <see cref="_edges"/>.
    /// </summary>
    private void BuildEdgesFromList()
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
    /// Builds edges from the adjacency matrix into <see cref="_edges"/>,
    /// also updates the adjacency dictionary <see cref="_adjacencyList"/>.
    /// </summary>
    private void BuildEdges()
    {
        foreach (var source in _nodes)
        {
            foreach (var target in _nodes)
            {
                double weight = _adjacencyMatrix[source.Id, target.Id];
                if (
                    (!_isDirected && source.Id > target.Id)
                    || Math.Abs(weight - double.MaxValue) < 1e-9
                    || source.Equals(target)
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

    #endregion Private Helpers - Edge Building
}
