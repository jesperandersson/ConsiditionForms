using QuickGraph;
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

        public Graph()
        {
        }

        public Graph(AdjacencyGraph<TVertex, TEdge> adjacencyGraph, Dictionary<TEdge, double> edgeCosts)
        {
            AdjacencyGraph = adjacencyGraph;
            EdgeCosts = edgeCosts;
        }
    }
}
