using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GraphDrawer<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        public Graph<TVertex, TEdge> Graph { get; set; }
        public GraphvizAlgorithm<TVertex, TEdge> GraphvizAlgorithm { get; set; }
        public IEdgeListGraph<TVertex, TEdge> Path { get; set; }

        public GraphDrawer(Graph<TVertex, TEdge> graph)
        {
            Graph = graph;
            GraphvizAlgorithm = new GraphvizAlgorithm<TVertex, TEdge>(Graph.AdjacencyGraph);
        }

        public void DrawGraph()
        {
            GraphvizAlgorithm.FormatEdge += OnFormatEdge;
            GraphvizAlgorithm.FormatVertex += OnFormatVertex;

            string output = GraphvizAlgorithm.Generate(new FileDotEngine(), "output");
        }

        private void OnFormatVertex(object sender, FormatVertexEventArgs<TVertex> v)
        {
            v.VertexFormatter.Label = v.Vertex.ToString();
            if (Path != null && Path.ContainsVertex(v.Vertex))
            {
                v.VertexFormatter.Shape = GraphvizVertexShape.Box;
            }
        }

        private void OnFormatEdge(object obj, FormatEdgeEventArgs<TVertex, TEdge> e)
        {
            e.EdgeFormatter.Label.Value = Graph.EdgeCosts[e.Edge].ToString();
            if (Path != null && Path.ContainsEdge(e.Edge))
            {
                e.EdgeFormatter.Style = GraphvizEdgeStyle.Bold;
            } else
            {
                e.EdgeFormatter.Style = GraphvizEdgeStyle.Dashed;
            }
        }
    }
}
