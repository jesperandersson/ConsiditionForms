using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GraphReducer<TVertex>
    {
        public static Graph<TVertex, CompositeEdge<TVertex>> removeNegativeCycles(Graph<TVertex, CompositeEdge<TVertex>> graph, bool onlyRemoveNegativeCyclesOfLengthTwo)
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

        public static Graph<TVertex, CompositeEdge<TVertex>> CreateReducedGraph(Graph<TVertex, EdgeWithId<TVertex>> graph)
        {
            var reducedGraph = new Graph<TVertex, CompositeEdge<TVertex>>();

            HashSet<TVertex> bonusStartDestSet = new HashSet<TVertex>(); // bonusStartDestSet = C
            // C = B + {s, d}
            foreach(var v in graph.BonusVertices.Keys)
            {
                bonusStartDestSet.Add(v);
            }
            bonusStartDestSet.Add(graph.Start);
            bonusStartDestSet.Add(graph.Goal);

            foreach (var source in bonusStartDestSet)
            {
                reducedGraph.AddVertex(source);
                TryFunc<TVertex, IEnumerable<EdgeWithId<TVertex>>> tryGetPaths = graph.getAllShortestPathDijkstra(source);
                foreach (var destination in bonusStartDestSet)
                {
                    if (source.Equals(destination))
                        continue;

                    IEnumerable<EdgeWithId<TVertex>> path;
                    double cost = 0;
                    if (tryGetPaths(destination, out path))
                    {
                        foreach (var e in path)
                        {
                            cost += graph.EdgeCosts[e];
                        }

                        if (graph.BonusVertices.ContainsKey(destination))
                            cost -= graph.BonusVertices[destination];

                        var compositeEdge = new CompositeEdge<TVertex>(source, destination, path);
                        reducedGraph.AddEdge(compositeEdge, cost);
                    }
                    else
                    {
                        Console.WriteLine("No path found between {0} and {1}", source.ToString(), destination.ToString());
                    }
                }
            }

            if (Properties.Settings.Default.removeNegativeCycles)
            {
                reducedGraph = removeNegativeCycles(reducedGraph, false);
            }
            else if (Properties.Settings.Default.removeNegativeCyclesOfLengthTwo)
            {
                reducedGraph = removeNegativeCycles(reducedGraph, true);
            }

            return reducedGraph;
        }
    }
}
