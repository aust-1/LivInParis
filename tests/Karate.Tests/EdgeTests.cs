namespace Karate.Tests
{
    [TestClass]
    public class EdgeTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeEdgeProperties()
        {
            // Arrange
            var source = new Node("SourceNode");
            var target = new Node("TargetNode");
            double weight = 2.5;

            // Act
            var edge = new Edge(source, target, weight);

            // Assert
            Assert.AreEqual(source, edge.SourceNode);
            Assert.AreEqual(target, edge.TargetNode);
            Assert.AreEqual(weight, edge.Weight);
        }

        [TestMethod]
        public void ToString_ShouldReturnEdgeRepresentation()
        {
            // Arrange
            var source = new Node("FromNode");
            var target = new Node("ToNode");
            var edge = new Edge(source, target);

            // Act
            var result = edge.ToString();

            // Assert
            Assert.IsTrue(result.Contains(source.Id.ToString()));
            Assert.IsTrue(result.Contains(target.Id.ToString()));
        }
    }
}