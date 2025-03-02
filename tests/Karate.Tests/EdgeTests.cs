using System;
using Karate.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karate.Tests
{
    [TestClass]
    public class EdgeTests
    {
        private readonly Node _node1 = Node.GetOrCreateNode("Node1");
        private readonly Node _node2 = Node.GetOrCreateNode("Node2");

        [TestInitialize]
        public void TestInitialize()
        {
            typeof(Node)
                .GetField(
                    "ExistingNodes",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                )
                ?.SetValue(null, new SortedDictionary<int, Node>());
        }

        [TestMethod]
        public void Constructor_ShouldInitializeEdgeCorrectly()
        {
            var edge = new Edge(_node1, _node2, 2.5, true);

            Assert.AreEqual(_node1, edge.SourceNode);
            Assert.AreEqual(_node2, edge.TargetNode);
            Assert.AreEqual(2.5, edge.Weight);
            Assert.IsTrue(edge.IsDirected);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrueForSameEdge()
        {
            var edge1 = new Edge(_node1, _node2);
            var edge2 = new Edge(_node1, _node2);

            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void Equals_ShouldReturnFalseForDifferentEdges()
        {
            var edge1 = new Edge(_node1, _node2, isDirected: true);
            var edge2 = new Edge(_node2, _node1, isDirected: true);

            Assert.IsFalse(edge1.Equals(edge2));
        }

        [TestMethod]
        public void Equals_ShouldReturnTrueForUndirectedEdgesWithReversedNodes()
        {
            var edge1 = new Edge(_node1, _node2, 1.0, false);
            var edge2 = new Edge(_node2, _node1, 1.0, false);

            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnSameHashCodeForEqualEdges()
        {
            var edge1 = new Edge(_node1, _node2);
            var edge2 = new Edge(_node1, _node2);

            Assert.AreEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }

        [TestMethod]
        public void EqualityOperator_ShouldReturnTrueForEqualEdges()
        {
            var edge1 = new Edge(_node1, _node2);
            var edge2 = new Edge(_node1, _node2);

            Assert.IsTrue(edge1 == edge2);
        }

        [TestMethod]
        public void InequalityOperator_ShouldReturnTrueForDifferentEdges()
        {
            var edge1 = new Edge(_node1, _node2, isDirected: true);
            var edge2 = new Edge(_node2, _node1);

            Assert.IsTrue(edge1 != edge2);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            var edge = new Edge(_node1, _node2, 2.5, true);

            Assert.AreEqual($"Edge: {_node1.Id} --(2,5)--> {_node2.Id}", edge.ToString());
        }
    }
}
