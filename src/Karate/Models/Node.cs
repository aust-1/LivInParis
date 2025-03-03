namespace Karate.Models;

/// <summary>
/// Represents a node in a graph.
/// </summary>
/// <remarks>
/// <para>
/// A static dictionary (<see cref="_existingNodes"/>) maps an integer ID to
/// the corresponding node instance. Node names must be unique. Attempting
/// to create a node with a duplicate name will result in an <see cref="ArgumentException"/>.
/// </para>
/// <para>
/// Because the dictionary is static, it applies across the entire application
/// domain. This may introduce collisions if you instantiate multiple graphs,
/// each requiring unique node sets.
/// </para>
/// </remarks>
public class Node<T> : IComparable<Node<T>>
{
    #region Fields

    /// <summary>
    /// Static dictionary mapping integer IDs to node instances.
    /// Ensures each node can be retrieved by ID.
    /// </summary>
    private static SortedDictionary<int, Node<T>> _existingNodes = new();

    /// <summary>
    /// The unique identifier for this node.
    /// </summary>
    private readonly int _id;

    /// <summary>
    /// The data of this node (must be unique among all nodes).
    /// </summary>
    private readonly T _data;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class with the specified name.
    /// If the name is empty or whitespace, a name is auto-generated based on the next ID.
    /// </summary>
    /// <param name="data">
    /// The data of this node. Must be unique among all nodes;
    /// otherwise, an <see cref="ArgumentException"/> is thrown.
    /// </param>
    /// <exception cref="ArgumentException">Thrown if the data is already in use by another node.</exception>
    public Node(T data)
    {
        int nextId = _existingNodes.Count;

        if (_existingNodes.Any(kvp => kvp.Value.Data?.Equals(data) == true))
        {
            throw new ArgumentException($"A node with the data '{data}' already exists.");
        }

        _data = data;
        _id = nextId;
        _existingNodes.Add(_id, this);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the unique identifier for this node.
    /// </summary>
    public int Id
    {
        get { return _id; }
    }

    /// <summary>
    /// Gets the data of this node.
    /// </summary>
    public T Data
    {
        get { return _data; }
    }

    /// <summary>
    /// Gets the total number of nodes that have been created so far.
    /// </summary>
    public static int Count
    {
        get { return _existingNodes.Count; }
    }

    #endregion Properties

    #region Static Methods

    /// <summary>
    /// Retrieves a node by its data, or creates a new node if none is found with that data.
    /// </summary>
    /// <param name="dataToFind">The data of the node to retrieve or create.</param>
    /// <returns>
    /// The existing node if one matches <paramref name="dataToFind"/>,
    /// otherwise a new node is created and returned.
    /// </returns>
    public static Node<T> GetOrCreateNode(T dataToFind)
    {
        foreach (Node<T> existingNode in _existingNodes.Values)
        {
            if (existingNode.Data != null && existingNode.Data.Equals(dataToFind))
            {
                return existingNode;
            }
        }

        return new Node<T>(dataToFind);
    }

    /// <summary>
    /// Retrieves a node by its ID.
    /// </summary>
    /// <param name="idToFind">The ID of the node to retrieve.</param>
    /// <returns>
    /// The existing node if one matches <paramref name="idToFind"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">Thrown if no node is found with the specified ID.</exception>
    public static Node<T> GetNode(int idToFind)
    {
        if (_existingNodes.TryGetValue(idToFind, out Node<T>? node))
        {
            return node;
        }

        throw new KeyNotFoundException($"Node with ID {idToFind} not found.");
    }

    #endregion Static Methods

    #region Public Methods

    /// <summary>
    /// Returns a string representation of the current node, including its ID and name.
    /// </summary>
    /// <returns>A string in the form "Node: Id={ID}, Name={Name}".</returns>
    public override string ToString()
    {
        return $"Node: Id={_id}, Data={_data}";
    }

    #endregion Public Methods

    #region IComparable<Node> Implementation

    /// <summary>
    /// Compares this node to another node by their IDs.
    /// </summary>
    /// <param name="other">The other <see cref="Node"/> to compare with.</param>
    /// <returns>
    /// A negative value if this node's ID is less than the other's ID,
    /// zero if the IDs are equal, or a positive value if this node's ID is greater.
    /// </returns>
    public int CompareTo(Node<T>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }
        if (other is null)
        {
            return 1;
        }

        return _id.CompareTo(other._id);
    }

    /// <summary>
    /// Determines whether the specified object is equal to this node,
    /// comparing by ID.
    /// </summary>
    /// <param name="obj">The object to compare with the current node.</param>
    /// <returns>
    /// <c>true</c> if the specified object is a <see cref="Node"/> with the same ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is Node<T> other && other._id == _id;
    }

    /// <summary>
    /// Gets the hash code for this node, based on its unique ID.
    /// </summary>
    /// <returns>An integer hash code derived from the node's ID.</returns>
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    /// <summary>
    /// Compares two nodes by their IDs.
    /// </summary>
    /// <param name="left">The first node to compare.</param>
    /// <param name="right">The second node to compare.</param>
    /// <returns>
    /// A negative number if <paramref name="left"/>'s ID is less than
    /// <paramref name="right"/>'s ID, zero if they have the same ID,
    /// or a positive number if it is greater.
    /// </returns>
    public static int Compare(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right);
    }

    /// <summary>
    /// Equality operator: checks whether two nodes have the same ID.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if both nodes have the same ID or are both <c>null</c>;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(Node<T>? left, Node<T>? right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator: checks whether two nodes have different IDs.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if they differ in ID or if only one is <c>null</c>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(Node<T>? left, Node<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Greater-than operator: compares two nodes by their IDs.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/>'s ID is greater than <paramref name="right"/>'s ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Less-than operator: compares two nodes by their IDs.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/>'s ID is less than <paramref name="right"/>'s ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Greater-than-or-equal operator: compares two nodes by their IDs.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/>'s ID is greater than or equal to
    /// <paramref name="right"/>'s ID; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >=(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Less-than-or-equal operator: compares two nodes by their IDs.
    /// </summary>
    /// <param name="left">The left node.</param>
    /// <param name="right">The right node.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/>'s ID is less than or equal to
    /// <paramref name="right"/>'s ID; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <=(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    #endregion IComparable<Node<T>> Implementation
}
