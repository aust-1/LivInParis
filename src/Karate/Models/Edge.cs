namespace Karate.Models
{
    /// <summary>
    /// Represents an edge (or arc) between two nodes in a graph.
    /// </summary>
    /// <remarks>
    /// - In an undirected graph, the target node is simply the other endpoint.
    /// - In a weighted graph, the <see cref="Weight"/> can be used to store the cost or capacity of the edge.
    /// </remarks>
    public class Edge
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
        public Node SourceNode
        {
            get { return _sourceNode; }
        }

        /// <summary>
        /// Gets the target node of this edge.
        /// </summary>
        public Node TargetNode
        {
            get { return _targetNode; }
        }

        /// <summary>
        /// Gets the weight of this edge.
        /// </summary>
        public double Weight
        {
            get { return _weight; }
        }

        #endregion Properties

        #region Methods

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