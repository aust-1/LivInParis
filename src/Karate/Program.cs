using Karate;

/// <summary>
/// Programme principal : lit un fichier .mtx, construit le graphe, effectue BFS/DFS et dessine.
/// </summary>
public static class Program
{
    static void Main(string[] args)
    {
        string path = "data/soc-karate.mtx";
        
        Graph graphe = Graph.ReadMtxFile(path, isDirected: false);


        Console.WriteLine("== BFS depuis le noeud 1 ==");
        var bfs = graphe.BFS(1);
        Console.WriteLine("Ordre BFS : " + string.Join(" -> ", bfs));


        Console.WriteLine("== DFS depuis le noeud 1 ==");
        var dfs = graphe.DFSIterative(1);
        Console.WriteLine("Ordre DFS : " + string.Join(" -> ", dfs));
        
        graphe.BuildAdjacencyMatrix();
        
        graphe.DrawGraph("karate.png");
    }
}