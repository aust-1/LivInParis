using System;
using System.Linq;
using LivinParis.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karate.Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            typeof(Node<string>)
                .GetField(
                    "ExistingNodes",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                )
                ?.SetValue(null, new SortedDictionary<int, Node<string>>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShouldThrowExceptionForDuplicateName()
        {
            var node1 = new Node<string>("Node1");
            var node2 = new Node<string>("Node1");
        }

        [TestMethod]
        public void GetOrCreateNode_ByName_ShouldReturnExistingNode()
        {
            var newNode = new Node<string>("newNode");
            var retrievedNode = Node<string>.GetOrCreateNode("newNode");

            Assert.AreEqual(newNode, retrievedNode);
        }

        [TestMethod]
        public void GetOrCreateNode_ByName_ShouldCreateNewNodeIfNotFound()
        {
            var newNode = Node<string>.GetOrCreateNode("Node1");

            Assert.AreEqual("Node1", newNode.Data);
        }

        [TestMethod]
        public void GetNode_ById_ShouldReturnExistingNode()
        {
            var newNewNode = new Node<string>("newNewNode");
            var retrievedNode = Node<string>.GetNode(newNewNode.Id);

            Assert.AreEqual(newNewNode, retrievedNode);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnZeroForSameNode()
        {
            var node1 = Node<string>.GetOrCreateNode("Node1");

            Assert.AreEqual(0, node1.CompareTo(node1));
        }

        [TestMethod]
        public void CompareTo_ShouldReturnNegativeForSmallerId()
        {
            var node1 = Node<string>.GetOrCreateNode("Node1");
            var node2 = Node<string>.GetOrCreateNode("Node2");

            Assert.IsTrue(node1.CompareTo(node2) < 0);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnPositiveForLargerId()
        {
            var node1 = Node<string>.GetOrCreateNode("Node1");
            var node2 = Node<string>.GetOrCreateNode("Node2");

            Assert.AreEqual(node2.CompareTo(node1) > 0, node2.Id.CompareTo(node1.Id) > 0);
        }

        [TestMethod]
        public void EqualityOperator_ShouldReturnTrueForEqualNodes()
        {
            var node1 = Node<string>.GetOrCreateNode("Node1");
            var node2 = Node<string>.GetOrCreateNode("Node1");

            Assert.IsTrue(node1 == node2);
        }

        [TestMethod]
        public void InequalityOperator_ShouldReturnTrueForDifferentNodes()
        {
            var node1 = Node<string>.GetOrCreateNode("Node1");
            var node2 = Node<string>.GetOrCreateNode("Node2");

            Assert.IsTrue(node1 != node2);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            var node = Node<string>.GetOrCreateNode("Node1");

            Assert.AreEqual("Node<string>: Id=0, Name=Node1", node.ToString());
        }
    }
}
