namespace LivinParisRoussilleTeynier.Models.Maps.Helpers;

/// <summary>
/// Provides various graph algorithms, including BFS, DFS,
/// shortest-path algorithms (Dijkstra, Bellman-Ford), and Roy-Floyd-Warshall.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in each node of the graph (must be non-null).
/// </typeparam>
public static class GraphAlgorithms<T>
    where T : notnull
{
    //FIXME: nearest station avec correspondances
    #region Public Methods - Traversals

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node or identifier.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The node or identifier from which to begin BFS.</param>
    /// <returns>A list of nodes in the order they were discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="start"/> is invalid or not found in the graph.
    /// </exception>
    public static List<Node<T>> BFS<TU>(Graph<T> graph, TU start)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.AdjacencyList.ContainsKey(startNode))
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

            if (graph.AdjacencyList.TryGetValue(current, out var neighbors))
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
    /// Performs a Depth-First Search (DFS) starting from the specified node or identifier.
    /// If <paramref name="inverted"/> is <c>true</c>, traverses in reverse order.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The node or identifier from which to begin DFS.</param>
    /// <param name="inverted">If <c>true</c>, the graph traversal order is reversed.</param>
    /// <returns>A list of visited nodes in the order they were discovered.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or not found in the graph.
    /// </exception>
    public static List<Node<T>> DFS<TU>(Graph<T> graph, TU start, bool inverted = false)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.AdjacencyList.ContainsKey(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var visited = new HashSet<Node<T>>();
        var result = new List<Node<T>>();

        DFSUtil(graph, startNode, visited, result, inverted);
        return result;
    }

    #endregion Public Methods - Traversals

    #region Public Methods - Pathfinding

    //TODO: Méthode graph.Dist(Node<T> a, Node<T> b)

    /// <summary>
    /// Executes Dijkstra's algorithm from the specified node or identifier,
    /// returning the shortest path to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The starting node or identifier for the Dijkstra's algorithm.</param>
    /// <returns>
    /// A dictionary mapping each reachable node to a list of nodes representing the path taken
    /// from <paramref name="start"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    public static SortedDictionary<Node<T>, List<Node<T>>> Dijkstra<TU>(Graph<T> graph, TU start)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.Nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        var visited = new HashSet<Node<T>>();

        foreach (var node in graph.Nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        while (visited.Count < graph.Order)
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

            if (graph.AdjacencyList.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors.Keys.Where(n => !visited.Contains(n)))
                {
                    var altDistance =
                        result[current].Distance
                        + graph.AdjacencyMatrix[
                            graph.NodeIndexMap[current],
                            graph.NodeIndexMap[neighbor]
                        ];

                    if (altDistance < result[neighbor].Distance)
                    {
                        result[neighbor].Distance = altDistance;
                        result[neighbor].Predecessor = current;
                    }
                }
            }
        }

        return BuildPaths(result);
    }

    /// <summary>
    /// Executes the Bellman-Ford algorithm from the specified node or identifier,
    /// returning the paths to each reachable node and detecting negative-weight cycles if present.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="graph">The graph to traverse.</param>
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
    public static SortedDictionary<Node<T>, List<Node<T>>> BellmanFord<TU>(Graph<T> graph, TU start)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.Nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        foreach (var node in graph.Nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        bool relaxed = false;
        for (int i = 0; i < graph.Order; i++)
        {
            relaxed = false;
            foreach (var edge in graph.Edges)
            {
                var source = edge.SourceNode;
                var target = edge.TargetNode;
                var weight = edge.Weight;

                double altDist = result[source].Distance + weight;
                if (altDist < result[target].Distance)
                {
                    result[target].Distance = altDist;
                    result[target].Predecessor = source;
                    relaxed = true;
                }

                if (!edge.IsDirected)
                {
                    double altDistReverse = result[target].Distance + weight;
                    if (altDistReverse < result[source].Distance)
                    {
                        result[source].Distance = altDistReverse;
                        result[source].Predecessor = target;
                        relaxed = true;
                    }
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

    /// <summary>
    /// Executes Dijkstra's algorithm from the specified node or identifier,
    /// returning the shortest path to each reachable node.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The starting node or identifier for the Dijkstra's algorithm.</param>
    /// <returns>
    /// A graph representing the shortest paths from <paramref name="start"/> to each reachable node.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="start"/> is invalid or the node does not exist.
    /// </exception>
    public static Graph<T> GetPartialGraphByDijkstra<TU>(Graph<T> graph, TU start)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.Nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        var visited = new HashSet<Node<T>>();

        foreach (var node in graph.Nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        while (visited.Count < graph.Order)
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

            if (graph.AdjacencyList.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors.Keys.Where(n => !visited.Contains(n)))
                {
                    var altDistance =
                        result[current].Distance
                        + graph.AdjacencyMatrix[
                            graph.NodeIndexMap[current],
                            graph.NodeIndexMap[neighbor]
                        ];

                    if (altDistance < result[neighbor].Distance)
                    {
                        result[neighbor].Distance = altDistance;
                        result[neighbor].Predecessor = current;
                    }
                }
            }
        }

        return BuildGraph(graph, result);
    }

    /// <summary>
    /// Executes the Bellman-Ford algorithm from the specified node or identifier,
    /// returning the paths to each reachable node and detecting negative-weight cycles if present.
    /// </summary>
    /// <typeparam name="TU">
    /// The type of <paramref name="start"/> (could be an int for ID, a <see cref="Node{T}"/>, or the node's data of type <typeparamref name="T"/>).
    /// </typeparam>
    /// <param name="graph">The graph to traverse.</param>
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
    public static Graph<T> GetPartialGraphByBellmanFord<TU>(Graph<T> graph, TU start)
        where TU : notnull
    {
        var startNode = ResolveNode(start);

        if (!graph.Nodes.Contains(startNode))
        {
            throw new ArgumentException("Invalid start node.");
        }

        var result = new SortedDictionary<Node<T>, PathfindingResult<T>>();
        foreach (var node in graph.Nodes)
        {
            result[node] = new PathfindingResult<T>(double.MaxValue, null);
        }
        result[startNode].Distance = 0;

        bool relaxed = false;
        for (int i = 0; i < graph.Order; i++)
        {
            relaxed = false;
            foreach (var edge in graph.Edges)
            {
                var source = edge.SourceNode;
                var target = edge.TargetNode;
                var weight = edge.Weight;

                double altDist = result[source].Distance + weight;
                if (altDist < result[target].Distance)
                {
                    result[target].Distance = altDist;
                    result[target].Predecessor = source;
                    relaxed = true;
                }

                if (!edge.IsDirected)
                {
                    double altDistReverse = result[target].Distance + weight;
                    if (altDistReverse < result[source].Distance)
                    {
                        result[source].Distance = altDistReverse;
                        result[source].Predecessor = target;
                        relaxed = true;
                    }
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

        return BuildGraph(graph, result);
    }

    //TODO: Distance + chemins. Pas d'attribut distance_matrix mais méthode de recherche de pcc dans pathfinding. Lzay computation ??

    /// <summary>
    /// Uses the Roy-Floyd-Warshall algorithm to compute shortest paths
    /// between all pairs of nodes in the graph.
    /// </summary>
    /// <param name="graph">The graph to analyze.</param>
    /// <returns>
    /// A 2D array of lists, where each element <c>pathMatrix[i, j]</c>
    /// represents the path from node i to node j.
    /// </returns>
    public static List<Node<T>>[,] RoyFloydWarshall(Graph<T> graph)
    {
        int n = graph.Order;
        var distanceMatrix = new double[n, n];
        var pathMatrix = new List<Node<T>>[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distanceMatrix[i, j] = graph.AdjacencyMatrix[i, j];
                pathMatrix[i, j] = [];

                if (i == j)
                {
                    var nodeI = graph.NodeIndexMap.First(kvp => kvp.Value == i).Key;
                    pathMatrix[i, j].Add(nodeI);
                }
                else if (Math.Abs(distanceMatrix[i, j] - double.MaxValue) > 1e-9)
                {
                    var sourceI = graph.NodeIndexMap.First(kvp => kvp.Value == i).Key;
                    var targetJ = graph.NodeIndexMap.First(kvp => kvp.Value == j).Key;
                    pathMatrix[i, j].Add(sourceI);
                    pathMatrix[i, j].Add(targetJ);
                }
            }
        }

        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (
                        i == j
                        || i == k
                        || j == k
                        || Math.Abs(distanceMatrix[i, k] - double.MaxValue) < 1e-9
                        || Math.Abs(distanceMatrix[k, j] - double.MaxValue) < 1e-9
                    )
                    {
                        continue;
                    }

                    double distanceViaK = distanceMatrix[i, k] + distanceMatrix[k, j];
                    if (distanceViaK < distanceMatrix[i, j])
                    {
                        distanceMatrix[i, j] = distanceViaK;

                        var pathViaK = new List<Node<T>>(pathMatrix[i, k]);
                        pathViaK.RemoveAt(pathViaK.Count - 1);
                        pathViaK.AddRange(pathMatrix[k, j]);

                        pathMatrix[i, j] = pathViaK;
                    }
                }
            }
        }

        return pathMatrix;
    }

    #endregion Public Methods - Pathfinding

    #region Private Methods

    /// <summary>
    /// Recursively performs DFS from the specified node, adding
    /// each visited node to <paramref name="result"/>.
    /// </summary>
    /// <param name="graph">The graph being traversed.</param>
    /// <param name="node">The current node in DFS.</param>
    /// <param name="visited">A set of nodes already visited.</param>
    /// <param name="result">The list storing discovered nodes.</param>
    /// <param name="inverted">If <c>true</c>, the graph traversal order is reversed.</param>
    private static void DFSUtil(
        Graph<T> graph,
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
                var predecessor in graph
                    .AdjacencyList.Where(kvp => kvp.Value.ContainsKey(node))
                    .Select(kvp => kvp.Key)
            )
            {
                if (!visited.Contains(predecessor))
                {
                    DFSUtil(graph, predecessor, visited, result, inverted);
                }
            }
        }
        else
        {
            if (graph.AdjacencyList.TryGetValue(node, out var neighbors))
            {
                foreach (var neighbor in neighbors.Keys)
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFSUtil(graph, neighbor, visited, result, inverted);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Builds the resulting path list for each node, given a dictionary
    /// of <see cref="PathfindingResult{T}"/>.
    /// </summary>
    /// <param name="results">
    /// A dictionary from each node to its <see cref="PathfindingResult{T}"/>,
    /// which includes distance and predecessor data.
    /// </param>
    /// <returns>
    /// A dictionary mapping each node to a list of nodes describing
    /// the path from the start node to that node.
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
        }

        return paths;
    }

    /// <summary>
    /// Builds the resulting path list for each node, given a dictionary
    /// of <see cref="PathfindingResult{T}"/>.
    /// </summary>
    /// <param name="graph">The original graph.</param>
    /// <param name="results">
    /// A dictionary from each node to its <see cref="PathfindingResult{T}"/>,
    /// which includes distance and predecessor data.
    /// </param>
    /// <returns>
    /// A new graph representing the shortest paths from the start node to each reachable node.
    /// </returns>
    private static Graph<T> BuildGraph(
        Graph<T> graph,
        SortedDictionary<Node<T>, PathfindingResult<T>> results
    )
    {
        var localAdjacency = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();

        foreach (var node in graph.Nodes)
        {
            localAdjacency[node] = [];
        }

        foreach (var kvp in results)
        {
            var node = kvp.Key;
            var predecessor = kvp.Value.Predecessor;

            if (predecessor != null)
            {
                localAdjacency[predecessor]
                    .Add(
                        node,
                        graph.AdjacencyMatrix[
                            graph.NodeIndexMap[predecessor],
                            graph.NodeIndexMap[node]
                        ]
                    );
            }
        }

        return new Graph<T>(localAdjacency);
    }

    /// <summary>
    /// Converts the given <paramref name="start"/> object to a <see cref="Node{T}"/>.
    /// Acceptable types: <c>int</c> (node ID), <see cref="Node{T}"/>, or <typeparamref name="T"/> (node data).
    /// </summary>
    /// <typeparam name="TU">The type of <paramref name="start"/>.</typeparam>
    /// <param name="start">An integer ID, a node, or a data object.</param>
    /// <returns>A corresponding <see cref="Node{T}"/> in this graph.</returns>
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

    #endregion Private Methods
}
