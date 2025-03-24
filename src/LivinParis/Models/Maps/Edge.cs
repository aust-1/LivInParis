namespace LivinParis.Models.Maps;

/// <summary>
/// Represents an edge (or arc) connecting two nodes in a graph.
/// </summary>
/// <typeparam name="T">
/// The type of data stored in the connected nodes.
/// </typeparam>
/// <remarks>
/// <para>
/// In an undirected graph, swapping the source and target nodes
/// is considered equivalent. If <c>IsDirected</c> is <c>false</c>,
/// reversing the direction does not create a new, distinct edge.
/// </para>
/// <para>
/// In a weighted graph, the <see cref="Weight"/> can store cost, distance,
/// capacity, or any other relevant metric.
/// </para>
/// </remarks>
public sealed class Edge<T> : IEquatable<Edge<T>>
    where T : notnull
{
    #region Fields

    /// <summary>
    /// The source node for this edge.
    /// </summary>
    private readonly Node<T> _sourceNode;

    /// <summary>
    /// The target node for this edge.
    /// </summary>
    private readonly Node<T> _targetNode;

    /// <summary>
    /// The numeric weight of this edge, defaulting to 1.0.
    /// </summary>
    private readonly double _weight;

    /// <summary>
    /// A value indicating whether this edge is directed.
    /// <c>true</c> implies a directed edge; <c>false</c> implies undirected.
    /// </summary>
    private readonly bool _isDirected;

    private readonly string _rgbColor;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Edge{T}"/> class.
    /// </summary>
    /// <param name="sourceNode">The source node of this edge.</param>
    /// <param name="targetNode">The target node of this edge.</param>
    /// <param name="weight">An optional weight for this edge (default is 1.0).</param>
    /// <param name="isDirected">
    /// <c>true</c> if the edge is directed;
    /// <c>false</c> if it is undirected (default is <c>false</c>).
    /// </param>
    /// <param name="rgbColor">The RGB color of this edge (default is black).</param>
    public Edge(
        Node<T> sourceNode,
        Node<T> targetNode,
        double weight = 1.0,
        bool isDirected = false,
        string rgbColor = "#000000"
    )
    {
        _sourceNode = sourceNode;
        _targetNode = targetNode;
        _weight = weight;
        _isDirected = isDirected;
        _rgbColor = rgbColor;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the source node of this edge.
    /// </summary>
    public Node<T> SourceNode
    {
        get { return _sourceNode; }
    }

    /// <summary>
    /// Gets the target node of this edge.
    /// </summary>
    public Node<T> TargetNode
    {
        get { return _targetNode; }
    }

    /// <summary>
    /// Gets the numeric weight of this edge.
    /// </summary>
    public double Weight
    {
        get { return _weight; }
    }

    /// <summary>
    /// Gets a value indicating whether this edge is directed.
    /// </summary>
    public bool IsDirected
    {
        get { return _isDirected; }
    }

    /// <summary>
    /// Gets the RGB color of this edge.
    /// </summary>
    public string RGBColor
    {
        get { return _rgbColor; }
    }

    #endregion Properties

    #region Public Methods

    /// <summary>
    /// Returns a string representation of this edge.
    /// </summary>
    /// <remarks>
    /// In a directed context, this takes the form:
    /// <c>Source --(Weight)--> Target</c>.
    /// </remarks>
    /// <returns>
    /// A string describing the source node, target node, and weight of the edge.
    /// </returns>
    public override string ToString()
    {
        return $"Edge: {_sourceNode.Id} --({_weight})--> {_targetNode.Id}";
    }

    #endregion Public Methods

    #region Equality and IEquatable Implementation

    /// <summary>
    /// Determines whether the specified <see cref="Edge{T}"/> is equal
    /// to the current <see cref="Edge{T}"/>, considering directedness,
    /// source, target, and weight.
    /// </summary>
    /// <param name="other">
    /// The other <see cref="Edge{T}"/> to compare with this instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if both edges represent the same connection (orientation, weight),
    /// or if both are undirected and effectively reversed of one another;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(Edge<T>? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        bool sameOrientation =
            _sourceNode.Equals(other._sourceNode)
            && _targetNode.Equals(other._targetNode)
            && _isDirected == other._isDirected
            && Math.Abs(_weight - other._weight) < 1e-9;

        bool reversedOrientation =
            !_isDirected
            && !other._isDirected
            && _sourceNode.Equals(other._targetNode)
            && _targetNode.Equals(other._sourceNode)
            && Math.Abs(_weight - other._weight) < 1e-9;

        return sameOrientation || reversedOrientation;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal
    /// to the current <see cref="Edge{T}"/>.
    /// </summary>
    /// <param name="obj">
    /// Another object to compare with this edge, which should also be an <see cref="Edge{T}"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="obj"/> is an equivalent <see cref="Edge{T}"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is Edge<T> edge)
        {
            return Equals(edge);
        }
        return false;
    }

    /// <summary>
    /// Returns the hash code for this edge, combining source, target, weight,
    /// and the directedness flag.
    /// </summary>
    /// <remarks>
    /// Ensures that edges that are considered equal produce the same hash code
    /// in both directed and undirected contexts.
    /// </remarks>
    /// <returns>
    /// An integer hash code derived from the fields of this edge.
    /// </returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 31) + _sourceNode.GetHashCode();
            hash = (hash * 31) + _targetNode.GetHashCode();
            hash = (hash * 31) + _weight.GetHashCode();
            hash = (hash * 31) + _isDirected.GetHashCode();
            return hash;
        }
    }

    #endregion Equality and IEquatable Implementation

    #region Operators

    /// <summary>
    /// Checks whether two <see cref="Edge{T}"/> objects are equivalent.
    /// </summary>
    /// <param name="left">The left <see cref="Edge{T}"/>.</param>
    /// <param name="right">The right <see cref="Edge{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal;
    /// otherwise <c>false</c>.
    /// </returns>
    public static bool operator ==(Edge<T>? left, Edge<T>? right)
    {
        if (left is null)
        {
            return right is null;
        }
        return left.Equals(right);
    }

    /// <summary>
    /// Checks whether two <see cref="Edge{T}"/> objects differ.
    /// </summary>
    /// <param name="left">The left <see cref="Edge{T}"/>.</param>
    /// <param name="right">The right <see cref="Edge{T}"/>.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/>
    /// are not equal; otherwise <c>false</c>.
    /// </returns>
    public static bool operator !=(Edge<T>? left, Edge<T>? right)
    {
        return !(left == right);
    }

    #endregion Operators
}
