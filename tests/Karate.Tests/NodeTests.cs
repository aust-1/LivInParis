namespace Karate.Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void Constructor_ShouldCreateNodeWithUniqueName()
        {
            // Arrange
            var nodeName = "TestNode";

            // Act
            var node = new Node(nodeName);

            // Assert
            Assert.AreEqual(nodeName, node.Name);
            Assert.AreNotEqual(-1, node.Id);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShouldThrowExceptionForDuplicateName()
        {
            // Arrange
            var nodeName = "DuplicateNode";
            _ = new Node(nodeName);

            // Act / Assert
            _ = new Node(nodeName);
        }

        [TestMethod]
        public void Constructor_ShouldCreateNodeWithIncreaseId()
        {
            // Arrange
            var nodeName1 = "UniqueNode1";
            var nodeName2 = "UniqueNode2";
            var nodeName3 = "UniqueNode3";
            
            // Act
            var node1 = new Node(nodeName1);
            var node2 = new Node(nodeName2);
            var node3 = new Node(nodeName3);
            
            // Assert
            Assert.AreEqual(node1.Id+1, node2.Id);
            Assert.AreEqual(node1.Id+2, node3.Id);
        }
        
        [TestMethod]
        public void Constructor_ShouldCreateNodeWithDefaultName()
        {
            // Arrange
            var nodeName = "Node 0";
            
            // Act
            var node = new Node();
            
            // Assert
            Assert.AreEqual(nodeName, node.Name);
        }
        
        [TestMethod]
        public void Id_ShouldBeUnique()
        {
            // Arrange
            var nodeName1 = "UniqueNode4";
            var nodeName2 = "UniqueNode5";
            
            // Act
            var node1 = new Node(nodeName1);
            var node2 = new Node(nodeName2);
            
            // Assert
            Assert.AreNotEqual(node1.Id, node2.Id);
        }
        
        [TestMethod]
        public void Name_ShouldNotBeAltered()
        {
            // Arrange
            var nodeName = "UniqueNode6";
            
            // Act
            var node = new Node(nodeName);
            
            // Assert
            Assert.AreEqual(nodeName, node.Name);
        }

        [TestMethod]
        public void GetOrCreateNode_ShouldReturnCorrectNodeFromId()
        {
            // Arrange
            var node = new Node();

            // Act
            var result = Node.GetOrCreateNode(node.Id);

            // Assert
            Assert.AreEqual(node, result);
        }

        [TestMethod]
        public void GetOrCreateNode_ShouldReturnCorrectNodeFromName()
        {
            // Arrange
            var nodeName = "nodeName";
            var node = new Node(nodeName);

            // Act
            var result = Node.GetOrCreateNode(nodeName);

            // Assert
            Assert.AreEqual(node, result);
        }
        
        [TestMethod]
        public void GetOrCreateNode_ShouldCreateNewNodeIfNotFoundId()
        {
            // Arrange
            var nodeId = 0;

            // Act
            var result = Node.GetOrCreateNode(nodeId);

            // Assert
            Assert.AreEqual(nodeId, result.Id);
        }
        
        [TestMethod]
        public void GetOrCreateNode_ShouldCreateNewNodeIfNotFoundName()
        {
            // Arrange
            var nodeName = "nodeName";

            // Act
            var result = Node.GetOrCreateNode(nodeName);

            // Assert
            Assert.AreEqual(nodeName, result.Name);
        }
        
        [TestMethod]
        public void ToString_ShouldReturnTheDescriptionOfTheNode()
        {
            // Arrange
            var nodeName = "nodeName";
            var expected = $"Node: Id=0, Name={nodeName}";
            var node = new Node(nodeName);

            // Act
            var result = node.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}