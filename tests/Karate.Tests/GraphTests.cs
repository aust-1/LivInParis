// namespace Karate.Tests
// {
//     [TestClass]
//     public class GraphTests
//     {
//         private Node<string> _node1 = Node<string>.GetOrCreateNode("Node1");
//         private Node<string> _node2 = Node<string>.GetOrCreateNode("Node2");
//         private Node<string> _node3 = Node<string>.GetOrCreateNode("Node3");

//         [TestInitialize]
//         public void TestInitialize()
//         {
//             typeof(Node<string>)
//                 .GetField(
//                     "ExistingNodes",
//                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
//                 )
//                 ?.SetValue(null, new SortedDictionary<int, Node<string>>());
//         }

//         [TestMethod]
//         public void Constructor_ShouldInitializeGraphCorrectly()
//         {
//             var graph = new Graph<string>();

//             Assert.IsNotNull(graph.Nodes);
//             Assert.IsNotNull(graph.Edges);
//             Assert.IsNotNull(graph.AdjacencyList);
//             Assert.IsFalse(graph.IsDirected);
//         }

//         [TestMethod]
//         public void Constructor_WithAdjacencyList_ShouldInitializeGraphCorrectly()
//         {
//             var adjacencyList = new SortedDictionary<Node, SortedSet<Node>>
//             {
//                 {
//                     _node1,
//                     new SortedSet<Node> { _node2 }
//                 },
//                 {
//                     _node2,
//                     new SortedSet<Node> { _node3 }
//                 },
//                 {
//                     _node3,
//                     new SortedSet<Node> { _node1 }
//                 },
//             };

//             var graph = new Graph<string>(adjacencyList);

//             Assert.AreEqual(3, graph.Nodes.Count);
//             Assert.AreEqual(3, graph.Edges.Count);
//             Assert.IsTrue(graph.IsDirected);
//         }

//         [TestMethod]
//         public void Constructor_WithAdjacencyMatrix_ShouldInitializeGraphCorrectly()
//         {
//             double[,] adjacencyMatrix =
//             {
//                 { 0, 1, 0 },
//                 { 0, 0, 1 },
//                 { 1, 0, 0 },
//             };

//             var graph = new Graph<string>(adjacencyMatrix);

//             Assert.AreEqual(3, graph.Nodes.Count);
//             Assert.AreEqual(3, graph.Edges.Count);
//             Assert.IsTrue(graph.IsDirected);
//         }

//         [TestMethod]
//         public void AddNode_ShouldAddNodeToGraph()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);

//             Assert.AreEqual(1, graph.Nodes.Count);
//             Assert.IsTrue(graph.Nodes.Contains(_node1));
//         }

//         [TestMethod]
//         public void AddEdge_ShouldAddEdgeToGraph()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             var edge = new Edge(_node1, _node2);

//             graph.AddEdge(edge);

//             Assert.AreEqual(1, graph.Edges.Count);
//             Assert.IsTrue(graph.Edges.Contains(edge));
//         }

//         [TestMethod]
//         public void BuildAdjacencyMatrix_ShouldBuildCorrectMatrix()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             var edge = new Edge(_node1, _node2);

//             graph.AddEdge(edge);
//             graph.BuildAdjacencyMatrix();

//             var matrix = graph.AdjacencyMatrix;
//             Assert.IsNotNull(matrix);
//             Assert.AreEqual(1.0, matrix[_node1.Id, _node2.Id]);
//         }

//         [TestMethod]
//         public void FindAnyCycle_ShouldReturnCycleIfExists()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddNode(_node3);
//             graph.AddEdge(new Edge(_node1, _node2));
//             graph.AddEdge(new Edge(_node2, _node3));
//             graph.AddEdge(new Edge(_node3, _node1));

//             var cycle = graph.FindAnyCycle();

//             Assert.IsNotNull(cycle);
//         }

//         [TestMethod]
//         public void BFS_ShouldReturnCorrectOrder()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddNode(_node3);
//             graph.AddEdge(new Edge(_node1, _node2));
//             graph.AddEdge(new Edge(_node2, _node3));

//             var bfsResult = graph.BFS(_node1);

//             CollectionAssert.AreEqual(new List<string> { "Node1", "Node2", "Node3" }, bfsResult);
//         }

//         [TestMethod]
//         public void DFSRecursive_ShouldReturnCorrectOrder()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddNode(_node3);
//             graph.AddEdge(new Edge(_node1, _node2));
//             graph.AddEdge(new Edge(_node2, _node3));

//             var dfsResult = graph.DFSRecursive(_node1);

//             CollectionAssert.AreEqual(new List<string> { "Node1", "Node2", "Node3" }, dfsResult);
//         }

//         [TestMethod]
//         public void DFSIterative_ShouldReturnCorrectOrder()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddNode(_node3);
//             graph.AddEdge(new Edge(_node1, _node2));
//             graph.AddEdge(new Edge(_node2, _node3));

//             var dfsResult = graph.DFSIterative(_node1);

//             CollectionAssert.AreEqual(new List<string> { "Node1", "Node2", "Node3" }, dfsResult);
//         }

//         [TestMethod]
//         public void Order_ShouldReturnCorrectNumberOfNodes()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);

//             Assert.AreEqual(2, graph.Order);
//         }

//         [TestMethod]
//         public void Size_ShouldReturnCorrectNumberOfEdges()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddEdge(new Edge(_node1, _node2));

//             Assert.AreEqual(1, graph.Size);
//         }

//         [TestMethod]
//         public void Density_ShouldReturnCorrectValue()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddEdge(new Edge(_node1, _node2));

//             double expectedDensity = 1.0 / 1.0; // (E) / (V * (V - 1)) for directed graphs
//             Assert.AreEqual(expectedDensity, graph.Density);
//         }

//         [TestMethod]
//         public void IsDirected_ShouldReturnCorrectValue()
//         {
//             var directedGraph = new Graph<string>(true);
//             var undirectedGraph = new Graph<string>(false);

//             Assert.IsTrue(directedGraph.IsDirected);
//             Assert.IsFalse(undirectedGraph.IsDirected);
//         }

//         [TestMethod]
//         public void IsWeighted_ShouldReturnCorrectValue()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddEdge(new Edge(_node1, _node2, 2.0, true));

//             Assert.IsTrue(graph.IsWeighted);
//         }

//         [TestMethod]
//         public void IsConnected_ShouldReturnTrueForConnectedGraph()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);
//             graph.AddEdge(new Edge(_node1, _node2));

//             Assert.IsTrue(graph.IsConnected);
//         }

//         [TestMethod]
//         public void IsConnected_ShouldReturnFalseForDisconnectedGraph()
//         {
//             var graph = new Graph<string>();
//             graph.AddNode(_node1);
//             graph.AddNode(_node2);

//             Assert.IsFalse(graph.IsConnected);
//         }
//     }
// }
