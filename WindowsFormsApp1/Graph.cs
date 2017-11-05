using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Graph<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        public AdjacencyGraph<TVertex, TEdge> AdjacencyGraph { get; set; }
        public Dictionary<TEdge, double> EdgeCosts { get; set; }

        public Dictionary<TVertex, double> BonusPoints { get; set; }

        public TVertex Start { get; set; }
        public TVertex Goal { get; set; }

        public Graph()
        {
        }
        public Graph(AdjacencyGraph<TVertex, TEdge> adjacencyGraph, Dictionary<TEdge, double> edgeCosts)
        {
            AdjacencyGraph = adjacencyGraph;
            EdgeCosts = edgeCosts;
        }

        public TryFunc<TVertex, IEnumerable<TEdge>> getAllShortestPath(TVertex start)
        {
            Func<TEdge, double> edgeCost = AlgorithmExtensions.GetIndexer(EdgeCosts);
            return AdjacencyGraph.ShortestPathsDijkstra(edgeCost, start);
        }
    }
}
