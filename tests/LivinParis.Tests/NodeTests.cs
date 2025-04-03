namespace LivinParisRoussilleTeynier.Tests
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
    }
}
