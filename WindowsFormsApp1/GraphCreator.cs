using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GraphCreator
    {
        public static Graph<string, Edge<string>> CreateGraphTestGeneral()
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

        public static Graph<string, EdgeWithId<string>> ParseGraph(string filename)
        {
            var bonusCityTimeCost = 1;
            var bonusCityReward = 5;
            Graph<string, EdgeWithId<string>> graph = new Graph<string, EdgeWithId<string>>();
            Dictionary<string, double> bonusCities = new Dictionary<string, double>();


            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                string type = line.Split(' ')[0];
                string content = String.Join(" ", line.Split(' ').Skip(1).ToArray());

                switch (type)
                {
                    case "START":
                        graph.Start = content;
                        graph.AddVertex(content);
                        break;
                    case "GOAL":
                        graph.Goal = content;
                        graph.AddVertex(content);
                        break;
                    case "CITY":
                        graph.AddVertex(content);
                        break;
                    case "BONUS":
                        var bonusVertexName = content + "_task";
                        graph.AddVertex(bonusVertexName);
                        bonusCities[bonusVertexName] = bonusCityReward;
                        break;
                    case "EDGE":
                        string[] edgeInformation = content.Split(';');
                        string source = edgeInformation[0];
                        string target = edgeInformation[1];
                        double cost = Convert.ToDouble(edgeInformation[2].Replace('.',','));
                        string transportationMode = "Tåg";
                        EdgeWithId<string> edge = new EdgeWithId<string>(source, target, transportationMode, cost, cost/2);
                        graph.AddEdge(edge, edge.GetAlgorithmCost());
                        break;
                    default:
                        throw new FormatException("Line is not allowed to start with: " + type);
                }
            }

            List<EdgeWithId<string>> edges = new List<EdgeWithId<string>>(graph.AdjacencyGraph.Edges);

            // Add edges to bonus vertex
            foreach (var edge in edges)
            {
                foreach (var bonusCity in bonusCities)
                {
                    var bonusCityName = bonusCity.Key.Split('_')[0];
                    if (edge.Target.Equals(bonusCityName))
                    {
                        var timeCost = edge.TimeCost + bonusCityTimeCost;
                        var toBonusCityEdge = new EdgeWithId<string>(edge.Source, bonusCity.Key, edge.Id, edge.EmissionCost, timeCost);
                        var fromBonusCityEdge = new EdgeWithId<string>(bonusCity.Key, edge.Source, edge.Id, edge.EmissionCost, edge.TimeCost);
                        graph.AddEdge(toBonusCityEdge, toBonusCityEdge.GetAlgorithmCost());
                        graph.AddEdge(fromBonusCityEdge, fromBonusCityEdge.GetAlgorithmCost());
                    }
                }
            }

            graph.BonusVertices = bonusCities;

            return graph;
        }

        public static Graph<string, EdgeWithId<string>> CreateGraphWithBonusCities()
        {
            Graph<string, EdgeWithId<string>> graph = new Graph<string, EdgeWithId<string>>();

            // Add some vertices to the graph
            string start = "Stockholm";
            string goal = "Dubai";
            graph.AddVertex(start);
            graph.AddVertex("London");
            graph.AddVertex("Moskva");
            graph.AddVertex("Istanbul");
            graph.AddVertex("Kairo");
            graph.AddVertex("Kabul");
            graph.AddVertex(goal);

            // Add bonus vertices
            Dictionary<string, double> bonusVertices = new Dictionary<string, double>
            {
                { "London", 400 },
                { "Kabul", 800 }
            };
            graph.BonusVertices = bonusVertices;

            var walk = "Gå";
            var plane = "Flyg";
            // Create the edges
            graph.AddEdge(new EdgeWithId<string>("Stockholm", "London", plane, 100, 90), 190);
            graph.AddEdge(new EdgeWithId<string>("Stockholm", "London" , walk, 600, 400), 1000);
            graph.AddEdge(new EdgeWithId<string>("Stockholm", "Moskva", plane, 100, 58), 158);
            graph.AddEdge(new EdgeWithId<string>("Stockholm", "Istanbul", plane, 200, 127), 327);
            graph.AddEdge(new EdgeWithId<string>("London", "Stockholm", plane, 100, 190), 190);
            graph.AddEdge(new EdgeWithId<string>("London", "Istanbul", plane, 200, 105), 305);
            graph.AddEdge(new EdgeWithId<string>("London", "Kairo", plane, 300, 262), 562);
            graph.AddEdge(new EdgeWithId<string>("Moskva", "Stockholm", plane, 100, 58), 158);
            graph.AddEdge(new EdgeWithId<string>("Moskva", "Istanbul", plane, 150, 66), 216);
            graph.AddEdge(new EdgeWithId<string>("Moskva", "Kabul", plane, 300, 111), 411);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "Stockholm", plane, 200, 127), 327);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "London", plane, 200, 105), 305);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "Moskva", plane, 150, 66), 216);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "Kairo", plane, 150, 112), 262);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "Kabul", plane, 300, 146), 446);
            graph.AddEdge(new EdgeWithId<string>("Istanbul", "Dubai", plane, 300, 115), 415);
            graph.AddEdge(new EdgeWithId<string>("Kairo", "London", plane, 300, 262), 562);
            graph.AddEdge(new EdgeWithId<string>("Kairo", "Istanbul", plane, 150, 112), 262);
            graph.AddEdge(new EdgeWithId<string>("Kairo", "Dubai", plane, 200, 107), 307);
            graph.AddEdge(new EdgeWithId<string>("Kabul", "Moskva", plane, 300, 111), 411);
            graph.AddEdge(new EdgeWithId<string>("Kabul", "Istanbul", plane, 300, 146), 446);
            graph.AddEdge(new EdgeWithId<string>("Kabul", "Dubai", plane, 300, 112), 412);
            graph.AddEdge(new EdgeWithId<string>("Dubai", "Istanbul", plane, 300, 115), 415);
            graph.AddEdge(new EdgeWithId<string>("Dubai", "Kairo", plane, 200, 107), 307);
            graph.AddEdge(new EdgeWithId<string>("Dubai", "Kabul", plane, 300, 112), 412);

            graph.Start = start;
            graph.Goal = goal;

            return graph;
        }

        public static Graph<string, EdgeWithId<string>> CreateBigGraph()
        {
            Graph<string, EdgeWithId<string>> graph = new Graph<string, EdgeWithId<string>>();
            string start = "2673730";
            graph.Start = start;
            string goal = "292223";
            graph.Goal = goal;
            graph.AddVertex("2673730");
            graph.AddVertex("292223");
            graph.AddVertex("108410");
            graph.AddVertex("501175");
            graph.AddVertex("2210247");
            graph.AddVertex("379252");
            graph.AddVertex("703448");
            graph.AddVertex("683506");
            graph.AddVertex("524901");
            graph.AddVertex("706483");
            graph.AddVertex("104515");
            graph.AddVertex("472757");
            graph.AddVertex("2306104");
            graph.AddVertex("727011");
            graph.AddVertex("2911298");
            graph.AddVertex("2317765");
            graph.AddVertex("388349");
            graph.AddVertex("611717");
            graph.AddVertex("2293538");
            graph.AddVertex("3054643");
            graph.AddVertex("3128760");
            graph.AddVertex("2253354");
            graph.AddVertex("625144");
            graph.AddVertex("2800866");
            graph.AddVertex("418863");
            graph.AddVertex("2220957");
            graph.AddVertex("499099");
            graph.AddVertex("756135");
            graph.AddVertex("698740");
            graph.AddVertex("2964574");
            graph.AddVertex("325363");
            graph.AddVertex("2988507");
            graph.AddVertex("745044");
            graph.AddVertex("124665");
            graph.AddVertex("170063");
            graph.AddVertex("2643743");
            graph.AddVertex("709930");
            graph.AddVertex("2422488");
            graph.AddVertex("344979");
            graph.AddVertex("71137");
            graph.AddVertex("709717");
            graph.AddVertex("2331447");
            graph.AddVertex("53654");
            graph.AddVertex("2347283");
            graph.AddVertex("112931");
            graph.AddVertex("1486209");
            graph.AddVertex("1508291");
            graph.AddVertex("311046");
            graph.AddVertex("2335204");
            graph.AddVertex("99072");
            graph.AddVertex("105343");
            graph.AddVertex("360995");
            graph.AddVertex("3169070");
            graph.AddVertex("113646");
            graph.AddVertex("3067696");
            graph.AddVertex("498817");
            graph.AddVertex("3173435");
            graph.AddVertex("3117735");
            graph.AddVertex("323786");
            graph.AddVertex("520555");
            graph.AddVertex("2548885");
            graph.AddVertex("250441");
            graph.AddVertex("2232593");
            graph.AddVertex("276781");
            graph.AddVertex("2357048");
            graph.AddVertex("2335727");
            graph.AddVertex("551487");
            graph.AddVertex("587084");
            graph.AddVertex("2298890");
            graph.AddVertex("2655603");
            graph.AddVertex("98182");
            graph.AddVertex("616052");
            graph.AddVertex("128747");
            graph.AddVertex("511196");
            graph.AddVertex("2507480");
            graph.AddVertex("792680");
            graph.AddVertex("115019");
            graph.AddVertex("170654");
            graph.AddVertex("479561");
            graph.AddVertex("99071");
            graph.AddVertex("2553604");
            graph.AddVertex("268743");
            graph.AddVertex("232422");
            graph.AddVertex("2339354");
            graph.AddVertex("109223");
            graph.AddVertex("314830");
            graph.AddVertex("99532");
            graph.AddVertex("2422465");
            graph.AddVertex("2867714");
            graph.AddVertex("2618425");
            graph.AddVertex("2761369");
            graph.AddVertex("2460596");
            graph.AddVertex("2332459");
            graph.AddVertex("365137");
            graph.AddVertex("2324774");
            graph.AddVertex("361058");
            graph.AddVertex("750269");
            graph.AddVertex("360630");
            graph.AddVertex("2950159");
            graph.AddVertex("2538475");
            Dictionary<string, double> bonusVertices = new Dictionary<string, double>
            {
                { "98182", 1 },
                { "616052", 1 },
                { "128747", 1 },
                { "511196", 1 },
                { "2507480", 1 },
                { "792680", 1 },
                { "115019", 1 },
                { "170654", 1 },
                { "479561", 1 },
                { "99071", 1 },
                { "2553604", 1 },
                { "268743", 1 },
                { "232422", 1 },
                { "2339354", 1 },
                { "109223", 1 },
                { "314830", 1 },
                { "99532", 1 },
                { "2422465", 1 },
                { "2867714", 1 },
                { "2618425", 1 },
                { "2761369", 1 },
                { "2460596", 1 },
                { "2332459", 1 },
                { "365137", 1 },
                { "2324774", 1 },
                { "361058", 1 },
                { "750269", 1 },
                { "360630", 1 },
                { "2950159", 1 },
                { "2538475", 1 }
            };
            graph.BonusVertices = bonusVertices;
            var bus = "Buss";
            var plane = "Flyg";
            graph.AddEdge(new EdgeWithId<string>("108410", "99532", bus), 5.91625931874187);
            graph.AddEdge(new EdgeWithId<string>("99532", "108410", bus), 5.91625931874187);
            graph.AddEdge(new EdgeWithId<string>("108410", "388349", bus), 5.917292966661361);
            graph.AddEdge(new EdgeWithId<string>("388349", "108410", bus), 5.917292966661361);
            graph.AddEdge(new EdgeWithId<string>("108410", "109223", bus), 7.111056781998019);
            graph.AddEdge(new EdgeWithId<string>("109223", "108410", bus), 7.111056781998019);
            graph.AddEdge(new EdgeWithId<string>("108410", "115019", bus), 7.614428932940403);
            graph.AddEdge(new EdgeWithId<string>("115019", "108410", bus), 7.614428932940403);
            graph.AddEdge(new EdgeWithId<string>("501175", "709717", bus), 2.0777642802060163);
            graph.AddEdge(new EdgeWithId<string>("709717", "501175", bus), 2.0777642802060163);
            graph.AddEdge(new EdgeWithId<string>("501175", "706483", bus), 4.427676253431368);
            graph.AddEdge(new EdgeWithId<string>("706483", "501175", bus), 4.427676253431368);
            graph.AddEdge(new EdgeWithId<string>("501175", "709930", bus), 4.842893705151916);
            graph.AddEdge(new EdgeWithId<string>("709930", "501175", bus), 4.842893705151916);
            graph.AddEdge(new EdgeWithId<string>("501175", "472757", bus), 5.004877934985023);
            graph.AddEdge(new EdgeWithId<string>("472757", "501175", bus), 5.004877934985023);
            graph.AddEdge(new EdgeWithId<string>("2210247", "3169070", bus), 9.042054634014328);
            graph.AddEdge(new EdgeWithId<string>("3169070", "2210247", bus), 9.042054634014328);
            graph.AddEdge(new EdgeWithId<string>("2210247", "2507480", bus), 10.811425060721643);
            graph.AddEdge(new EdgeWithId<string>("2507480", "2210247", bus), 10.811425060721643);
            graph.AddEdge(new EdgeWithId<string>("2210247", "3173435", bus), 13.20865396052527);
            graph.AddEdge(new EdgeWithId<string>("3173435", "2210247", bus), 13.20865396052527);
            graph.AddEdge(new EdgeWithId<string>("2210247", "3128760", bus), 13.93228392981208);
            graph.AddEdge(new EdgeWithId<string>("3128760", "2210247", bus), 13.93228392981208);
            graph.AddEdge(new EdgeWithId<string>("379252", "365137", bus), 0.10767692417598011);
            graph.AddEdge(new EdgeWithId<string>("365137", "379252", bus), 0.10767692417598011);
            graph.AddEdge(new EdgeWithId<string>("379252", "105343", bus), 8.961980712191922);
            graph.AddEdge(new EdgeWithId<string>("105343", "379252", bus), 8.961980712191922);
            graph.AddEdge(new EdgeWithId<string>("379252", "344979", bus), 9.012151791353718);
            graph.AddEdge(new EdgeWithId<string>("344979", "379252", bus), 9.012151791353718);
            graph.AddEdge(new EdgeWithId<string>("379252", "104515", bus), 9.36510306858926);
            graph.AddEdge(new EdgeWithId<string>("104515", "379252", bus), 9.36510306858926);
            graph.AddEdge(new EdgeWithId<string>("703448", "698740", bus), 3.9826682122039743);
            graph.AddEdge(new EdgeWithId<string>("698740", "703448", bus), 3.9826682122039743);
            graph.AddEdge(new EdgeWithId<string>("703448", "625144", bus), 4.540372842895177);
            graph.AddEdge(new EdgeWithId<string>("625144", "703448", bus), 4.540372842895177);
            graph.AddEdge(new EdgeWithId<string>("703448", "709930", bus), 4.936125206282753);
            graph.AddEdge(new EdgeWithId<string>("709930", "703448", bus), 4.936125206282753);
            graph.AddEdge(new EdgeWithId<string>("703448", "706483", bus), 5.748483120693662);
            graph.AddEdge(new EdgeWithId<string>("706483", "703448", bus), 5.748483120693662);
            graph.AddEdge(new EdgeWithId<string>("683506", "727011", bus), 3.2786367471404945);
            graph.AddEdge(new EdgeWithId<string>("727011", "683506", bus), 3.2786367471404945);
            graph.AddEdge(new EdgeWithId<string>("683506", "745044", bus), 4.4463974730224045);
            graph.AddEdge(new EdgeWithId<string>("745044", "683506", bus), 4.4463974730224045);
            graph.AddEdge(new EdgeWithId<string>("683506", "698740", bus), 5.058273588686163);
            graph.AddEdge(new EdgeWithId<string>("698740", "683506", bus), 5.058273588686163);
            graph.AddEdge(new EdgeWithId<string>("683506", "750269", bus), 5.164749358148953);
            graph.AddEdge(new EdgeWithId<string>("750269", "683506", bus), 5.164749358148953);
            graph.AddEdge(new EdgeWithId<string>("524901", "706483", bus), 5.930135432998141);
            graph.AddEdge(new EdgeWithId<string>("706483", "524901", bus), 5.930135432998141);
            graph.AddEdge(new EdgeWithId<string>("524901", "520555", bus), 6.412452660456834);
            graph.AddEdge(new EdgeWithId<string>("520555", "524901", bus), 6.412452660456834);
            graph.AddEdge(new EdgeWithId<string>("524901", "709717", bus), 7.731474065842812);
            graph.AddEdge(new EdgeWithId<string>("709717", "524901", bus), 7.731474065842812);
            graph.AddEdge(new EdgeWithId<string>("524901", "709930", bus), 7.734801049445297);
            graph.AddEdge(new EdgeWithId<string>("709930", "524901", bus), 7.734801049445297);
            graph.AddEdge(new EdgeWithId<string>("706483", "709930", bus), 1.946524760952194);
            graph.AddEdge(new EdgeWithId<string>("709930", "706483", bus), 1.946524760952194);
            graph.AddEdge(new EdgeWithId<string>("104515", "105343", bus), 0.6382419785629891);
            graph.AddEdge(new EdgeWithId<string>("105343", "104515", bus), 0.6382419785629891);
            graph.AddEdge(new EdgeWithId<string>("104515", "109223", bus), 3.049310875017504);
            graph.AddEdge(new EdgeWithId<string>("109223", "104515", bus), 3.049310875017504);
            graph.AddEdge(new EdgeWithId<string>("104515", "71137", bus), 7.4874377438480275);
            graph.AddEdge(new EdgeWithId<string>("71137", "104515", bus), 7.4874377438480275);
            graph.AddEdge(new EdgeWithId<string>("472757", "709717", bus), 6.735685948750877);
            graph.AddEdge(new EdgeWithId<string>("709717", "472757", bus), 6.735685948750877);
            graph.AddEdge(new EdgeWithId<string>("472757", "611717", bus), 7.0331133576034395);
            graph.AddEdge(new EdgeWithId<string>("611717", "472757", bus), 7.0331133576034395);
            graph.AddEdge(new EdgeWithId<string>("472757", "499099", bus), 7.209598991018849);
            graph.AddEdge(new EdgeWithId<string>("499099", "472757", bus), 7.209598991018849);
            graph.AddEdge(new EdgeWithId<string>("2306104", "2298890", bus), 1.8221711095558508);
            graph.AddEdge(new EdgeWithId<string>("2298890", "2306104", bus), 1.8221711095558508);
            graph.AddEdge(new EdgeWithId<string>("2306104", "2332459", bus), 3.702143820464029);
            graph.AddEdge(new EdgeWithId<string>("2332459", "2306104", bus), 3.702143820464029);
            graph.AddEdge(new EdgeWithId<string>("2306104", "2293538", bus), 3.823704699267453);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2306104", bus), 3.823704699267453);
            graph.AddEdge(new EdgeWithId<string>("2306104", "2339354", bus), 4.48899296810543);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2306104", bus), 4.48899296810543);
            graph.AddEdge(new EdgeWithId<string>("727011", "792680", bus), 3.551244515715581);
            graph.AddEdge(new EdgeWithId<string>("792680", "727011", bus), 3.551244515715581);
            graph.AddEdge(new EdgeWithId<string>("727011", "311046", bus), 5.736522478061077);
            graph.AddEdge(new EdgeWithId<string>("311046", "727011", bus), 5.736522478061077);
            graph.AddEdge(new EdgeWithId<string>("727011", "745044", bus), 5.87206159955769);
            graph.AddEdge(new EdgeWithId<string>("745044", "727011", bus), 5.87206159955769);
            graph.AddEdge(new EdgeWithId<string>("2911298", "2618425", bus), 3.3039481564485844);
            graph.AddEdge(new EdgeWithId<string>("2618425", "2911298", bus), 3.3039481564485844);
            graph.AddEdge(new EdgeWithId<string>("2911298", "2950159", bus), 3.5541259176624562);
            graph.AddEdge(new EdgeWithId<string>("2950159", "2911298", bus), 3.5541259176624562);
            graph.AddEdge(new EdgeWithId<string>("2911298", "3067696", bus), 5.618616126307258);
            graph.AddEdge(new EdgeWithId<string>("3067696", "2911298", bus), 5.618616126307258);
            graph.AddEdge(new EdgeWithId<string>("2911298", "2867714", bus), 5.65727104482364);
            graph.AddEdge(new EdgeWithId<string>("2867714", "2911298", bus), 5.65727104482364);
            graph.AddEdge(new EdgeWithId<string>("2317765", "2335727", bus), 0.650136758690047);
            graph.AddEdge(new EdgeWithId<string>("2335727", "2317765", bus), 0.650136758690047);
            graph.AddEdge(new EdgeWithId<string>("2317765", "2335204", bus), 1.191849112094312);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2317765", bus), 1.191849112094312);
            graph.AddEdge(new EdgeWithId<string>("2317765", "2347283", bus), 5.213441214725645);
            graph.AddEdge(new EdgeWithId<string>("2347283", "2317765", bus), 5.213441214725645);
            graph.AddEdge(new EdgeWithId<string>("2317765", "2339354", bus), 5.339339935094974);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2317765", bus), 5.339339935094974);
            graph.AddEdge(new EdgeWithId<string>("388349", "99532", bus), 0.035081882788697165);
            graph.AddEdge(new EdgeWithId<string>("99532", "388349", bus), 0.035081882788697165);
            graph.AddEdge(new EdgeWithId<string>("388349", "418863", bus), 4.417648321505457);
            graph.AddEdge(new EdgeWithId<string>("418863", "388349", bus), 4.417648321505457);
            graph.AddEdge(new EdgeWithId<string>("388349", "98182", bus), 4.439329410226279);
            graph.AddEdge(new EdgeWithId<string>("98182", "388349", bus), 4.439329410226279);
            graph.AddEdge(new EdgeWithId<string>("611717", "616052", bus), 1.5464843371014194);
            graph.AddEdge(new EdgeWithId<string>("616052", "611717", bus), 1.5464843371014194);
            graph.AddEdge(new EdgeWithId<string>("611717", "113646", bus), 3.897203697588826);
            graph.AddEdge(new EdgeWithId<string>("113646", "611717", bus), 3.897203697588826);
            graph.AddEdge(new EdgeWithId<string>("611717", "587084", bus), 5.226826634058181);
            graph.AddEdge(new EdgeWithId<string>("587084", "611717", bus), 5.226826634058181);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2298890", bus), 2.757677850166694);
            graph.AddEdge(new EdgeWithId<string>("2298890", "2293538", bus), 2.757677850166694);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2357048", bus), 7.4787356076010605);
            graph.AddEdge(new EdgeWithId<string>("2357048", "2293538", bus), 7.4787356076010605);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2332459", bus), 7.495212603855878);
            graph.AddEdge(new EdgeWithId<string>("2332459", "2293538", bus), 7.495212603855878);
            graph.AddEdge(new EdgeWithId<string>("3054643", "2761369", bus), 2.76081486871177);
            graph.AddEdge(new EdgeWithId<string>("2761369", "3054643", bus), 2.76081486871177);
            graph.AddEdge(new EdgeWithId<string>("3054643", "792680", bus), 3.047767715624013);
            graph.AddEdge(new EdgeWithId<string>("792680", "3054643", bus), 3.047767715624013);
            graph.AddEdge(new EdgeWithId<string>("3054643", "756135", bus), 5.126190007647008);
            graph.AddEdge(new EdgeWithId<string>("756135", "3054643", bus), 5.126190007647008);
            graph.AddEdge(new EdgeWithId<string>("3054643", "3067696", bus), 5.295734332781431);
            graph.AddEdge(new EdgeWithId<string>("3067696", "3054643", bus), 5.295734332781431);
            graph.AddEdge(new EdgeWithId<string>("3128760", "2507480", bus), 4.74820190308921);
            graph.AddEdge(new EdgeWithId<string>("2507480", "3128760", bus), 4.74820190308921);
            graph.AddEdge(new EdgeWithId<string>("3128760", "3117735", bus), 5.941642554597172);
            graph.AddEdge(new EdgeWithId<string>("3117735", "3128760", bus), 5.941642554597172);
            graph.AddEdge(new EdgeWithId<string>("3128760", "2988507", bus), 7.467032849833992);
            graph.AddEdge(new EdgeWithId<string>("2988507", "3128760", bus), 7.467032849833992);
            graph.AddEdge(new EdgeWithId<string>("2253354", "2422488", bus), 6.381365459554875);
            graph.AddEdge(new EdgeWithId<string>("2422488", "2253354", bus), 6.381365459554875);
            graph.AddEdge(new EdgeWithId<string>("2253354", "2422465", bus), 6.385163607567154);
            graph.AddEdge(new EdgeWithId<string>("2422465", "2253354", bus), 6.385163607567154);
            graph.AddEdge(new EdgeWithId<string>("2253354", "2460596", bus), 9.662659001206656);
            graph.AddEdge(new EdgeWithId<string>("2460596", "2253354", bus), 9.662659001206656);
            graph.AddEdge(new EdgeWithId<string>("2253354", "2357048", bus), 16.079601919015285);
            graph.AddEdge(new EdgeWithId<string>("2357048", "2253354", bus), 16.079601919015285);
            graph.AddEdge(new EdgeWithId<string>("625144", "498817", bus), 6.634273790589296);
            graph.AddEdge(new EdgeWithId<string>("498817", "625144", bus), 6.634273790589296);
            graph.AddEdge(new EdgeWithId<string>("625144", "756135", bus), 6.764336712864015);
            graph.AddEdge(new EdgeWithId<string>("756135", "625144", bus), 6.764336712864015);
            graph.AddEdge(new EdgeWithId<string>("625144", "698740", bus), 8.069522352865754);
            graph.AddEdge(new EdgeWithId<string>("698740", "625144", bus), 8.069522352865754);
            graph.AddEdge(new EdgeWithId<string>("2800866", "2988507", bus), 2.8263207111012756);
            graph.AddEdge(new EdgeWithId<string>("2988507", "2800866", bus), 2.8263207111012756);
            graph.AddEdge(new EdgeWithId<string>("2800866", "2643743", bus), 4.522653924058306);
            graph.AddEdge(new EdgeWithId<string>("2643743", "2800866", bus), 4.522653924058306);
            graph.AddEdge(new EdgeWithId<string>("2800866", "2655603", bus), 6.457955564495623);
            graph.AddEdge(new EdgeWithId<string>("2655603", "2800866", bus), 6.457955564495623);
            graph.AddEdge(new EdgeWithId<string>("2800866", "3173435", bus), 7.2417954904360595);
            graph.AddEdge(new EdgeWithId<string>("3173435", "2800866", bus), 7.2417954904360595);
            graph.AddEdge(new EdgeWithId<string>("418863", "112931", bus), 3.0524421037916514);
            graph.AddEdge(new EdgeWithId<string>("112931", "418863", bus), 3.0524421037916514);
            graph.AddEdge(new EdgeWithId<string>("418863", "115019", bus), 3.160424971835275);
            graph.AddEdge(new EdgeWithId<string>("115019", "418863", bus), 3.160424971835275);
            graph.AddEdge(new EdgeWithId<string>("418863", "128747", bus), 3.252730647456072);
            graph.AddEdge(new EdgeWithId<string>("128747", "418863", bus), 3.252730647456072);
            graph.AddEdge(new EdgeWithId<string>("2220957", "2232593", bus), 1.8214653639583696);
            graph.AddEdge(new EdgeWithId<string>("2232593", "2220957", bus), 1.8214653639583696);
            graph.AddEdge(new EdgeWithId<string>("2220957", "2324774", bus), 4.59444297553033);
            graph.AddEdge(new EdgeWithId<string>("2324774", "2220957", bus), 4.59444297553033);
            graph.AddEdge(new EdgeWithId<string>("2220957", "2347283", bus), 6.388360653313179);
            graph.AddEdge(new EdgeWithId<string>("2347283", "2220957", bus), 6.388360653313179);
            graph.AddEdge(new EdgeWithId<string>("2220957", "2335727", bus), 7.809048736049737);
            graph.AddEdge(new EdgeWithId<string>("2335727", "2220957", bus), 7.809048736049737);
            graph.AddEdge(new EdgeWithId<string>("499099", "551487", bus), 2.785266333494877);
            graph.AddEdge(new EdgeWithId<string>("551487", "499099", bus), 2.785266333494877);
            graph.AddEdge(new EdgeWithId<string>("499099", "479561", bus), 6.018928361776707);
            graph.AddEdge(new EdgeWithId<string>("479561", "499099", bus), 6.018928361776707);
            graph.AddEdge(new EdgeWithId<string>("499099", "520555", bus), 6.898219129782705);
            graph.AddEdge(new EdgeWithId<string>("520555", "499099", bus), 6.898219129782705);
            graph.AddEdge(new EdgeWithId<string>("756135", "2761369", bus), 6.139829714935101);
            graph.AddEdge(new EdgeWithId<string>("2761369", "756135", bus), 6.139829714935101);
            graph.AddEdge(new EdgeWithId<string>("756135", "3067696", bus), 6.9302634894569515);
            graph.AddEdge(new EdgeWithId<string>("3067696", "756135", bus), 6.9302634894569515);
            graph.AddEdge(new EdgeWithId<string>("698740", "745044", bus), 5.747190543082763);
            graph.AddEdge(new EdgeWithId<string>("745044", "698740", bus), 5.747190543082763);
            graph.AddEdge(new EdgeWithId<string>("2964574", "2655603", bus), 4.431660363024226);
            graph.AddEdge(new EdgeWithId<string>("2655603", "2964574", bus), 4.431660363024226);
            graph.AddEdge(new EdgeWithId<string>("2964574", "2643743", bus), 6.389199922009015);
            graph.AddEdge(new EdgeWithId<string>("2643743", "2964574", bus), 6.389199922009015);
            graph.AddEdge(new EdgeWithId<string>("2964574", "2988507", bus), 9.694716987029588);
            graph.AddEdge(new EdgeWithId<string>("2988507", "2964574", bus), 9.694716987029588);
            graph.AddEdge(new EdgeWithId<string>("2964574", "3117735", bus), 13.165155475819498);
            graph.AddEdge(new EdgeWithId<string>("3117735", "2964574", bus), 13.165155475819498);
            graph.AddEdge(new EdgeWithId<string>("325363", "170063", bus), 1.9994844793846207);
            graph.AddEdge(new EdgeWithId<string>("170063", "325363", bus), 1.9994844793846207);
            graph.AddEdge(new EdgeWithId<string>("325363", "314830", bus), 2.0544224017956965);
            graph.AddEdge(new EdgeWithId<string>("314830", "325363", bus), 2.0544224017956965);
            graph.AddEdge(new EdgeWithId<string>("325363", "268743", bus), 3.105512598992313);
            graph.AddEdge(new EdgeWithId<string>("268743", "325363", bus), 3.105512598992313);
            graph.AddEdge(new EdgeWithId<string>("325363", "276781", bus), 3.1171282029778618);
            graph.AddEdge(new EdgeWithId<string>("276781", "325363", bus), 3.1171282029778618);
            graph.AddEdge(new EdgeWithId<string>("2988507", "2643743", bus), 3.629464206463541);
            graph.AddEdge(new EdgeWithId<string>("2643743", "2988507", bus), 3.629464206463541);
            graph.AddEdge(new EdgeWithId<string>("745044", "750269", bus), 0.8256734726270417);
            graph.AddEdge(new EdgeWithId<string>("750269", "745044", bus), 0.8256734726270417);
            graph.AddEdge(new EdgeWithId<string>("124665", "112931", bus), 8.170100185585243);
            graph.AddEdge(new EdgeWithId<string>("112931", "124665", bus), 8.170100185585243);
            graph.AddEdge(new EdgeWithId<string>("124665", "128747", bus), 8.589995918101478);
            graph.AddEdge(new EdgeWithId<string>("128747", "124665", bus), 8.589995918101478);
            graph.AddEdge(new EdgeWithId<string>("124665", "115019", bus), 9.719966889208012);
            graph.AddEdge(new EdgeWithId<string>("115019", "124665", bus), 9.719966889208012);
            graph.AddEdge(new EdgeWithId<string>("124665", "587084", bus), 10.494022218811052);
            graph.AddEdge(new EdgeWithId<string>("587084", "124665", bus), 10.494022218811052);
            graph.AddEdge(new EdgeWithId<string>("170063", "314830", bus), 0.8862811116683053);
            graph.AddEdge(new EdgeWithId<string>("314830", "170063", bus), 0.8862811116683053);
            graph.AddEdge(new EdgeWithId<string>("170063", "170654", bus), 2.8281451330686695);
            graph.AddEdge(new EdgeWithId<string>("170654", "170063", bus), 2.8281451330686695);
            graph.AddEdge(new EdgeWithId<string>("170063", "268743", bus), 2.8479558639838483);
            graph.AddEdge(new EdgeWithId<string>("268743", "170063", bus), 2.8479558639838483);
            graph.AddEdge(new EdgeWithId<string>("2643743", "2655603", bus), 2.0233413652174463);
            graph.AddEdge(new EdgeWithId<string>("2655603", "2643743", bus), 2.0233413652174463);
            graph.AddEdge(new EdgeWithId<string>("2422488", "2422465", bus), 0.010896907818276598);
            graph.AddEdge(new EdgeWithId<string>("2422465", "2422488", bus), 0.010896907818276598);
            graph.AddEdge(new EdgeWithId<string>("2422488", "2460596", bus), 6.48491066464296);
            graph.AddEdge(new EdgeWithId<string>("2460596", "2422488", bus), 6.48491066464296);
            graph.AddEdge(new EdgeWithId<string>("2422488", "2298890", bus), 12.39463954025691);
            graph.AddEdge(new EdgeWithId<string>("2298890", "2422488", bus), 12.39463954025691);
            graph.AddEdge(new EdgeWithId<string>("344979", "71137", bus), 8.35912272376115);
            graph.AddEdge(new EdgeWithId<string>("71137", "344979", bus), 8.35912272376115);
            graph.AddEdge(new EdgeWithId<string>("344979", "365137", bus), 9.117068701024468);
            graph.AddEdge(new EdgeWithId<string>("365137", "344979", bus), 9.117068701024468);
            graph.AddEdge(new EdgeWithId<string>("344979", "53654", bus), 9.609825661228198);
            graph.AddEdge(new EdgeWithId<string>("53654", "344979", bus), 9.609825661228198);
            graph.AddEdge(new EdgeWithId<string>("71137", "105343", bus), 7.960792169476607);
            graph.AddEdge(new EdgeWithId<string>("105343", "71137", bus), 7.960792169476607);
            graph.AddEdge(new EdgeWithId<string>("71137", "109223", bus), 10.205589016911274);
            graph.AddEdge(new EdgeWithId<string>("109223", "71137", bus), 10.205589016911274);
            graph.AddEdge(new EdgeWithId<string>("709717", "323786", bus), 9.494373224062764);
            graph.AddEdge(new EdgeWithId<string>("323786", "709717", bus), 9.494373224062764);
            graph.AddEdge(new EdgeWithId<string>("2331447", "2335204", bus), 4.6429282139615315);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2331447", bus), 4.6429282139615315);
            graph.AddEdge(new EdgeWithId<string>("2331447", "2335727", bus), 5.8688196981164795);
            graph.AddEdge(new EdgeWithId<string>("2335727", "2331447", bus), 5.8688196981164795);
            graph.AddEdge(new EdgeWithId<string>("2331447", "2232593", bus), 8.528836139128247);
            graph.AddEdge(new EdgeWithId<string>("2232593", "2331447", bus), 8.528836139128247);
            graph.AddEdge(new EdgeWithId<string>("2331447", "2347283", bus), 9.331027863520719);
            graph.AddEdge(new EdgeWithId<string>("2347283", "2331447", bus), 9.331027863520719);
            graph.AddEdge(new EdgeWithId<string>("53654", "232422", bus), 12.877059816685643);
            graph.AddEdge(new EdgeWithId<string>("232422", "53654", bus), 12.877059816685643);
            graph.AddEdge(new EdgeWithId<string>("53654", "365137", bus), 18.72688841470467);
            graph.AddEdge(new EdgeWithId<string>("365137", "53654", bus), 18.72688841470467);
            graph.AddEdge(new EdgeWithId<string>("53654", "105343", bus), 20.4505787101808);
            graph.AddEdge(new EdgeWithId<string>("105343", "53654", bus), 20.4505787101808);
            graph.AddEdge(new EdgeWithId<string>("2347283", "2339354", bus), 2.0095329740265524);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2347283", bus), 2.0095329740265524);
            graph.AddEdge(new EdgeWithId<string>("112931", "128747", bus), 0.4516460943039369);
            graph.AddEdge(new EdgeWithId<string>("128747", "112931", bus), 0.4516460943039369);
            graph.AddEdge(new EdgeWithId<string>("112931", "587084", bus), 4.9267110538776295);
            graph.AddEdge(new EdgeWithId<string>("587084", "112931", bus), 4.9267110538776295);
            graph.AddEdge(new EdgeWithId<string>("1486209", "1508291", bus), 1.8841984494474011);
            graph.AddEdge(new EdgeWithId<string>("1508291", "1486209", bus), 1.8841984494474011);
            graph.AddEdge(new EdgeWithId<string>("1486209", "511196", bus), 4.513265668504353);
            graph.AddEdge(new EdgeWithId<string>("511196", "1486209", bus), 4.513265668504353);
            graph.AddEdge(new EdgeWithId<string>("1486209", "479561", bus), 5.100759785924055);
            graph.AddEdge(new EdgeWithId<string>("479561", "1486209", bus), 5.100759785924055);
            graph.AddEdge(new EdgeWithId<string>("1486209", "551487", bus), 11.539141562057377);
            graph.AddEdge(new EdgeWithId<string>("551487", "1486209", bus), 11.539141562057377);
            graph.AddEdge(new EdgeWithId<string>("1508291", "479561", bus), 5.4768002676015115);
            graph.AddEdge(new EdgeWithId<string>("479561", "1508291", bus), 5.4768002676015115);
            graph.AddEdge(new EdgeWithId<string>("1508291", "511196", bus), 5.9144808152533574);
            graph.AddEdge(new EdgeWithId<string>("511196", "1508291", bus), 5.9144808152533574);
            graph.AddEdge(new EdgeWithId<string>("1508291", "551487", bus), 12.32336661056953);
            graph.AddEdge(new EdgeWithId<string>("551487", "1508291", bus), 12.32336661056953);
            graph.AddEdge(new EdgeWithId<string>("311046", "750269", bus), 2.621395209063295);
            graph.AddEdge(new EdgeWithId<string>("750269", "311046", bus), 2.621395209063295);
            graph.AddEdge(new EdgeWithId<string>("311046", "323786", bus), 5.911249400228346);
            graph.AddEdge(new EdgeWithId<string>("323786", "311046", bus), 5.911249400228346);
            graph.AddEdge(new EdgeWithId<string>("311046", "361058", bus), 7.728711001208418);
            graph.AddEdge(new EdgeWithId<string>("361058", "311046", bus), 7.728711001208418);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2335727", bus), 1.8258571272145037);
            graph.AddEdge(new EdgeWithId<string>("2335727", "2335204", bus), 1.8258571272145037);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2339354", bus), 6.528983826729853);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2335204", bus), 6.528983826729853);
            graph.AddEdge(new EdgeWithId<string>("99072", "99071", bus), 0.013535128370281362);
            graph.AddEdge(new EdgeWithId<string>("99071", "99072", bus), 0.013535128370281362);
            graph.AddEdge(new EdgeWithId<string>("99072", "98182", bus), 3.257307092753152);
            graph.AddEdge(new EdgeWithId<string>("98182", "99072", bus), 3.257307092753152);
            graph.AddEdge(new EdgeWithId<string>("99072", "113646", bus), 3.6211900613058106);
            graph.AddEdge(new EdgeWithId<string>("113646", "99072", bus), 3.6211900613058106);
            graph.AddEdge(new EdgeWithId<string>("99072", "616052", bus), 4.091186381784625);
            graph.AddEdge(new EdgeWithId<string>("616052", "99072", bus), 4.091186381784625);
            graph.AddEdge(new EdgeWithId<string>("360995", "360630", bus), 0.06690657740461446);
            graph.AddEdge(new EdgeWithId<string>("360630", "360995", bus), 0.06690657740461446);
            graph.AddEdge(new EdgeWithId<string>("360995", "361058", bus), 1.7420916133200368);
            graph.AddEdge(new EdgeWithId<string>("361058", "360995", bus), 1.7420916133200368);
            graph.AddEdge(new EdgeWithId<string>("360995", "250441", bus), 5.118892164287114);
            graph.AddEdge(new EdgeWithId<string>("250441", "360995", bus), 5.118892164287114);
            graph.AddEdge(new EdgeWithId<string>("360995", "268743", bus), 5.779311641225103);
            graph.AddEdge(new EdgeWithId<string>("268743", "360995", bus), 5.779311641225103);
            graph.AddEdge(new EdgeWithId<string>("3169070", "3173435", bus), 4.878124761422157);
            graph.AddEdge(new EdgeWithId<string>("3173435", "3169070", bus), 4.878124761422157);
            graph.AddEdge(new EdgeWithId<string>("3169070", "2867714", bus), 6.3152249964352025);
            graph.AddEdge(new EdgeWithId<string>("2867714", "3169070", bus), 6.3152249964352025);
            graph.AddEdge(new EdgeWithId<string>("3169070", "2761369", bus), 7.402994042689752);
            graph.AddEdge(new EdgeWithId<string>("2761369", "3169070", bus), 7.402994042689752);
            graph.AddEdge(new EdgeWithId<string>("113646", "616052", bus), 2.752631205991821);
            graph.AddEdge(new EdgeWithId<string>("616052", "113646", bus), 2.752631205991821);
            graph.AddEdge(new EdgeWithId<string>("113646", "99071", bus), 3.633985232028328);
            graph.AddEdge(new EdgeWithId<string>("99071", "113646", bus), 3.633985232028328);
            graph.AddEdge(new EdgeWithId<string>("3067696", "2950159", bus), 2.637473890259388);
            graph.AddEdge(new EdgeWithId<string>("2950159", "3067696", bus), 2.637473890259388);
            graph.AddEdge(new EdgeWithId<string>("498817", "2673730", bus), 12.264213476428072);
            graph.AddEdge(new EdgeWithId<string>("2673730", "498817", bus), 12.264213476428072);
            graph.AddEdge(new EdgeWithId<string>("498817", "520555", bus), 14.155951579741997);
            graph.AddEdge(new EdgeWithId<string>("520555", "498817", bus), 14.155951579741997);
            graph.AddEdge(new EdgeWithId<string>("498817", "792680", bus), 18.057118362141846);
            graph.AddEdge(new EdgeWithId<string>("792680", "498817", bus), 18.057118362141846);
            graph.AddEdge(new EdgeWithId<string>("3173435", "2867714", bus), 3.5831110708433274);
            graph.AddEdge(new EdgeWithId<string>("2867714", "3173435", bus), 3.5831110708433274);
            graph.AddEdge(new EdgeWithId<string>("3117735", "2548885", bus), 6.513945790018519);
            graph.AddEdge(new EdgeWithId<string>("2548885", "3117735", bus), 6.513945790018519);
            graph.AddEdge(new EdgeWithId<string>("3117735", "2538475", bus), 7.127302993601437);
            graph.AddEdge(new EdgeWithId<string>("2538475", "3117735", bus), 7.127302993601437);
            graph.AddEdge(new EdgeWithId<string>("323786", "750269", bus), 3.804145088978599);
            graph.AddEdge(new EdgeWithId<string>("750269", "323786", bus), 3.804145088978599);
            graph.AddEdge(new EdgeWithId<string>("323786", "314830", bus), 5.356017804096623);
            graph.AddEdge(new EdgeWithId<string>("314830", "323786", bus), 5.356017804096623);
            graph.AddEdge(new EdgeWithId<string>("520555", "551487", bus), 5.148479971117694);
            graph.AddEdge(new EdgeWithId<string>("551487", "520555", bus), 5.148479971117694);
            graph.AddEdge(new EdgeWithId<string>("2548885", "2538475", bus), 1.8323778451236528);
            graph.AddEdge(new EdgeWithId<string>("2538475", "2548885", bus), 1.8323778451236528);
            graph.AddEdge(new EdgeWithId<string>("2548885", "2553604", bus), 2.6487181885583824);
            graph.AddEdge(new EdgeWithId<string>("2553604", "2548885", bus), 2.6487181885583824);
            graph.AddEdge(new EdgeWithId<string>("2548885", "2507480", bus), 8.526241087489844);
            graph.AddEdge(new EdgeWithId<string>("2507480", "2548885", bus), 8.526241087489844);
            graph.AddEdge(new EdgeWithId<string>("250441", "170654", bus), 1.593063671954134);
            graph.AddEdge(new EdgeWithId<string>("170654", "250441", bus), 1.593063671954134);
            graph.AddEdge(new EdgeWithId<string>("250441", "276781", bus), 1.98552824469963);
            graph.AddEdge(new EdgeWithId<string>("276781", "250441", bus), 1.98552824469963);
            graph.AddEdge(new EdgeWithId<string>("250441", "268743", bus), 1.9988336945328875);
            graph.AddEdge(new EdgeWithId<string>("268743", "250441", bus), 1.9988336945328875);
            graph.AddEdge(new EdgeWithId<string>("2232593", "2324774", bus), 2.7879194566737406);
            graph.AddEdge(new EdgeWithId<string>("2324774", "2232593", bus), 2.7879194566737406);
            graph.AddEdge(new EdgeWithId<string>("2232593", "2332459", bus), 6.752707012161864);
            graph.AddEdge(new EdgeWithId<string>("2332459", "2232593", bus), 6.752707012161864);
            graph.AddEdge(new EdgeWithId<string>("276781", "170654", bus), 0.882286714849546);
            graph.AddEdge(new EdgeWithId<string>("170654", "276781", bus), 0.882286714849546);
            graph.AddEdge(new EdgeWithId<string>("276781", "314830", bus), 3.6901106130304595);
            graph.AddEdge(new EdgeWithId<string>("314830", "276781", bus), 3.6901106130304595);
            graph.AddEdge(new EdgeWithId<string>("2357048", "2298890", bus), 5.677902082186694);
            graph.AddEdge(new EdgeWithId<string>("2298890", "2357048", bus), 5.677902082186694);
            graph.AddEdge(new EdgeWithId<string>("2357048", "2460596", bus), 6.4723687387230955);
            graph.AddEdge(new EdgeWithId<string>("2460596", "2357048", bus), 6.4723687387230955);
            graph.AddEdge(new EdgeWithId<string>("587084", "128747", bus), 4.676120626299117);
            graph.AddEdge(new EdgeWithId<string>("128747", "587084", bus), 4.676120626299117);
            graph.AddEdge(new EdgeWithId<string>("2655603", "2867714", bus), 14.158195443717394);
            graph.AddEdge(new EdgeWithId<string>("2867714", "2655603", bus), 14.158195443717394);
            graph.AddEdge(new EdgeWithId<string>("98182", "99071", bus), 3.2604787602129806);
            graph.AddEdge(new EdgeWithId<string>("99071", "98182", bus), 3.2604787602129806);
            graph.AddEdge(new EdgeWithId<string>("98182", "99532", bus), 4.40927650233006);
            graph.AddEdge(new EdgeWithId<string>("99532", "98182", bus), 4.40927650233006);
            graph.AddEdge(new EdgeWithId<string>("616052", "99071", bus), 4.097903796284142);
            graph.AddEdge(new EdgeWithId<string>("99071", "616052", bus), 4.097903796284142);
            graph.AddEdge(new EdgeWithId<string>("511196", "479561", bus), 3.2795794279754853);
            graph.AddEdge(new EdgeWithId<string>("479561", "511196", bus), 3.2795794279754853);
            graph.AddEdge(new EdgeWithId<string>("511196", "115019", bus), 28.642621712128594);
            graph.AddEdge(new EdgeWithId<string>("115019", "511196", bus), 28.642621712128594);
            graph.AddEdge(new EdgeWithId<string>("2507480", "2538475", bus), 10.285891278839186);
            graph.AddEdge(new EdgeWithId<string>("2538475", "2507480", bus), 10.285891278839186);
            graph.AddEdge(new EdgeWithId<string>("792680", "2761369", bus), 5.323865360140129);
            graph.AddEdge(new EdgeWithId<string>("2761369", "792680", bus), 5.323865360140129);
            graph.AddEdge(new EdgeWithId<string>("170654", "360630", bus), 6.107664880868957);
            graph.AddEdge(new EdgeWithId<string>("360630", "170654", bus), 6.107664880868957);
            graph.AddEdge(new EdgeWithId<string>("2553604", "2538475", bus), 0.8872148401035672);
            graph.AddEdge(new EdgeWithId<string>("2538475", "2553604", bus), 0.8872148401035672);
            graph.AddEdge(new EdgeWithId<string>("2553604", "2460596", bus), 20.941916129153512);
            graph.AddEdge(new EdgeWithId<string>("2460596", "2553604", bus), 20.941916129153512);
            graph.AddEdge(new EdgeWithId<string>("2553604", "2422465", bus), 24.803529592735384);
            graph.AddEdge(new EdgeWithId<string>("2422465", "2553604", bus), 24.803529592735384);
            graph.AddEdge(new EdgeWithId<string>("232422", "365137", bus), 15.328605936421615);
            graph.AddEdge(new EdgeWithId<string>("365137", "232422", bus), 15.328605936421615);
            graph.AddEdge(new EdgeWithId<string>("232422", "109223", bus), 25.155194039190004);
            graph.AddEdge(new EdgeWithId<string>("109223", "232422", bus), 25.155194039190004);
            graph.AddEdge(new EdgeWithId<string>("232422", "2324774", bus), 25.95505330689382);
            graph.AddEdge(new EdgeWithId<string>("2324774", "232422", bus), 25.95505330689382);
            graph.AddEdge(new EdgeWithId<string>("99532", "292223", bus), 9.178747012898876);
            graph.AddEdge(new EdgeWithId<string>("292223", "99532", bus), 9.178747012898876);
            graph.AddEdge(new EdgeWithId<string>("2422465", "2332459", bus), 17.348260261363386);
            graph.AddEdge(new EdgeWithId<string>("2332459", "2422465", bus), 17.348260261363386);
            graph.AddEdge(new EdgeWithId<string>("2618425", "2950159", bus), 3.2628849910623567);
            graph.AddEdge(new EdgeWithId<string>("2950159", "2618425", bus), 3.2628849910623567);
            graph.AddEdge(new EdgeWithId<string>("2618425", "2673730", bus), 6.60409618997937);
            graph.AddEdge(new EdgeWithId<string>("2673730", "2618425", bus), 6.60409618997937);
            graph.AddEdge(new EdgeWithId<string>("2618425", "361058", bus), 30.011819894128376);
            graph.AddEdge(new EdgeWithId<string>("361058", "2618425", bus), 30.011819894128376);
            graph.AddEdge(new EdgeWithId<string>("2324774", "361058", bus), 35.00441223424984);
            graph.AddEdge(new EdgeWithId<string>("361058", "2324774", bus), 35.00441223424984);
            graph.AddEdge(new EdgeWithId<string>("360630", "292223", bus), 24.437936378037328);
            graph.AddEdge(new EdgeWithId<string>("292223", "360630", bus), 24.437936378037328);
            graph.AddEdge(new EdgeWithId<string>("360630", "2950159", bus), 28.68387490851262);
            graph.AddEdge(new EdgeWithId<string>("2950159", "360630", bus), 28.68387490851262);
            graph.AddEdge(new EdgeWithId<string>("292223", "2673730", bus), 50.50843990501786);
            graph.AddEdge(new EdgeWithId<string>("745044", "524901", plane), 34.19460003184128);
            graph.AddEdge(new EdgeWithId<string>("524901", "745044", plane), 34.19460003184128);
            graph.AddEdge(new EdgeWithId<string>("745044", "2332459", plane), 85.9636019802102);
            graph.AddEdge(new EdgeWithId<string>("2332459", "745044", plane), 85.9636019802102);
            graph.AddEdge(new EdgeWithId<string>("745044", "360630", plane), 22.380263310711968);
            graph.AddEdge(new EdgeWithId<string>("360630", "745044", plane), 22.380263310711968);
            graph.AddEdge(new EdgeWithId<string>("745044", "2643743", plane), 61.82288907374355);
            graph.AddEdge(new EdgeWithId<string>("2643743", "745044", plane), 61.82288907374355);
            graph.AddEdge(new EdgeWithId<string>("524901", "2332459", plane), 120.02294623636932);
            graph.AddEdge(new EdgeWithId<string>("2332459", "524901", plane), 120.02294623636932);
            graph.AddEdge(new EdgeWithId<string>("524901", "360630", plane), 52.93314991043704);
            graph.AddEdge(new EdgeWithId<string>("360630", "524901", plane), 52.93314991043704);
            graph.AddEdge(new EdgeWithId<string>("524901", "2643743", plane), 75.95826829269083);
            graph.AddEdge(new EdgeWithId<string>("2643743", "524901", plane), 75.95826829269083);
            graph.AddEdge(new EdgeWithId<string>("2332459", "360630", plane), 73.0278064924204);
            graph.AddEdge(new EdgeWithId<string>("360630", "2332459", plane), 73.0278064924204);
            graph.AddEdge(new EdgeWithId<string>("2332459", "2643743", plane), 90.38357488968225);
            graph.AddEdge(new EdgeWithId<string>("2643743", "2332459", plane), 90.38357488968225);
            graph.AddEdge(new EdgeWithId<string>("360630", "2643743", plane), 76.00902523985161);
            graph.AddEdge(new EdgeWithId<string>("2643743", "360630", plane), 76.00902523985161);
            graph.AddEdge(new EdgeWithId<string>("98182", "112931", plane), 14.809411482297323);
            graph.AddEdge(new EdgeWithId<string>("112931", "98182", plane), 14.809411482297323);
            graph.AddEdge(new EdgeWithId<string>("98182", "498817", plane), 60.196105833018805);
            graph.AddEdge(new EdgeWithId<string>("498817", "98182", plane), 60.196105833018805);
            graph.AddEdge(new EdgeWithId<string>("98182", "108410", plane), 17.91744567324261);
            graph.AddEdge(new EdgeWithId<string>("108410", "98182", plane), 17.91744567324261);
            graph.AddEdge(new EdgeWithId<string>("98182", "361058", plane), 29.2021244621483);
            graph.AddEdge(new EdgeWithId<string>("361058", "98182", plane), 29.2021244621483);
            graph.AddEdge(new EdgeWithId<string>("112931", "498817", plane), 64.2901131945496);
            graph.AddEdge(new EdgeWithId<string>("498817", "112931", plane), 64.2901131945496);
            graph.AddEdge(new EdgeWithId<string>("112931", "108410", plane), 23.936028782669858);
            graph.AddEdge(new EdgeWithId<string>("108410", "112931", plane), 23.936028782669858);
            graph.AddEdge(new EdgeWithId<string>("112931", "361058", plane), 43.85697943543764);
            graph.AddEdge(new EdgeWithId<string>("361058", "112931", plane), 43.85697943543764);
            graph.AddEdge(new EdgeWithId<string>("498817", "108410", plane), 77.76475362034913);
            graph.AddEdge(new EdgeWithId<string>("108410", "498817", plane), 77.76475362034913);
            graph.AddEdge(new EdgeWithId<string>("498817", "361058", plane), 57.450463358956476);
            graph.AddEdge(new EdgeWithId<string>("361058", "498817", plane), 57.450463358956476);
            graph.AddEdge(new EdgeWithId<string>("108410", "361058", plane), 35.9850976858199);
            graph.AddEdge(new EdgeWithId<string>("361058", "108410", plane), 35.9850976858199);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2335204", plane), 28.407577735245223);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2293538", plane), 28.407577735245223);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2339354", plane), 16.368257238313433);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2293538", plane), 16.368257238313433);
            graph.AddEdge(new EdgeWithId<string>("2293538", "323786", plane), 101.13430998170699);
            graph.AddEdge(new EdgeWithId<string>("323786", "2293538", plane), 101.13430998170699);
            graph.AddEdge(new EdgeWithId<string>("2293538", "2950159", plane), 100.65379059250972);
            graph.AddEdge(new EdgeWithId<string>("2950159", "2293538", plane), 100.65379059250972);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2339354", plane), 13.057967653459706);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2335204", plane), 13.057967653459706);
            graph.AddEdge(new EdgeWithId<string>("2335204", "323786", plane), 74.07641406183212);
            graph.AddEdge(new EdgeWithId<string>("323786", "2335204", plane), 74.07641406183212);
            graph.AddEdge(new EdgeWithId<string>("2335204", "2950159", plane), 81.63734964778314);
            graph.AddEdge(new EdgeWithId<string>("2950159", "2335204", plane), 81.63734964778314);
            graph.AddEdge(new EdgeWithId<string>("2339354", "323786", plane), 87.1093447759929);
            graph.AddEdge(new EdgeWithId<string>("323786", "2339354", plane), 87.1093447759929);
            graph.AddEdge(new EdgeWithId<string>("2339354", "2950159", plane), 92.27290511348387);
            graph.AddEdge(new EdgeWithId<string>("2950159", "2339354", plane), 92.27290511348387);
            graph.AddEdge(new EdgeWithId<string>("323786", "2950159", plane), 46.34360561879491);
            graph.AddEdge(new EdgeWithId<string>("2950159", "323786", plane), 46.34360561879491);
            graph.AddEdge(new EdgeWithId<string>("3117735", "2553604", plane), 15.735698582331828);
            graph.AddEdge(new EdgeWithId<string>("2553604", "3117735", plane), 15.735698582331828);
            graph.AddEdge(new EdgeWithId<string>("3117735", "105343", plane), 93.73767396421354);
            graph.AddEdge(new EdgeWithId<string>("105343", "3117735", plane), 93.73767396421354);
            graph.AddEdge(new EdgeWithId<string>("3117735", "703448", plane), 71.33606030711816);
            graph.AddEdge(new EdgeWithId<string>("703448", "3117735", plane), 71.33606030711816);
            graph.AddEdge(new EdgeWithId<string>("3117735", "344979", plane), 105.59136254530291);
            graph.AddEdge(new EdgeWithId<string>("344979", "3117735", plane), 105.59136254530291);
            graph.AddEdge(new EdgeWithId<string>("2553604", "105343", plane), 96.66891283111441);
            graph.AddEdge(new EdgeWithId<string>("105343", "2553604", plane), 96.66891283111441);
            graph.AddEdge(new EdgeWithId<string>("2553604", "703448", plane), 83.3970195140066);
            graph.AddEdge(new EdgeWithId<string>("703448", "2553604", plane), 83.3970195140066);
            graph.AddEdge(new EdgeWithId<string>("2553604", "344979", plane), 104.92753441015374);
            graph.AddEdge(new EdgeWithId<string>("344979", "2553604", plane), 104.92753441015374);
            graph.AddEdge(new EdgeWithId<string>("105343", "703448", plane), 60.37089232361237);
            graph.AddEdge(new EdgeWithId<string>("703448", "105343", plane), 60.37089232361237);
            graph.AddEdge(new EdgeWithId<string>("105343", "344979", plane), 25.05106993918623);
            graph.AddEdge(new EdgeWithId<string>("344979", "105343", plane), 25.05106993918623);
            graph.AddEdge(new EdgeWithId<string>("703448", "344979", plane), 84.4757580053402);
            graph.AddEdge(new EdgeWithId<string>("344979", "703448", plane), 84.4757580053402);
            return graph;

        }
    }
}
