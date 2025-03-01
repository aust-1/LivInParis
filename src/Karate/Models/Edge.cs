namespace Karate.Models
{
    /// <summary>
    /// Represents an edge (or arc) between two nodes in a graph.
    /// </summary>
    /// <remarks>
    /// - In an undirected graph, the target node is simply the other endpoint.
    /// - In a weighted graph, the <see cref="Weight"/> can be used to store the cost or capacity of the edge.
    /// </remarks>
    public sealed class Edge : IEquatable<Edge>
    {
        #region Fields

        /// <summary>
        /// The source node.
        /// </summary>
        private readonly Node _sourceNode;

        /// <summary>
        /// The target node.
        /// </summary>
        private readonly Node _targetNode;

        /// <summary>
        /// The weight of the edge (default is 1.0).
        /// </summary>
        private readonly double _weight;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="sourceNode">The source node.</param>
        /// <param name="targetNode">The target node.</param>
        /// <param name="weight">The weight of this edge (default is 1.0).</param>
        public Edge(Node sourceNode, Node targetNode, double weight = 1.0)
        {
            _sourceNode = sourceNode;
            _targetNode = targetNode;
            _weight = weight;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the source node of this edge.
        /// </summary>
        /// <value>The source node.</value>
        public Node SourceNode
        {
            get { return _sourceNode; }
        }

        /// <summary>
        /// Gets the target node of this edge.
        /// </summary>
        /// <value>The target node.</value>
        public Node TargetNode
        {
            get { return _targetNode; }
        }

        /// <summary>
        /// Gets the weight of this edge.
        /// </summary>
        /// <value>The weight.</value>
        public double Weight
        {
            get { return _weight; }
        }

        #endregion Properties

        #region Methods

        public bool Equals(Edge? other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (
                _sourceNode.Equals(other._sourceNode)
                && _targetNode.Equals(other._targetNode)
                && _weight == other._weight
            )
            {
                return true;
            }
            if (
                _sourceNode.Equals(other._targetNode)
                && _targetNode.Equals(other._sourceNode)
                && _weight == other._weight
            )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current edge.
        /// </summary>
        /// <returns>A string representation of this edge.</returns>
        public override string ToString()
        {
            return $"Edge: {_sourceNode.Id} --({_weight})--> {_targetNode.Id}";
        }

        #endregion Methods
    }
}
