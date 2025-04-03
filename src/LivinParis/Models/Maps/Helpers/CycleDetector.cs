namespace LivinParis.Models.Maps.Helpers;

/// <summary>
/// Provides functionality for detecting cycles and extracting strongly connected components
/// (SCCs) within a generic <see cref="Graph{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in each node (must be non-null).
/// </typeparam>
public static class CycleDetector<T>
    where T : notnull
{
    #region Public Methods - Strongly Connected Components

    /// <summary>
    /// Computes the strongly connected components (SCCs) of the specified graph.
    /// Each SCC is returned as a separate subgraph.
    /// </summary>
    /// <param name="graph">The graph from which to extract strongly connected components.</param>
    /// <returns>A list of <see cref="Graph{T}"/> objects, each representing one SCC.</returns>
    public static List<Graph<T>> GetStronglyConnectedComponents(Graph<T> graph)
    {
        var result = new List<Graph<T>>();
        var visited = new HashSet<Node<T>>();

        while (visited.Count < graph.Order)
        {
            var startNode = graph.Nodes.First(n => !visited.Contains(n));
            var localAdjacency = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();

            var successors = GraphAlgorithms<T>.DFS(graph, startNode, inverted: false);
            var predecessors = GraphAlgorithms<T>.DFS(graph, startNode, inverted: true);

            foreach (var node in successors.Where(n => predecessors.Contains(n)))
            {
                visited.Add(node);
                localAdjacency[node] = [];
            }

            foreach (var edge in graph.Edges)
            {
                if (
                    localAdjacency.ContainsKey(edge.SourceNode)
                    && localAdjacency.ContainsKey(edge.TargetNode)
                )
                {
                    localAdjacency[edge.SourceNode].Add(edge.TargetNode, edge.Weight);
                    if (!edge.IsDirected)
                    {
                        localAdjacency[edge.TargetNode].Add(edge.SourceNode, edge.Weight);
                    }
                }
            }

            var scc = new Graph<T>(localAdjacency);
            result.Add(scc);
        }

        return result;
    }

    #endregion Public Methods - Strongly Connected Components

    #region Public Methods - Cycle Detection

    /// <summary>
    /// Searches for any cycle in the specified graph, working for both directed and undirected graphs.
    /// In an undirected graph, if <paramref name="simpleCycle"/> is <c>true</c>, it ignores
    /// immediate back edges to the parent node, detecting only "simple" cycles.
    /// </summary>
    /// <param name="graph">The graph in which to search for cycles.</param>
    /// <param name="simpleCycle">
    /// <c>true</c> if parent-to-child edges should be ignored in an undirected scenario; otherwise <c>false</c>.
    /// </param>
    /// <returns>
    /// A list of <see cref="Node{T}"/> representing the first cycle found, or an empty list if no cycle is discovered.
    /// </returns>
    public static List<Node<T>> FindAnyCycle(Graph<T> graph, bool simpleCycle = false)
    {
        var visited = new HashSet<Node<T>>();
        var recursionStack = new HashSet<Node<T>>();
        var parentMap = new Dictionary<Node<T>, Node<T>>();

        foreach (var node in graph.Nodes)
        {
            if (!visited.Contains(node))
            {
                if (graph.IsDirected)
                {
                    if (
                        TryFindCycleDirected(
                            graph,
                            node,
                            visited,
                            recursionStack,
                            parentMap,
                            out var cycle,
                            simpleCycle
                        )
                    )
                    {
                        return cycle;
                    }
                }
                else
                {
                    if (
                        TryFindCycleUndirected(
                            graph,
                            node,
                            visited,
                            parentMap,
                            null,
                            out var cycle,
                            simpleCycle
                        )
                    )
                    {
                        return cycle;
                    }
                }
            }
        }
        return [];
    }

    #endregion Public Methods - Cycle Detection

    #region Private Methods - Cycle Detection Helpers

    /// <summary>
    /// Attempts to detect a cycle in a directed graph using DFS and a recursion stack.
    /// </summary>
    /// <param name="graph">The directed graph to search.</param>
    /// <param name="current">The current node being explored.</param>
    /// <param name="visited">A set of visited nodes.</param>
    /// <param name="recursionStack">Tracks the current path in the DFS recursion.</param>
    /// <param name="parentMap">A map to reconstruct the cycle if found.</param>
    /// <param name="cycle">
    /// Outputs a list of nodes forming the cycle, or <c>null</c> if no cycle is found.
    /// </param>
    /// <param name="simpleCycle">
    /// <c>true</c> in undirected scenarios for ignoring back edges to the immediate parent;
    /// not typically used in directed scenarios.
    /// </param>
    /// <returns><c>true</c> if a cycle is detected; otherwise <c>false</c>.</returns>
    private static bool TryFindCycleDirected(
        Graph<T> graph,
        Node<T> current,
        HashSet<Node<T>> visited,
        HashSet<Node<T>> recursionStack,
        Dictionary<Node<T>, Node<T>> parentMap,
        out List<Node<T>> cycle,
        bool simpleCycle = false
    )
    {
        visited.Add(current);
        recursionStack.Add(current);

        if (graph.AdjacencyList.TryGetValue(current, out var neighbors))
        {
            foreach (var neighbor in neighbors.Keys)
            {
                if (!visited.Contains(neighbor))
                {
                    parentMap[neighbor] = current;
                    if (
                        TryFindCycleDirected(
                            graph,
                            neighbor,
                            visited,
                            recursionStack,
                            parentMap,
                            out cycle,
                            simpleCycle
                        )
                    )
                    {
                        return true;
                    }
                }
                else if (recursionStack.Contains(neighbor))
                {
                    cycle = ReconstructCyclePath(current, neighbor, parentMap);
                    return true;
                }
            }
        }

        recursionStack.Remove(current);
        cycle = null!;
        return false;
    }

    /// <summary>
    /// Attempts to detect a cycle in an undirected graph via DFS.
    /// </summary>
    /// <param name="graph">The undirected graph to search.</param>
    /// <param name="current">The current node being visited.</param>
    /// <param name="visited">A set of visited nodes.</param>
    /// <param name="parentMap">A map to reconstruct the cycle, if found.</param>
    /// <param name="parent">The node's parent in the DFS tree.</param>
    /// <param name="cycle">
    /// Outputs the cycle as a list of <see cref="Node{T}"/>, or <c>null</c> if none is found.
    /// </param>
    /// <param name="simpleCycle">
    /// <c>true</c> to ignore immediate back edges to the parent, finding only "simple" cycles.
    /// </param>
    /// <returns><c>true</c> if a cycle is found; otherwise <c>false</c>.</returns>
    private static bool TryFindCycleUndirected(
        Graph<T> graph,
        Node<T> current,
        HashSet<Node<T>> visited,
        Dictionary<Node<T>, Node<T>> parentMap,
        Node<T>? parent,
        out List<Node<T>> cycle,
        bool simpleCycle = false
    )
    {
        visited.Add(current);

        if (graph.AdjacencyList.TryGetValue(current, out var neighbors))
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
                            graph,
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
                    cycle = ReconstructCyclePath(current, neighbor, parentMap);
                    return true;
                }
            }
        }

        cycle = null!;
        return false;
    }

    /// <summary>
    /// Reconstructs a cycle path once two nodes in the DFS recursion stack meet.
    /// </summary>
    /// <param name="current">The node that discovered the cycle.</param>
    /// <param name="neighbor">The previously visited node forming the loop.</param>
    /// <param name="parentMap">Tracks the parent of each node in the DFS tree.</param>
    /// <returns>A list of nodes forming the detected cycle.</returns>
    private static List<Node<T>> ReconstructCyclePath(
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

    #endregion Private Methods - Cycle Detection Helpers
}
