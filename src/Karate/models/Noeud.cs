namespace Karate.Models
{
    /// <summary>
    /// Represents a node in the graph.
    /// </summary>
    /// <remarks>
    /// A static dictionary (<see cref="_existingNodes"/>) is maintained to ensure that each node name is unique.
    /// If a node with the given name already exists, the constructor will throw an <see cref="ArgumentException"/>.
    /// </remarks>
    public class Node : IComparable<Node>
    {
        #region Fields

        /// <summary>
        /// Static dictionary mapping node names to their unique IDs.
        /// </summary>
        private static readonly SortedDictionary<string, int> _existingNodes = new();

        /// <summary>
        /// The unique identifier for this node.
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// The name of this node.
        /// </summary>
        private readonly string _name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of this node. Must be unique among all nodes;
        /// otherwise an <see cref="ArgumentException"/> is thrown.
        /// </param>
        public Node(string name = "")
        {
            int nextId = _existingNodes.Count;
            if (!_existingNodes.TryAdd(name, nextId))
            {
                throw new ArgumentException($"A node with the name '{name}' already exists.");
            }

            _name = name;
            _id = nextId;
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a string that represents the current node.
        /// </summary>
        /// <returns>A string containing the node's ID and name.</returns>
        public override string ToString()
        {
            return $"Node: Id={_id}, Name={_name}";
        }

        /// <summary>
        /// Retrieves the ID associated with the specified node name.
        /// </summary>
        /// <param name="nameToFind">The name of the node whose ID you want to retrieve.</param>
        /// <returns>
        /// The node's ID if found; otherwise, -1 if no node with the given name exists.
        /// </returns>
        public static int GetIdFromName(string nameToFind)
        {
            return _existingNodes.TryGetValue(nameToFind, out int id) ? id : -1;
        }

        /// <summary>
        /// Retrieves the node name associated with the specified ID.
        /// </summary>
        /// <param name="idToFind">The ID of the node whose name you want to retrieve.</param>
        /// <returns>
        /// The node's name if found; otherwise, <c>null</c> if no node with the given ID exists.
        /// </returns>
        public static string? GetNameFromId(int idToFind)
        {
            return _existingNodes.FirstOrDefault(x => x.Value == idToFind).Key;
        }

        #endregion Methods

        #region IComparable<Node> Implementation

        /// <summary>
        /// Compares this node to another object by their IDs.
        /// </summary>
        /// <param name="other">Another object, preferably a <see cref="Node"/>.</param>
        /// <returns>
        /// A negative value if this node's ID is less than <paramref name="other"/>'s ID,
        /// zero if equal, or a positive value if greater.
        /// If <paramref name="other"/> is not a <see cref="Node"/>, returns 1 by default.
        /// </returns>
        public int CompareTo(object? other)
        {
            if (other is Node otherNode)
            {
                return _id.CompareTo(otherNode._id);
            }
            return 1;
        }

        /// <summary>
        /// Compares this node to another node by their IDs.
        /// </summary>
        /// <param name="other">Another <see cref="Node"/>.</param>
        /// <returns>
        /// A negative value if this node's ID is less than <paramref name="other"/>'s ID,
        /// zero if equal, or a positive value if greater.
        /// </returns>
        public int CompareTo(Node? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;

            return _id.CompareTo(other._id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current node.
        /// </summary>
        /// <param name="obj">Another object to compare with.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> is a <see cref="Node"/> with the same ID;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Node other && other._id == _id;
        }

        /// <summary>
        /// Gets the hash code for this node (based on its ID).
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return _id;
        }

        /// <summary>
        /// Compares two nodes by their IDs.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        /// <returns>
        /// A negative number if <paramref name="left"/>'s ID is less than <paramref name="right"/>'s ID;
        /// zero if they're equal; a positive number if greater.
        /// </returns>
        public static int Compare(Node left, Node right)
        {
            return left.CompareTo(right);
        }

        /// <summary>
        /// Equality operator: checks whether two nodes are equal by comparing their IDs.
        /// </summary>
        public static bool operator ==(Node? left, Node? right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        /// <summary>
        /// Greater-than operator: compares two nodes by their IDs.
        /// </summary>
        public static bool operator >(Node left, Node right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Less-than operator: compares two nodes by their IDs.
        /// </summary>
        public static bool operator <(Node left, Node right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Inequality operator: checks whether two nodes have different IDs.
        /// </summary>
        public static bool operator !=(Node? left, Node? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Greater-than-or-equal operator: compares two nodes by their IDs.
        /// </summary>
        public static bool operator >=(Node left, Node right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Less-than-or-equal operator: compares two nodes by their IDs.
        /// </summary>
        public static bool operator <=(Node left, Node right)
        {
            return Compare(left, right) <= 0;
        }

        #endregion IComparable<Node> Implementation
    }
}