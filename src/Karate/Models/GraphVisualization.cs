using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public class GraphVisualization
{
    public static void RenderDotFile(string dotFilePath, string outputImagePath)
    {
        if (!File.Exists(dotFilePath))
        {
            Console.WriteLine("Le fichier DOT spécifié n'existe pas.");
            return;
        }

        // Vérifie que GraphViz est installé et accessible
        string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
        if (!File.Exists(graphVizPath))
        {
            Console.WriteLine("GraphViz n'est pas installé ou le chemin est incorrect.");
            return;
        }

        // Exécute GraphViz pour générer une image
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = graphVizPath,
                Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        Console.WriteLine($"Image générée : {outputImagePath}");
    }

    /// <summary>
    /// Exporte le graphe au format DOT.
    /// </summary>
    /// <param name="filePath">Chemin du fichier DOT à générer.</param>
    public static void ExportToDot(Graph graph, string filePath, string layout = "dot")
    {
        StringBuilder dotBuilder = new StringBuilder();
        dotBuilder.AppendLine(graph.IsDirected ? "digraph G {" : "graph G {");
        dotBuilder.AppendLine($"    layout={layout};");

        foreach (var node in graph.Nodes)
        {
            dotBuilder.AppendLine($"    \"{node.Id}\";");
        }

        foreach (var edge in graph.Edges)
        {
            if (!graph.IsDirected && edge.SourceNode.Id.CompareTo(edge.TargetNode.Id) > 0)
            {
                dotBuilder.AppendLine($"    \"{edge.SourceNode.Id}\" {"--"} \"{edge.TargetNode.Id}\";");
            }
            else if (graph.IsDirected)
            {
                dotBuilder.AppendLine($"    \"{edge.SourceNode.Id}\" {"->"} \"{edge.TargetNode.Id}\";");
            }
        }

        dotBuilder.AppendLine("}");

        File.WriteAllText(filePath, dotBuilder.ToString());
        Console.WriteLine($"Graph exported to {filePath}");
    }
}
