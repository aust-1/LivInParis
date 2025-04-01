using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace LivinParis.Models.Maps.Helpers;

/// <summary>
/// Provides functionality to visualize a generic <see cref="Graph{T}"/>
/// by exporting it to a DOT file and generating a PNG image through GraphViz.
/// </summary>
/// <typeparam name="T">The type of data stored in each node (must be non-null).</typeparam>
public static class Visualization<T>
    where T : notnull
{
    #region Public Methods

    /// <summary>
    /// Exports the specified graph to a DOT file, calls GraphViz to render a PNG image,
    /// and then removes the intermediate DOT file.
    /// </summary>
    /// <param name="graph">The graph to visualize.</param>
    /// <param name="outputImageName">
    /// The base file name (without extension) for the output image. A timestamp is appended to avoid collisions.
    /// </param>
    /// <param name="layout">The GraphViz layout algorithm (e.g., "dot", "fdp", "neato"). Default is "neato".</param>
    /// <param name="shape">The node shape (e.g., "circle", "point", "square"). Default is "point".</param>
    /// <remarks>
    /// This method alters the current thread's culture to <c>en-US</c> to ensure consistent numeric formats.
    /// </remarks>
    public static void DisplayGraph(
        Graph<T> graph,
        string outputImageName = "graph",
        string layout = "neato",
        string shape = "point"
    )
    {
        CultureInfo culture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        string dotFilePath = $"{outputImageName}.dot";
        string outputImagePath =
            $"data/output/{outputImageName}_{DateTime.Now:yyyyMMdd_HH-mm-ss}.png";

        GenerateDotFile(graph, dotFilePath, layout, shape);
        RenderToPng(dotFilePath, outputImagePath);
        File.Delete(dotFilePath);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Renders a DOT file into a PNG image using GraphViz,
    /// installing GraphViz via winget if not found.
    /// </summary>
    /// <param name="dotFilePath">The path to the DOT file to be rendered.</param>
    /// <param name="outputImagePath">The path of the resulting PNG image.</param>
    /// <exception cref="FileNotFoundException">Thrown if the DOT file is missing.</exception>
    private static void RenderToPng(string dotFilePath, string outputImagePath)
    {
        if (!File.Exists(dotFilePath))
        {
            throw new FileNotFoundException("DOT file not found.", dotFilePath);
        }

        const string graphVizPath = @"C:\Program Files\Graphviz\bin\dot.exe";
        if (!File.Exists(graphVizPath))
        {
            Console.WriteLine(
                "GraphViz is not installed on this machine. Please install it or use another rendering method.\n"
                    + "GraphViz can be installed via winget using:\n"
                    + "  winget install -e --id Graphviz.Graphviz"
            );
            Console.WriteLine(
                "Install now? (y/n) ('y' will attempt a silent installation via winget.)"
            );

            string? response = Console.ReadLine();
            if (response?.ToLower() == "y")
            {
                InstallGraphViz();
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
    /// Generates a DOT file representing the specified graph for use with GraphViz.
    /// </summary>
    /// <param name="graph">The graph to export.</param>
    /// <param name="filePath">The path to the DOT file to be created.</param>
    /// <param name="layout">The GraphViz layout algorithm (e.g., "dot", "fdp", "neato").</param>
    /// <param name="shape">The shape of the nodes (e.g., "circle", "square", "point").</param>
    private static void GenerateDotFile(
        Graph<T> graph,
        string filePath,
        string layout,
        string shape
    )
    {
        var dotBuilder = new StringBuilder();
        var processedLabels = new HashSet<string>();

        dotBuilder.AppendLine(graph.IsDirected ? "digraph G {" : "graph G {");
        dotBuilder.AppendLine($"    layout={layout};");
        dotBuilder.AppendLine("    ratio=0.6438356164;");
        dotBuilder.AppendLine($"    node [shape={shape}, fontsize=\"10\"];");

        foreach (var node in graph.Nodes)
        {
            dotBuilder.Append($"    \"{node.Data}\" [{node.VisualizationParameters}");

            if (processedLabels.Contains(node.VisualizationParameters.Label))
            {
                dotBuilder.Append(", penwidth=4");
            }
            else
            {
                dotBuilder.Append($", xlabel=\"{node.VisualizationParameters.Label}\"");
                processedLabels.Add(node.VisualizationParameters.Label);
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

    /// <summary>
    /// Installs GraphViz using the Windows Package Manager (winget).
    /// </summary>
    private static void InstallGraphViz()
    {
        using var installProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments =
                    "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            },
        };

        installProcess.Start();
        string output = installProcess.StandardOutput.ReadToEnd();
        string error = installProcess.StandardError.ReadToEnd();
        installProcess.WaitForExit();

        if (installProcess.ExitCode == 0)
        {
            Console.WriteLine("GraphViz installed successfully.");
            Console.WriteLine("Output:\n" + output);
        }
        else
        {
            Console.WriteLine("Failed to install GraphViz.");
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error:\n" + error);
            }
        }
    }

    #endregion Private Methods
}
