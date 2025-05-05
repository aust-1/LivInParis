using LivInParisRoussilleTeynier.Domain.Models.Maps;

namespace LivInParisRoussilleTeynier.Services.Interfaces
{
    public interface IGraphService<T>
        where T : notnull
    {
        //TODO: rajouter affichage propriétés sur les graphes (ex: connexe, orienté, etc.)
        List<Node<T>> PerformBreadthFirstSearch<TU>(Graph<T> graph, TU start)
            where TU : notnull;
        List<Node<T>> PerformDepthFirstSearch<TU>(Graph<T> graph, TU start, bool inverted = false)
            where TU : notnull;

        SortedDictionary<Node<T>, List<Node<T>>> ComputeDijkstra<TU>(Graph<T> graph, TU start)
            where TU : notnull;
        SortedDictionary<Node<T>, List<Node<T>>> ComputeBellmanFord<TU>(Graph<T> graph, TU start)
            where TU : notnull;
        (double Weight, List<Node<T>> Path)[,] ComputeRoyFloydWarshall(Graph<T> graph);

        Graph<T> GetPartialGraphByDijkstra<TU>(Graph<T> graph, TU start)
            where TU : notnull;
        Graph<T> GetPartialGraphByBellmanFord<TU>(Graph<T> graph, TU start)
            where TU : notnull;

        List<Node<T>> DetectAnyCycle(Graph<T> graph, bool simpleCycle = false);

        void DisplayGraph(
            Graph<T> graph,
            string outputImageName = "graph",
            string layout = "neato",
            string nodeShape = "point",
            float fontsize = 10.0f,
            float penwidth = 1.0f
        );
    }
}
