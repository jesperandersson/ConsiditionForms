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
            Graph<string, Edge<string>> graph = CreateGraph();
            string sourceVertex = "I";
            string destinationVertex = "A";

            var adjacencyGraph = graph.AdjacencyGraph;

            Func<Edge<string>, double> edgeCost = AlgorithmExtensions.GetIndexer(graph.EdgeCosts);
            TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths = adjacencyGraph.ShortestPathsDijkstra(edgeCost, sourceVertex);

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

        public static Graph<string, Edge<string>> CreateGraph()
        {
            AdjacencyGraph<string, Edge<string>> adjacencyGraph = new AdjacencyGraph<string, Edge<string>>(true);

            // Add some vertices to the graph
            adjacencyGraph.AddVertex("A");
            adjacencyGraph.AddVertex("B");
            adjacencyGraph.AddVertex("C");
            adjacencyGraph.AddVertex("D");
            adjacencyGraph.AddVertex("E");
            adjacencyGraph.AddVertex("F");
            adjacencyGraph.AddVertex("G");
            adjacencyGraph.AddVertex("H");
            adjacencyGraph.AddVertex("I");
            adjacencyGraph.AddVertex("J");

            // Create the edges
            Edge<string> a_b = new Edge<string>("A", "B");
            Edge<string> a_d = new Edge<string>("A", "D");
            Edge<string> b_a = new Edge<string>("B", "A");
            Edge<string> b_c = new Edge<string>("B", "C");
            Edge<string> b_e = new Edge<string>("B", "E");
            Edge<string> c_b = new Edge<string>("C", "B");
            Edge<string> c_f = new Edge<string>("C", "F");
            Edge<string> c_j = new Edge<string>("C", "J");
            Edge<string> d_e = new Edge<string>("D", "E");
            Edge<string> d_g = new Edge<string>("D", "G");
            Edge<string> e_d = new Edge<string>("E", "D");
            Edge<string> e_f = new Edge<string>("E", "F");
            Edge<string> e_h = new Edge<string>("E", "H");
            Edge<string> f_i = new Edge<string>("F", "I");
            Edge<string> f_j = new Edge<string>("F", "J");
            Edge<string> g_d = new Edge<string>("G", "D");
            Edge<string> g_h = new Edge<string>("G", "H");
            Edge<string> h_g = new Edge<string>("H", "G");
            Edge<string> h_i = new Edge<string>("H", "I");
            Edge<string> i_f = new Edge<string>("I", "F");
            Edge<string> i_j = new Edge<string>("I", "J");
            Edge<string> i_h = new Edge<string>("I", "H");
            Edge<string> j_f = new Edge<string>("J", "F");

            // Add the edges
            adjacencyGraph.AddEdge(a_b);
            adjacencyGraph.AddEdge(a_d);
            adjacencyGraph.AddEdge(b_a);
            adjacencyGraph.AddEdge(b_c);
            adjacencyGraph.AddEdge(b_e);
            adjacencyGraph.AddEdge(c_b);
            adjacencyGraph.AddEdge(c_f);
            adjacencyGraph.AddEdge(c_j);
            adjacencyGraph.AddEdge(d_e);
            adjacencyGraph.AddEdge(d_g);
            adjacencyGraph.AddEdge(e_d);
            adjacencyGraph.AddEdge(e_f);
            adjacencyGraph.AddEdge(e_h);
            adjacencyGraph.AddEdge(f_i);
            adjacencyGraph.AddEdge(f_j);
            adjacencyGraph.AddEdge(g_d);
            adjacencyGraph.AddEdge(g_h);
            adjacencyGraph.AddEdge(h_g);
            adjacencyGraph.AddEdge(h_i);
            adjacencyGraph.AddEdge(i_f);
            adjacencyGraph.AddEdge(i_h);
            adjacencyGraph.AddEdge(i_j);
            adjacencyGraph.AddEdge(j_f);

            // Define some weights to the edges
            Dictionary<Edge<string>, double> edgeCosts = new Dictionary<Edge<string>, double>(adjacencyGraph.EdgeCount);
            edgeCosts.Add(a_b, 4);
            edgeCosts.Add(a_d, 1);
            edgeCosts.Add(b_a, 74);
            edgeCosts.Add(b_c, 2);
            edgeCosts.Add(b_e, 12);
            edgeCosts.Add(c_b, 12);
            edgeCosts.Add(c_f, 74);
            edgeCosts.Add(c_j, 12);
            edgeCosts.Add(d_e, 32);
            edgeCosts.Add(d_g, 22);
            edgeCosts.Add(e_d, 66);
            edgeCosts.Add(e_f, 76);
            edgeCosts.Add(e_h, 33);
            edgeCosts.Add(f_i, 11);
            edgeCosts.Add(f_j, 21);
            edgeCosts.Add(g_d, 12);
            edgeCosts.Add(g_h, 10);
            edgeCosts.Add(h_g, 2);
            edgeCosts.Add(h_i, 72);
            edgeCosts.Add(i_f, 31);
            edgeCosts.Add(i_h, 18);
            edgeCosts.Add(i_j, 7);
            edgeCosts.Add(j_f, 8);

            Graph<string, Edge<string>> graph = new Graph<string, Edge<string>>(adjacencyGraph, edgeCosts);

            return graph;
        }
    }
}
