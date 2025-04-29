namespace LivInParisRoussilleTeynier.Tests;

[TestClass]
public class GraphTests
{
    private Graph<string> _graph = null!;

    [TestInitialize]
    public void Init()
    {
        var a = new Node<string>(0, "A", 0, 0, "", "");
        var b = new Node<string>(1, "B", 0.0, 0.0, "", "");
        var c = new Node<string>(2, "C", 0.0, 0.0, "", "");
        var d = new Node<string>(3, "D", 0.0, 0.0, "", "");

        var adjacencyList = new SortedDictionary<
            Node<string>,
            SortedDictionary<Node<string>, double>
        >
        {
            [a] = new() { [b] = 1.0, [c] = 5.0 },
            [b] = new() { [c] = 2.0 },
            [c] = new() { [d] = 1.0 },
            [d] = new() { [a] = 4.0 }, // cycle
        };

        _graph = new Graph<string>(adjacencyList);
    }

    [TestMethod]
    public void TestBFS()
    {
        var result = _graph.PerformBreadthFirstSearch("A");
        var names = result.ConvertAll(n => n.Data);

        CollectionAssert.AreEqual(new List<string> { "A", "B", "C", "D" }, names);
    }

    [TestMethod]
    public void TestDFS()
    {
        var result = _graph.PerformDepthFirstSearch("A");
        Assert.IsTrue(result.Count == 4, "DFS ne couvre pas tous les sommets");
    }

    [TestMethod]
    public void TestDetectCycle()
    {
        var cycle = _graph.DetectAnyCycle();
        Assert.IsTrue(cycle.Count > 0, "Aucun cycle détecté alors qu'il y en a un");
    }

    [TestMethod]
    public void TestStronglyConnectedComponents()
    {
        var sccs = _graph.GetStronglyConnectedComponents();
        Assert.IsTrue(
            sccs.Count == 1,
            "Le graphe devrait être une seule composante fortement connexe"
        );
    }

    [TestMethod]
    public void TestDijkstra()
    {
        var paths = _graph.ComputeDijkstra("A");
        Assert.IsTrue(
            paths.ContainsKey(Node<string>.GetNode(3)),
            "Le chemin jusqu'à D n'a pas été trouvé"
        );
        var pathNames = paths[Node<string>.GetNode(3)].ConvertAll(n => n.Data);
        CollectionAssert.AreEqual(new List<string> { "A", "B", "C", "D" }, pathNames);
    }

    [TestMethod]
    public void TestBellmanFord()
    {
        var paths = _graph.ComputeBellmanFord("A");
        var pathNames = paths[Node<string>.GetNode(3)].ConvertAll(n => n.Data);
        CollectionAssert.AreEqual(new List<string> { "A", "B", "C", "D" }, pathNames);
    }

    [TestMethod]
    public void TestRoyFloyd()
    {
        var paths = _graph.ComputeRoyFloydWarshall();
        var start = Node<string>.GetNode(0); // A
        var end = Node<string>.GetNode(3); // D
        var path = paths[_graph.NodeIndexMap[start], _graph.NodeIndexMap[end]];

        var pathNames = path.ConvertAll(n => n.Data);
        CollectionAssert.AreEqual(new List<string> { "A", "B", "C", "D" }, pathNames);
    }
}
