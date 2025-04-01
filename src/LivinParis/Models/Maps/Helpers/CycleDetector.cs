namespace LivinParis.Models.Maps.Helpers;

public static class CycleDetector<T>
    where T : notnull
{
    #region Public Methods - SCC Detection

    public static List<Graph<T>> GetStronglyConnectedComponents(Graph<T> graph)
    {
        var result = new List<Graph<T>>();
        var visited = new HashSet<Node<T>>();

        while (visited.Count < graph.Order)
        {
            var startNode = graph.Nodes.Where(n => !visited.Contains(n)).First();
            var adjacencyList = new SortedDictionary<Node<T>, SortedDictionary<Node<T>, double>>();
            var successors = GraphAlgorithms<T>.DFS(graph, startNode, false);
            var predecessor = GraphAlgorithms<T>.DFS(graph, startNode, true);

            foreach (var node in successors.Where(n => predecessor.Contains(n)))
            {
                visited.Add(node);
                adjacencyList[node] = new SortedDictionary<Node<T>, double>();
            }

            foreach (var edge in graph.Edges)
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

    #region Public Methods - Cycle Detection

    /// <summary>
    /// Searches for any cycle in the graph.
    /// Works for both directed and undirected graphs.
    /// Optionally ignores the immediate parent for undirected simple cycles.
    /// </summary>
    /// <param name="graph">The graph to search for cycles.</param>
    /// <param name="simpleCycle">
    /// If <c>true</c> and the graph is undirected, the method will ignore
    /// an edge back to the immediate parent. Defaults to <c>false</c>.
    /// </param>
    /// <returns>
    /// A string describing the detected cycle (IDs and data) if found; otherwise <c>null</c>.
    /// </returns>
    public static List<Node<T>> FindAnyCycle(Graph<T> graph, bool simpleCycle = false)
    {
        var visited = new HashSet<Node<T>>();
        var recStack = new HashSet<Node<T>>();
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
                            recStack,
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
        return new List<Node<T>>();
    }

    #endregion Public Methods - Cycle Detection

    #region Private Helpers - Cycle Detection

    /// <summary>
    /// Attempts to find a cycle in a directed graph using DFS and a recursion stack.
    /// </summary>
    /// <param name="graph">The graph to search for cycles.</param>
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
    private static bool TryFindCycleDirected(
        Graph<T> graph,
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
    /// <param name="graph">The graph to search for cycles.</param>
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

    #endregion Private Helpers - Cycle Detection
}
