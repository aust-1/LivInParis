namespace Karate.Models;

/// <summary>
/// Represents an edge (or arc) connecting two nodes in a graph.
/// </summary>
/// <remarks>
/// <para>
/// In an undirected graph, the target node is simply the other endpoint,
/// and reversing the source and target is considered equivalent.
/// </para>
/// <para>
/// In a weighted graph, the <see cref="Weight"/> can store cost, distance, capacity, or any other metric.
/// </para>
/// </remarks>
public sealed class Edge<T> : IEquatable<Edge<T>>
{
    #region Fields

    /// <summary>
    /// The source node of this edge.
    /// </summary>
    private readonly Node<T> _sourceNode;

    /// <summary>
    /// The target node of this edge.
    /// </summary>
    private readonly Node<T> _targetNode;

    /// <summary>
    /// The weight of the edge, with a default of 1.0.
    /// </summary>
    private readonly double _weight;

    /// <summary>
    /// Indicates whether this edge is directed (true) or undirected (false).
    /// </summary>
    private readonly bool _isDirected;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Edge"/> class.
    /// </summary>
    /// <param name="sourceNode">The source node of this edge.</param>
    /// <param name="targetNode">The target node of this edge.</param>
    /// <param name="weight">The weight of this edge (default is 1.0).</param>
    /// <param name="isDirected">
    /// <c>true</c> if the edge is directed; <c>false</c> if it is undirected (default is false).
    /// </param>
    public Edge(
        Node<T> sourceNode,
        Node<T> targetNode,
        double weight = 1.0,
        bool isDirected = false
    )
    {
        _sourceNode = sourceNode;
        _targetNode = targetNode;
        _weight = weight;
        _isDirected = isDirected;
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
    /// Gets the weight associated with this edge.
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

    #endregion Properties

    #region Methods

    /// <summary>
    /// Returns a string representation of this edge.
    /// </summary>
    /// <remarks>
    /// In a directed context, this takes the form: Source --(Weight)--> Target.
    /// </remarks>
    /// <returns>A string describing the source, target, and weight.</returns>
    public override string ToString()
    {
        return $"Edge: {_sourceNode.Id} --({_weight})--> {_targetNode.Id}";
    }

    #endregion Methods

    #region IEquatable<Edge> Implementation

    /// <summary>
    /// Determines whether the specified <see cref="Edge"/> is equal to the current <see cref="Edge"/>.
    /// </summary>
    /// <param name="other">The other <see cref="Edge"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="Edge"/> is equal to this instance; otherwise, <c>false</c>.
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
            && Equals(_weight, other._weight);

        bool reversedOrientation =
            !_isDirected
            && _sourceNode.Equals(other._targetNode)
            && _targetNode.Equals(other._sourceNode)
            && Equals(_weight, other._weight);

        return sameOrientation || reversedOrientation;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Edge"/>.
    /// </summary>
    /// <param name="obj">The object to compare with this edge.</param>
    /// <returns>
    /// <c>true</c> if the specified object is an <see cref="Edge"/> and is considered equal;
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
    /// Returns the hash code for this <see cref="Edge"/>.
    /// </summary>
    /// <remarks>
    /// Combines the hash codes of the source node, target node,
    /// weight, and the directedness flag.
    /// </remarks>
    /// <returns>An integer hash code.</returns>
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

    #endregion IEquatable<Edge> Implementation

    #region Operators

    /// <summary>
    /// Checks equality of two <see cref="Edge"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="Edge"/>.</param>
    /// <param name="right">The right <see cref="Edge"/>.</param>
    /// <returns><c>true</c> if the edges are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Edge<T>? left, Edge<T>? right)
    {
        if (left is null)
        {
            return right is null;
        }
        return left.Equals(right);
    }

    /// <summary>
    /// Checks inequality of two <see cref="Edge"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="Edge"/>.</param>
    /// <param name="right">The right <see cref="Edge"/>.</param>
    /// <returns><c>true</c> if the edges are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Edge<T>? left, Edge<T>? right)
    {
        return !(left == right);
    }

    #endregion Operators
}
