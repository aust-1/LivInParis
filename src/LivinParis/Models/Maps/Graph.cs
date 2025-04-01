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
                if (Math.Abs(weight - double.MaxValue) > 1e-9 && i != j)
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
            _correspondingCoordinates.Add(node, i);
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
                if (Math.Abs(weight - double.MaxValue) > 1e-9 && source != target)
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
    public SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>> AdjacencyList
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
    /// Gets the mapping of nodes to their corresponding coordinates in the adjacency matrix.
    /// </summary>
    public SortedDictionary<Node<T>, int> CorrespondingCoordinates
    {
        get { return _correspondingCoordinates; }
    }

    // /// <summary>
    // /// Gets the distance matrix for all pairs of nodes,
    // /// computed by the Roy-Floyd-Warshall algorithm.
    // /// </summary>
    // public double[,] DistanceMatrix
    // {
    //     get { return _distanceMatrix; }
    // }

    /// <summary>
    /// Gets the number of nodes (the order of the graph).
    /// </summary>
    public int Order
    {
        get { return _order; }
    }

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

    /// <summary>
    /// Indicates whether this graph is directed.
    /// </summary>
    public bool IsDirected
    {
        get { return _isDirected; }
    }

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
    public List<Node<T>> FindAnyCycle(bool simpleCycle = false)
    {
        return CycleDetector<T>.FindAnyCycle(this, simpleCycle);
    }

    #endregion Public Methods - Cycle Detection

    #region Public Methods - SCC Detection

    public List<Graph<T>> GetStronglyConnectedComponents()
    {
        return CycleDetector<T>.GetStronglyConnectedComponents(this);
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
        return GraphAlgorithms<T>.BFS(this, start);
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
        return GraphAlgorithms<T>.DFS(this, start, inverted);
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
        return GraphAlgorithms<T>.Dijkstra(this, start);
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
        return GraphAlgorithms<T>.BellmanFord(this, start);
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
        return GraphAlgorithms<T>.RoyFloydWarshall(this);
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
        Visualization<T>.DisplayGraph(this, outputImageName, layout, shape);
    }

    #endregion Public Methods - Drawing

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
