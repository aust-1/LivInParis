namespace Karate.Models;

/// <summary>
/// Represents a node in a graph, identified by an integer ID and containing data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of data that this node holds. Must be unique among nodes within the static dictionary.
/// </typeparam>
/// <remarks>
/// <para>
/// A static dictionary (<see cref="_existingNodes"/>) maps integer IDs to node instances.
/// Attempting to create a node with duplicate data will result in an <see cref="ArgumentException"/>.
/// </para>
/// <para>
/// This static approach applies across the entire application domain, potentially causing collisions
/// if multiple graphs are created in the same process.
/// </para>
/// </remarks>
public class Node<T> : IComparable<Node<T>>
{
    #region Fields

    /// <summary>
    /// A static dictionary that associates each node's integer ID with the node instance.
    /// </summary>
    private static readonly SortedDictionary<int, Node<T>> _existingNodes = new();

    /// <summary>
    /// The unique integer ID for this node.
    /// </summary>
    private readonly int _id;

    /// <summary>
    /// The data stored in this node.
    /// </summary>
    private readonly T _data;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Node{T}"/> class with the specified data.
    /// Throws an <see cref="ArgumentException"/> if another node already holds the same data.
    /// </summary>
    /// <param name="data">
    /// The data to store in this node. Must be unique among all nodes in <see cref="_existingNodes"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if a node with the same data already exists.
    /// </exception>
    public Node(T data)
    {
        int nextId = _existingNodes.Count;

        if (_existingNodes.Values.Any(existingNode => existingNode.Data?.Equals(data) == true))
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
    /// Gets the unique integer ID for this node.
    /// </summary>
    public int Id
    {
        get { return _id; }
    }

    /// <summary>
    /// Gets the data stored in this node.
    /// </summary>
    public T Data
    {
        get { return _data; }
    }

    /// <summary>
    /// Gets the total number of nodes that have been created so far.
    /// </summary>
    /// <value>
    /// The current count of all nodes in <see cref="_existingNodes"/>.
    /// </value>
    public static int Count
    {
        get { return _existingNodes.Count; }
    }

    #endregion Properties

    #region Static Methods

    /// <summary>
    /// Retrieves an existing node with the specified data,
    /// or creates a new one if none is found.
    /// </summary>
    /// <param name="dataToFind">The data to locate or assign to a new node.</param>
    /// <returns>
    /// The existing node if found, or a newly created node if not.
    /// </returns>
    public static Node<T> GetOrCreateNode(T dataToFind)
    {
        foreach (var node in _existingNodes.Values)
        {
            if (node.Data != null && node.Data.Equals(dataToFind))
            {
                return node;
            }
        }
        return new Node<T>(dataToFind);
    }

    /// <summary>
    /// Retrieves a node by its integer ID.
    /// </summary>
    /// <param name="idToFind">The ID of the node to retrieve.</param>
    /// <returns>The <see cref="Node{T}"/> with the specified ID.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if no node with the specified ID is found in <see cref="_existingNodes"/>.
    /// </exception>
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
    /// Returns a string that represents the current node, including its ID and data.
    /// </summary>
    /// <returns>A string in the format: "Node: Id={ID}, Data={Data}".</returns>
    public override string ToString()
    {
        return $"Node: Id={_id}, Data={_data}";
    }

    #endregion Public Methods

    #region IComparable<Node<T>> Implementation

    /// <summary>
    /// Compares this node to another <see cref="Node{T}"/> by their IDs.
    /// </summary>
    /// <param name="other">Another node to compare with this instance.</param>
    /// <returns>
    /// A negative number if this node's ID is less than <paramref name="other"/>'s ID,
    /// zero if the IDs are the same, or a positive number if this node's ID is greater.
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
    /// Determines whether the specified object is equal to this node, comparing by ID.
    /// </summary>
    /// <param name="obj">Another object to compare with this node.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="obj"/> is a <see cref="Node{T}"/> with the same ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj is null)
        {
            return false;
        }

        return obj is Node<T> other && other._id == _id;
    }

    /// <summary>
    /// Returns a hash code for this node, based on its unique ID.
    /// </summary>
    /// <returns>An integer hash code that reflects this node's ID.</returns>
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
    /// A negative number if <paramref name="left"/>'s ID is less than <paramref name="right"/>'s ID,
    /// zero if the IDs are equal, or a positive number if it is greater.
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
    /// <c>true</c> if both nodes have the same ID or are both <c>null</c>; otherwise, <c>false</c>.
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
    /// <c>true</c> if they differ in ID or only one is <c>null</c>; otherwise <c>false</c>.
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
    /// <c>true</c> if <paramref name="left"/>'s ID is greater than or equal to <paramref name="right"/>'s ID;
    /// otherwise, <c>false</c>.
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
    /// <c>true</c> if <paramref name="left"/>'s ID is less than or equal to <paramref name="right"/>'s ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <=(Node<T> left, Node<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    #endregion IComparable<Node<T>> Implementation
}
