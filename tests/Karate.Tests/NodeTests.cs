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
        public void GetIdFromName_ShouldReturnCorrectId()
        {
            // Arrange
            var nodeName = "LookupNode";
            var node = new Node(nodeName);

            // Act
            var result = Node.GetIdFromName(nodeName);

            // Assert
            Assert.AreEqual(node.Id, result);
        }

        [TestMethod]
        public void GetNameFromId_ShouldReturnCorrectName()
        {
            // Arrange
            var nodeName = "ReverseLookupNode";
            var node = new Node(nodeName);

            // Act
            var result = Node.GetNameFromId(node.Id);

            // Assert
            Assert.AreEqual(nodeName, result);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrueForSameId()
        {
            // Arrange
            var node1 = new Node("EqualsNode1");
            var node2 = new Node("EqualsNode2");

            // Act
            bool areEqual = node1 == node1;
            bool areNotEqual = node1 == node2;

            // Assert
            Assert.IsTrue(areEqual);
            Assert.IsFalse(areNotEqual);
        }
    }
}