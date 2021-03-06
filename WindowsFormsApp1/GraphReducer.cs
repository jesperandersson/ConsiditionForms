﻿using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GraphReducer<TVertex>
    {
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
                reducedGraph = GraphNegativeCycleRemover<TVertex>.RemoveNegativeCycles(reducedGraph, false);
            }
            else if (Properties.Settings.Default.removeNegativeCyclesOfLengthTwo)
            {
                reducedGraph = GraphNegativeCycleRemover<TVertex>.RemoveNegativeCycles(reducedGraph, true);
            }

            reducedGraph.Start = graph.Start;
            reducedGraph.Goal = graph.Goal;

            return reducedGraph;
        }
    }
}
