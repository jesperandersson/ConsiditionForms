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
            Graph<string, Edge<string>> graph = GraphCreator.CreateGraphTestGeneral();
            string sourceVertex = "A";
            string destinationVertex = "I";

            TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths = graph.getAllShortestPath(sourceVertex);

            string target = destinationVertex;
            IEnumerable<Edge<string>> path;
            if (tryGetPaths(target, out path))
                foreach (var edge in path)
                    Console.WriteLine(edge);

            if (path != null)
            {
                GraphDrawer<string, Edge<string>> graphDrawer = new GraphDrawer<string, Edge<string>>(graph);
                EdgeListGraph<string, Edge<string>> highlightedPath = new EdgeListGraph<string, Edge<string>>();
                highlightedPath.AddVerticesAndEdgeRange(path);
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
