namespace LivInParisRoussilleTeynier.UI;

public class GraphPage : Page
{
    private Metro? _metro;
    private Graph<Station>? _graph;

    public override void Display()
    {
        LoadGraph();

        while (true)
        {
            var menu = new ScrollingMenu(
                "Exploration Graphe (Métro Paris)",
                choices:
                [
                    "Afficher propriétés du graphe",
                    "Parcours en largeur (BFS)",
                    "Parcours en profondeur (DFS)",
                    "Détection de cycle",
                    "Composantes fortement connexes",
                    "Dijkstra (plus court chemin)",
                    "Bellman-Ford (plus court chemin)",
                    "Roy-Floyd-Warshall (tous chemins)",
                    "Visualiser graphe (GraphViz)",
                    "Visualiser PCC",
                    "Retour",
                ]
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);
            var response = menu.GetResponse();
            Window.RemoveElement(menu);
            Window.Render();

            if (response?.Status != Status.Selected)
                return;

            switch (response.Value)
            {
                case 0:
                    ShowGraphInfo();
                    break;
                case 1:
                    RunBFS();
                    break;
                case 2:
                    RunDFS();
                    break;
                case 3:
                    DetectCycle();
                    break;
                case 4:
                    ShowSCC();
                    break;
                case 5:
                    RunDijkstra();
                    break;
                case 6:
                    RunBellmanFord();
                    break;
                case 7:
                    RunRoyFloyd();
                    break;
                case 8:
                    VisualizeGraph();
                    break;
                case 9:
                    VisualizePcc();
                    break;
            }
        }
    }

    private void LoadGraph()
    {
        _metro = new Metro("MetroParis");
        _graph = _metro.Graph;
    }

    private void ShowGraphInfo()
    {
        var lines = new List<string>
        {
            $"Graphe orienté : {_graph.IsDirected}",
            $"Nombre de stations (nœuds) : {_graph.Nodes.Count}",
            $"Nombre de connexions (arêtes) : {_graph.Edges.Count}",
            $"Densité : {Math.Round(_graph.Density, 4)}",
            $"Pondéré : {_graph.Edges.Any(e => Math.Abs(e.Weight - 1.0) > 1e-6)}",
            $"Connecté : {_graph.PerformDepthFirstSearch(_graph.Nodes.First()).Count == _graph.Nodes.Count}",
        };

        var text = new EmbedText(lines);
        Window.AddElement(text);
        Window.Render();
        Thread.Sleep(3000);
        Window.RemoveElement(text);
        Window.Render();
    }

    private void RunBFS()
    {
        var station = _graph.Nodes.First().Data;
        var visited = _graph.PerformBreadthFirstSearch(station);

        var lines = visited.Select(n => n.Data.ToString()).ToList();
        ShowList("Résultat BFS (depuis " + station.ToString() + ")", lines);
    }

    private void RunDFS()
    {
        var station = _graph.Nodes.First().Data;
        var visited = _graph.PerformDepthFirstSearch(station);

        var lines = visited.Select(n => n.Data.ToString()).ToList();
        ShowList("Résultat DFS (depuis " + station.ToString() + ")", lines);
    }

    private void DetectCycle()
    {
        var cycle = _graph.DetectAnyCycle(simpleCycle: true);

        if (cycle.Count > 0)
        {
            var names = cycle.Select(n => n.Data.ToString()).ToList();
            ShowList("Cycle détecté :", names);
        }
        else
        {
            ShowText("Aucun cycle détecté.");
        }
    }

    private void ShowSCC()
    {
        var sccs = _graph.GetStronglyConnectedComponents();
        var lines = new List<string> { $"Nombre de composantes fortement connexes : {sccs.Count}" };

        for (int i = 0; i < Math.Min(5, sccs.Count); i++)
        {
            var names = sccs[i].Nodes.Select(n => n.Data.ToString());
            lines.Add($"SCC {i + 1}: {string.Join(", ", names)}");
        }

        ShowList("SCC (extrait)", lines);
    }

    private void RunDijkstra()
    {
        var source = _graph.Nodes.First().Data;
        var dijkstraPaths = _graph.ComputeDijkstra(source);

        var lines = new List<string>
        {
            $"Plus court chemin depuis {source} (Dijkstra) :",
            $"Vers {dijkstraPaths.Keys.First().Data} : "
                + $"{string.Join(" -> ", dijkstraPaths.Values.First().Select(n => n.Data.ToString()))}",
        };

        ShowList("Exemple Dijkstra", lines);
    }

    private void RunBellmanFord()
    {
        var source = _graph.Nodes.First().Data;
        var paths = _graph.ComputeBellmanFord(source);
        var firstPath = paths.Values.FirstOrDefault();

        var lines = new List<string>
        {
            $"Plus court chemin depuis {source} (Bellman-Ford) :",
            firstPath != null
                ? string.Join(" -> ", firstPath.Select(n => n.Data))
                : "Aucun chemin trouvé.",
        };

        ShowList("Exemple Bellman-Ford", lines);
    }

    private void RunRoyFloyd()
    {
        var allPaths = _graph.ComputeRoyFloydWarshall();
        var from = _graph.Nodes.ElementAt(0);
        var to = _graph.Nodes.ElementAt(1);
        var path = allPaths[_graph.NodeIndexMap[from], _graph.NodeIndexMap[to]];

        var lines = new List<string>
        {
            $"Chemin Roy-Floyd-Warshall de {from.Data} à {to.Data}:",
            string.Join(" -> ", path.Select(n => n.Data)),
        };

        ShowList("Exemple Roy-Floyd", lines);
    }

    private void VisualizeGraph()
    {
        _graph.DisplayGraph("metro_visual", layout: "fdp", nodeShape: "circle");
        ShowText("Image générée : metro_visual_*.png");
    }

    private void VisualizePcc()
    {
        var source = _graph.Nodes.First().Data;
        var dijkstraGraph = _graph.GetPartialGraphByDijkstra(source);
        dijkstraGraph.DisplayGraph("pcc_visual", layout: "dot", fontsize: 9);
        ShowText("Image générée : pcc_visual_*.png");
    }

    private void ShowText(string message)
    {
        var text = new EmbedText([message]);
        Window.AddElement(text);
        Window.Render();
        Thread.Sleep(2000);
        Window.RemoveElement(text);
        Window.Render();
    }

    private void ShowList(string title, List<string> lines)
    {
        var liness = new List<List<string>> { lines };
        var scrollable = new TableView(lines: liness);
        Window.AddElement(scrollable);
        Window.Render();

        Prompt wait = new Prompt("Appuyez sur Entrée pour continuer...");
        Window.AddElement(wait);
        Window.ActivateElement(wait);
        wait.GetResponse();

        Window.RemoveElement(scrollable);
        Window.RemoveElement(wait);
        Window.Render();
    }
}
