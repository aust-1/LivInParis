namespace LivinParis.Tests
{
    [TestClass]
    public class GraphTests
    {
        private Node<string> _node1 = Node<string>.GetOrCreateNode("Node1");
        private Node<string> _node2 = Node<string>.GetOrCreateNode("Node2");
        private Node<string> _node3 = Node<string>.GetOrCreateNode("Node3");

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
    }
}
