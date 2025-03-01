using System;
using System.Linq;
using Karate.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karate.Tests
{
    [TestClass]
    public class NodeTests
    {
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
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShouldThrowExceptionForDuplicateName()
        {
            var node1 = new Node("Node1");
            var node2 = new Node("Node1");
        }

        [TestMethod]
        public void GetOrCreateNode_ByName_ShouldReturnExistingNode()
        {
            var newNode = new Node("newNode");
            var retrievedNode = Node.GetOrCreateNode("newNode");

            Assert.AreEqual(newNode, retrievedNode);
        }

        [TestMethod]
        public void GetOrCreateNode_ByName_ShouldCreateNewNodeIfNotFound()
        {
            var newNode = Node.GetOrCreateNode("Node1");

            Assert.AreEqual("Node1", newNode.Name);
        }

        [TestMethod]
        public void GetOrCreateNode_ById_ShouldReturnExistingNode()
        {
            var newNewNode = new Node("newNewNode");
            var retrievedNode = Node.GetOrCreateNode(newNewNode.Id);

            Assert.AreEqual(newNewNode, retrievedNode);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnZeroForSameNode()
        {
            var node1 = Node.GetOrCreateNode("Node1");

            Assert.AreEqual(0, node1.CompareTo(node1));
        }

        [TestMethod]
        public void CompareTo_ShouldReturnNegativeForSmallerId()
        {
            var node1 = Node.GetOrCreateNode("Node1");
            var node2 = Node.GetOrCreateNode("Node2");

            Assert.IsTrue(node1.CompareTo(node2) < 0);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnPositiveForLargerId()
        {
            var node1 = Node.GetOrCreateNode("Node1");
            var node2 = Node.GetOrCreateNode("Node2");

            Assert.AreEqual(node2.CompareTo(node1) > 0, node2.Id.CompareTo(node1.Id) > 0);
        }

        [TestMethod]
        public void EqualityOperator_ShouldReturnTrueForEqualNodes()
        {
            var node1 = Node.GetOrCreateNode("Node1");
            var node2 = Node.GetOrCreateNode("Node1");

            Assert.IsTrue(node1 == node2);
        }

        [TestMethod]
        public void InequalityOperator_ShouldReturnTrueForDifferentNodes()
        {
            var node1 = Node.GetOrCreateNode("Node1");
            var node2 = Node.GetOrCreateNode("Node2");

            Assert.IsTrue(node1 != node2);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            var node = Node.GetOrCreateNode("Node1");

            Assert.AreEqual("Node: Id=0, Name=Node1", node.ToString());
        }
    }
}
