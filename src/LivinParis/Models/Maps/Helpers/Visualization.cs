using System.Diagnostics;
using System.Text;

namespace LivinParis.Models.Maps.Helpers;

public static class Visualization<T>
    where T : notnull
{
    /// <summary>
    /// Exports the graph to a DOT file, then calls GraphViz to generate a PNG image,
    /// finally deleting the DOT file.
    /// </summary>
    /// <param name="graph">The graph to visualize.</param>
    /// <param name="outputImageName">
    /// The base file name (without extension) for the output. A timestamp is appended to avoid overwrites.
    /// </param>
    /// <param name="layout">The GraphViz layout to use (e.g. "dot", "fdp", "neato", ...).</param>
    /// <param name="shape">The shape of the nodes (e.g. "circle", "square", "triangle", ...).</param>
    public static void DisplayGraph(
        Graph<T> graph,
        string outputImageName = "graph",
        string layout = "neato",
        string shape = "point"
    )
    {
        string dotFilePath = $"{outputImageName}.dot";
        string outputImagePath =
            $"data/output/{outputImageName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

        ExportToDot(graph, dotFilePath, layout, shape);
        RenderDotFile(dotFilePath, outputImagePath);
        File.Delete(dotFilePath);
    }

    /// <summary>
    /// Renders a DOT file into a PNG image using GraphViz,
    /// installing GraphViz via winget if not found.
    /// </summary>
    /// <param name="dotFilePath">The path to the DOT file.</param>
    /// <param name="outputImagePath">The path of the resulting PNG image.</param>
    /// <exception cref="FileNotFoundException">Thrown if the DOT file is missing.</exception>
    private static void RenderDotFile(string dotFilePath, string outputImagePath)
    {
        if (!File.Exists(dotFilePath))
        {
            throw new FileNotFoundException("DOT file not found.", dotFilePath);
        }

        const string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
        if (!File.Exists(graphVizPath))
        {
            Console.WriteLine(
                "GraphViz is not installed on this machine. Please install it or use another method.\n"
                    + "GraphViz can be installed via winget:\n"
                    + "  winget install -e --id Graphviz.Graphviz"
            );
            Console.WriteLine(
                "Install now? (y/n) ('y' will attempt a silent installation via winget.)"
            );

            string? response = Console.ReadLine();
            if (response?.ToLower() == "y")
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using var installProcess = new Process { StartInfo = psi };
                installProcess.Start();
                string output = installProcess.StandardOutput.ReadToEnd();
                string error = installProcess.StandardError.ReadToEnd();
                installProcess.WaitForExit();

                Console.WriteLine("Output:\n" + output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error:\n" + error);
                }
            }
            else
            {
                return;
            }
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = graphVizPath,
                Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Exports the current graph to a DOT file for visualization with GraphViz.
    /// </summary>
    /// <param name="graph">The graph to export.</param>
    /// <param name="filePath">The path to the DOT file.</param>
    /// <param name="layout">The GraphViz layout algorithm (e.g., "dot", "fdp", "neato", ...).</param>
    /// <param name="shape">The shape of the nodes (e.g., "circle", "rectangle", "diamond", ...).</param>
    private static void ExportToDot(Graph<T> graph, string filePath, string layout, string shape)
    {
        var dotBuilder = new StringBuilder();
        var clusters = new List<string>();

        dotBuilder.AppendLine(graph.IsDirected ? "digraph G {" : "graph G {");
        dotBuilder.AppendLine($"    layout={layout};");
        dotBuilder.AppendLine("    ratio=0.6438356164;");
        dotBuilder.AppendLine($"    node [shape={shape}, fontsize=\"10\"];");

        foreach (var node in graph.Nodes)
        {
            dotBuilder.Append($"    \"{node.Data}\" [{node.VisualizationParameters}");
            if (clusters.Contains(node.VisualizationParameters.Label))
            {
                dotBuilder.AppendLine(", penwidth=4");
            }
            else
            {
                dotBuilder.Append($", xlabel=\"{node.VisualizationParameters.Label}\"");
                clusters.Add(node.VisualizationParameters.Label);
            }
            dotBuilder.AppendLine("];");
        }

        dotBuilder.AppendLine();

        foreach (var edge in graph.Edges.Where(e => e.RGBColor != "#000000"))
        {
            if (!graph.IsDirected && edge.SourceNode.Id > edge.TargetNode.Id)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -- \"{edge.TargetNode.Data}\"");
            }
            else if (graph.IsDirected)
            {
                dotBuilder.Append($"    \"{edge.SourceNode.Data}\" -> \"{edge.TargetNode.Data}\"");
                if (!edge.IsDirected)
                {
                    dotBuilder.Append(" [dir=both]");
                }
            }

            dotBuilder.AppendLine($" [color=\"{edge.RGBColor}\"];");
        }

        dotBuilder.AppendLine("}");
        File.WriteAllText(filePath, dotBuilder.ToString());
    }
}
