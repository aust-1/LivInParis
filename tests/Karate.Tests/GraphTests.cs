namespace Karate.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void AddNode_ShouldIncreaseOrder()
        {
            // Arrange
            Graph graph = new Graph();
            int initialOrder = graph.Order;
            Node node = new Node("GraphNode");

            // Act
            graph.AddNode(node);

            // Assert
            Assert.AreEqual(initialOrder + 1, graph.Order);
        }

        [TestMethod]
        public void AddEdge_ShouldIncreaseSize()
        {
            // Arrange
            Graph graph = new Graph();
            Node n1 = new Node("NodeA");
            Node n2 = new Node("NodeB");
            graph.AddNode(n1);
            graph.AddNode(n2);
            int initialSize = graph.Size;

            // Act
            graph.AddEdge(new Edge(n1, n2));

            // Assert
            Assert.AreEqual(initialSize + 1, graph.Size);
        }

        [TestMethod]
        public void BFS_ShouldReturnVisitedNodesInOrder()
        {
            // Arrange
            Graph graph = new Graph();
            Node n1 = new Node("BFS_A");
            Node n2 = new Node("BFS_B");
            Node n3 = new Node("BFS_C");
            graph.AddNode(n1);
            graph.AddNode(n2);
            graph.AddNode(n3);
            graph.AddEdge(new Edge(n1, n2));
            graph.AddEdge(new Edge(n2, n3));

            // Act
            List<string> visited = graph.BFS(n1);

            // Assert
            CollectionAssert.AreEqual(new List<string> { "BFS_A", "BFS_B", "BFS_C" }, visited);
        }

        [TestMethod]
        public void DFSIterative_ShouldReturnVisitedNodesInOrder()
        {
            // Arrange
            Graph graph = new Graph();
            Node n1 = new Node("DFS_A");
            Node n2 = new Node("DFS_B");
            Node n3 = new Node("DFS_C");
            graph.AddNode(n1);
            graph.AddNode(n2);
            graph.AddNode(n3);
            graph.AddEdge(new Edge(n1, n2));
            graph.AddEdge(new Edge(n2, n3));

            // Act
            List<string> visited = graph.DFSIterative(n1);

            // Assert
            CollectionAssert.AreEqual(new List<string> { "DFS_A", "DFS_B", "DFS_C" }, visited);
        }

        [TestMethod]
        public void AdjacencyMatrix_ShouldBeCorrectlyBuilt()
        {
            // Arrange
            Graph graph = new Graph();
            Node n1 = new Node("Matrix_A");
            Node n2 = new Node("Matrix_B");
            graph.AddNode(n1);
            graph.AddNode(n2);
            graph.AddEdge(new Edge(n1, n2, 1.5));

            // Act
            double[,]? matrix = graph.AdjacencyMatrix;

            // Assert
            Assert.IsNotNull(matrix);
            Assert.AreEqual(1.5, matrix[n1.Id, n2.Id]);
            Assert.AreEqual(1.5, matrix[n2.Id, n1.Id]); // Undirected by default
        }
    }
}