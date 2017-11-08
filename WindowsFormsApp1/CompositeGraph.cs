using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class CompositeGraph<TVertex,TEdge> : Graph<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        public Dictionary<TVertex, TEdge> InnerPath { get; set; }
        public Dictionary<TVertex, List<CompositeEdge<TVertex>>> IngoingEdges { get; set; }
        public Dictionary<TVertex, List<CompositeEdge<TVertex>>> OutgoingEdges { get; set; }

        public CompositeGraph(Graph<TVertex, TEdge> graph, Dictionary<TVertex, TEdge> innerPath, Dictionary<TVertex, List<CompositeEdge<TVertex>>> ingoingEdges, Dictionary<TVertex, List<CompositeEdge<TVertex>>> outgoingEdges) : base(graph)
        {
            InnerPath = innerPath;
            IngoingEdges = ingoingEdges;
            OutgoingEdges = outgoingEdges;
        }
    }
}
