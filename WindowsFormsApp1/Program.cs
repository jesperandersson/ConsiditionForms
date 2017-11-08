using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TestShortestPathAndDrawGraph();
            if (Properties.Settings.Default.showGraph)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        public static void TestShortestPathAndDrawGraph()
        {
            Graph<string, EdgeWithId<string>> graph = GraphCreator.CreateGraphWithBonusCities();

            Graph<string, CompositeEdge<string>> reducedGraph = GraphReducer<string>.CreateReducedGraph(graph);

            string target = graph.Goal;
            IEnumerable<CompositeEdge<string>> path;

            TryFunc<string, IEnumerable<CompositeEdge<string>>> tryGetPaths;
            CompositeGraph<string, CompositeEdge<string>> compositeGraph = null;
            if (Properties.Settings.Default.removeNegativeCyclesByAddingNodes)
            {
                compositeGraph = GraphNegativeCycleRemover<string>.RemoveNegativeCyclesByAddingNodes(reducedGraph);
                tryGetPaths = compositeGraph.getAllShortestPathBellmanFord(graph.Start);
            }
            else
            {
                tryGetPaths = reducedGraph.getAllShortestPathBellmanFord(graph.Start);
            }
            
            if (tryGetPaths(target, out path))
            {
                var fullPath = new List<EdgeWithId<string>>();
                foreach (var compPath in path)
                {
                    foreach (var edge in compPath.ComponentEdges)
                    {
                        if (Properties.Settings.Default.removeNegativeCyclesByAddingNodes)
                        {
                            if (compositeGraph.InnerPath.Keys.Contains(edge.Source))
                            {
                                fullPath.AddRange(compositeGraph.InnerPath[edge.Source].ComponentEdges);
                            }

                            fullPath.Add(edge);

                            if (compositeGraph.InnerPath.Keys.Contains(edge.Target))
                            {
                                fullPath.AddRange(compositeGraph.InnerPath[edge.Target].ComponentEdges);
                            }
                        }
                        else
                        {
                            fullPath.Add(edge);
                        }
                    }
                }

                drawGraph(graph, reducedGraph, path, fullPath);
                printResults(graph, fullPath);
            }
            else
            {
                Console.WriteLine("No path found");
            }
        }

        private static void drawGraph(Graph<string, EdgeWithId<string>> graph, Graph<string, CompositeEdge<string>> reducedGraph, IEnumerable<CompositeEdge<string>> path, IEnumerable<EdgeWithId<string>> fullPath)
        {
            if (Properties.Settings.Default.showGraph)
            {
                if (Properties.Settings.Default.showReducedGraph)
                {
                    GraphDrawer<string, CompositeEdge<string>> graphDrawer = new GraphDrawer<string, CompositeEdge<string>>(reducedGraph);
                    EdgeListGraph<string, CompositeEdge<string>> highlightedPath = new EdgeListGraph<string, CompositeEdge<string>>();
                    highlightedPath.AddVerticesAndEdgeRange(path);
                    graphDrawer.Path = highlightedPath;
                    graphDrawer.DrawGraph();
                }
                else
                {
                    GraphDrawer<string, EdgeWithId<string>> graphDrawer = new GraphDrawer<string, EdgeWithId<string>>(graph);
                    EdgeListGraph<string, EdgeWithId<string>> highlightedPath = new EdgeListGraph<string, EdgeWithId<string>>();
                    highlightedPath.AddVerticesAndEdgeRange(fullPath);
                    graphDrawer.Path = highlightedPath;
                    graphDrawer.DrawGraph();
                }
            }
        }

        private static void printResults(Graph<string, EdgeWithId<string>> graph, IEnumerable<EdgeWithId<string>> path)
        {
            var emissionCost = 0.0;
            var timeCost = 0.0;
            var algorithmCost = 0.0;
            var trueCost = 0.0;
            var bonusPoints = 0.0;

            foreach (var edge in path)
            {
                emissionCost += edge.EmissionCost;
                timeCost += edge.TimeCost;
                algorithmCost += edge.GetAlgorithmCost();
                trueCost += edge.GetTrueCost();
                Console.WriteLine(edge);
                if (graph.BonusVertices.Keys.Contains(edge.Target))
                {
                    bonusPoints += graph.BonusVertices[edge.Target];
                }
            }
            algorithmCost -= bonusPoints;
            trueCost -= bonusPoints;

            Console.WriteLine("Emission cost: " + emissionCost);
            Console.WriteLine("Time cost: " + timeCost);
            Console.WriteLine("Bonus points: " + bonusPoints);
            Console.WriteLine("Algorithm cost: " + algorithmCost);
            Console.WriteLine("True cost: " + trueCost);
        }
    }
}
