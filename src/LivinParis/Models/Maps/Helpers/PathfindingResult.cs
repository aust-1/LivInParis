namespace LivInParisRoussilleTeynier.Models.Maps.Helpers;

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
