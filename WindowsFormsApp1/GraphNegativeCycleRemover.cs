using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GraphNegativeCycleRemover<TVertex>
    {
        private static void AddCompositeNode(CompositeGraph<string, CompositeEdge<string>> graph, CompositeEdge<string> edge)
        {
            string vertex = edge.Source + ";" + edge.Target;
            graph.AddVertex(vertex);
            var componentEdges = new List<EdgeWithId<string>>();
            if (graph.InnerPath.Keys.Contains(edge.Source))
            {
                componentEdges.AddRange(graph.InnerPath[edge.Source].ComponentEdges);
            }
            componentEdges.AddRange(edge.ComponentEdges);
            if (graph.InnerPath.Keys.Contains(edge.Target))
            {
                componentEdges.AddRange(graph.InnerPath[edge.Target].ComponentEdges);
            }
            graph.InnerPath[vertex] = new CompositeEdge<string>(edge.Source, edge.Target, componentEdges);
            graph.IngoingEdges[vertex] = new List<CompositeEdge<string>>();
            graph.OutgoingEdges[vertex] = new List<CompositeEdge<string>>();
            foreach (var ie in graph.IngoingEdges[edge.Source])
            {
                if (!vertex.Split(';').Contains(ie.Source))
                {
                    var e = new CompositeEdge<string>(ie.Source, vertex, ie.ComponentEdges);
                    var cost = graph.EdgeCosts[ie] + graph.EdgeCosts[edge];
                    graph.AddEdge(e, cost);
                    graph.IngoingEdges[vertex].Add(e);
                }
            }
            foreach (var oe in graph.OutgoingEdges[edge.Target])
            {
                if (!vertex.Split(';').Contains(oe.Target))
                {
                    var e = new CompositeEdge<string>(vertex, oe.Target, oe.ComponentEdges);
                    var cost = graph.EdgeCosts[oe];
                    graph.AddEdge(e, cost);
                    graph.OutgoingEdges[vertex].Add(e);
                }
            }
        }

        private static bool RemoveNegativeCycleByAddingNodes(CompositeGraph<string, CompositeEdge<string>> graph)
        {
            foreach (var edge in graph.AdjacencyGraph.Edges)
            {
                foreach (var outgoingEdge in graph.OutgoingEdges[edge.Target])
                {
                    if (outgoingEdge.Target.Equals(edge.Source))
                    {
                        bool negativeCycle = graph.EdgeCosts[edge] + graph.EdgeCosts[outgoingEdge] < 0;
                        if (negativeCycle)
                        {
                            AddCompositeNode(graph, edge);
                            AddCompositeNode(graph, outgoingEdge);
                            
                            graph.EdgeCosts[edge] = int.MaxValue;
                            graph.EdgeCosts[outgoingEdge] = int.MaxValue;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static CompositeGraph<string, CompositeEdge<string>> RemoveNegativeCyclesByAddingNodes(Graph<string, CompositeEdge<string>> graph)
        {
            Dictionary<string, CompositeEdge<string>> innerPaths = new Dictionary<string, CompositeEdge<string>>();
            Dictionary<string, List<CompositeEdge<string>>> ingoingEdges = new Dictionary<string, List<CompositeEdge<string>>>();
            Dictionary<string, List<CompositeEdge<string>>> outgoingEdges = new Dictionary<string, List<CompositeEdge<string>>>();

            foreach (var vertex in graph.AdjacencyGraph.Vertices)
            {
                ingoingEdges[vertex] = new List<CompositeEdge<string>>();
                outgoingEdges[vertex] = new List<CompositeEdge<string>>();
            }

            foreach (var edge in graph.AdjacencyGraph.Edges)
            {
                if (edge.Target.Equals(graph.Start) || edge.Source.Equals(graph.Goal))
                {
                    graph.EdgeCosts[edge] = int.MaxValue;
                }

                ingoingEdges[edge.Target].Add(edge);
                outgoingEdges[edge.Source].Add(edge);
            }

            CompositeGraph<string, CompositeEdge<string>> compositeGraph = new CompositeGraph<string, CompositeEdge<string>>(graph, innerPaths, ingoingEdges, outgoingEdges);

            while (RemoveNegativeCycleByAddingNodes(compositeGraph));
            return compositeGraph;
        }


        public static Graph<TVertex, CompositeEdge<TVertex>> RemoveNegativeCycles(Graph<TVertex, CompositeEdge<TVertex>> graph, bool onlyRemoveNegativeCyclesOfLengthTwo)
        {
            var eps = 1 / int.MaxValue;

            var edges = graph.AdjacencyGraph.Edges;
            var edgeCosts = graph.EdgeCosts;
            foreach (var edge in edges)
            {
                if (edgeCosts[edge] <= 0)
                {
                    var source = edge.Source;
                    var target = edge.Target;

                    foreach (var edgeCandidate in edges)
                    {
                        bool condition = edgeCandidate.Source.Equals(target);
                        if (onlyRemoveNegativeCyclesOfLengthTwo)
                        {
                            condition = condition && edgeCandidate.Target.Equals(source);
                        }
                        if (condition)
                        {
                            if (edgeCosts[edgeCandidate] <= 0)
                            {
                                edgeCosts[edge] = eps;
                                edgeCosts[edgeCandidate] = eps;
                            }
                            else
                            {
                                edgeCosts[edge] = Math.Max(edgeCosts[edge], -edgeCosts[edgeCandidate] + eps);
                            }
                        }
                    }
                }
            }
            return graph;
        }
    }
}
