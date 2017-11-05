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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        public static void TestShortestPathAndDrawGraph()
        {
            Graph<string, EdgeWithId<string>> graph = GraphCreator.CreateGraphWithBonusCities();

            Graph<string, CompositeEdge<string>> reducedGraph = GraphReducer<string>.CreateReducedGraph(graph);
            TryFunc<string, IEnumerable<CompositeEdge<string>>> tryGetPaths = reducedGraph.getAllShortestPathBellmanFord(graph.Start);

            string target = graph.Goal;
            IEnumerable<CompositeEdge<string>> path;

            if (tryGetPaths(target, out path))
            {
                var fullPath = new List<EdgeWithId<string>>();
                foreach (var compPath in path)
                {
                    foreach (var edge in compPath.ComponentEdges)
                    {
                        fullPath.Add(edge);
                    }
                }
                GraphDrawer<string, EdgeWithId<string>> graphDrawer = new GraphDrawer<string, EdgeWithId<string>>(graph);
                EdgeListGraph<string, EdgeWithId<string>> highlightedPath = new EdgeListGraph<string, EdgeWithId<string>>();
                highlightedPath.AddVerticesAndEdgeRange(fullPath);
                graphDrawer.Path = highlightedPath;
                graphDrawer.DrawGraph();
            }
            else
            {
                Console.WriteLine("No path found");
            }
        }
    }
}
