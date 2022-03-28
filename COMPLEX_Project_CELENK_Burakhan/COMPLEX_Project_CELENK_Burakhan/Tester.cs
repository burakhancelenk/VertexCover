using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COMPLEX_Project_CELENK_Burakhan
{
    /// <summary>
    /// Tester class for all functionalities and comparaisons.
    /// </summary>
    class Tester
    {
        //********** TEST GRAPH FUNCTIONALITIES **************//

        // Print graph infos to the console
        public static void ShowGraphInfos(Graph G)
        {
            Console.WriteLine("*** GRAPH INFOS ***");

            Console.WriteLine("Number of vertices : " + G.m_vertices.Length);

            Tester.ShowAllGraphMaxDegree(G);
            Tester.ShowVertexDegrees(G);

            Console.WriteLine("Number of edges : " + G.m_edges.Length);
            foreach (Edge e in G.m_edges)
            {
                Console.WriteLine("Edge : " + e.startVertexValue + " " + e.endVertexValue);
            }
            
            Console.WriteLine("*** END GRAPH INFOS ***");
        }

        // Test GetMaxDegreeVertex() Function
        public static void ShowGraphMaxDegree(Graph G)
        {
            Console.WriteLine("*** GRAPH MAX DEGREE VERTEX ***");
            Vertex maxDegree = G.GetMaxDegreeVertex();
            Console.WriteLine("Vertex : " + maxDegree.value + " Degree : " + maxDegree.degree);
            Console.WriteLine("*** END GRAPH MAX DEGREE VERTEX ***");
        }

        // Test GetAllMaxDegreeVertices() Function
        public static void ShowAllGraphMaxDegree(Graph G)
        {
            Console.WriteLine("*** GRAPH MAX DEGREE VERTICES ***");
            Vertex[] maxDegrees = G.GetAllMaxDegreeVertices();

            foreach (Vertex v in maxDegrees)
            {
                Console.WriteLine("Vertex : " + v.value + " Degree : " + v.degree);
            }
            Console.WriteLine("*** END GRAPH MAX DEGREE VERTEX ***");
        }

        // Test GetVertexDegrees() Function
        public static void ShowVertexDegrees(Graph G)
        {
            if (!G.AreDegreesCalculated())
            {
                G.GetVertexDegrees();
            }

            Console.WriteLine("*** GRAPH VERTEX DEGREES ***");
            foreach (Vertex v in G.m_vertices)
            {
                Console.WriteLine("Vertex : " + v.value + " Degree : " + v.degree);
            }
            Console.WriteLine("*** END GRAPH VERTEX DEGREES ***");
        }

        public static void TestGraphConstructor()
        {
            Console.WriteLine("*** TEST GRAPH CONSTRUCTOR ***");
            int[] vertices = {1,3,5,6,8};
            List<Edge> edges = new List<Edge>();
            edges.Add(new Edge {startVertexValue = 1, endVertexValue = 3});
            edges.Add(new Edge { startVertexValue = 1, endVertexValue = 6 });
            edges.Add(new Edge { startVertexValue = 1, endVertexValue = 8 });
            edges.Add(new Edge { startVertexValue = 3, endVertexValue = 6 });
            edges.Add(new Edge { startVertexValue = 3, endVertexValue = 8 });
            edges.Add(new Edge { startVertexValue = 5, endVertexValue = 6 });
            edges.Add(new Edge { startVertexValue = 6, endVertexValue = 8 });

            Graph G = new Graph(vertices, edges.ToArray());
            Tester.ShowGraphInfos(G);
            Console.WriteLine("*** END TEST GRAPH CONSTRUCTOR ***");
        }

        public static void TestReadFromTextFile(string filepath)
        {
            Console.WriteLine("*** TEST READ FROM TEXT FILE ***");
            Graph G = new Graph();
            G.ReadFromTextFile(filepath);
            Tester.ShowGraphInfos(G);
            Console.WriteLine("*** END TEST READ FROM TEXT FILE ***");
        }

        public static void TestGenerateRandomGraph(int n, float p)
        {
            Console.WriteLine("*** TEST GENERATE RANDOM GRAPH ***");
            Graph G = new Graph();
            G.GenerateRandomGraph(n,p);
            Tester.ShowGraphInfos(G);
            Console.WriteLine("*** END TEST GENERATE RANDOM GRAPH ***");
        }

        public static void TestDeleteVertexSingleElement()
        {
            Console.WriteLine("*** TEST DELETE VERTEX (SINGLE ELEMENT) ***");

            Console.WriteLine("Creating random graph with n = 10, p = 0.45...");
            Graph G = new Graph();
            G.GenerateRandomGraph(10, 0.45f);
            Tester.ShowGraphInfos(G);

            Console.WriteLine("Deleting vertex 6 from the graph...");
            Graph newGraph = G.DeleteVertex(6);

            Console.WriteLine("Printing new graph infos...");
            ShowGraphInfos(newGraph);

            Console.WriteLine("*** END TEST DELETE VERTEX (SINGLE ELEMENT) ***");
        }

        public static void TestDeleteVertexMultipleElements()
        {
            Console.WriteLine("*** TEST DELETE VERTEX (MULTIPLE ELEMENTS) ***");

            Console.WriteLine("Creating random graph with n = 12, p = 0.4...");
            Graph G = new Graph();
            G.GenerateRandomGraph(12, 0.4f);
            Tester.ShowGraphInfos(G);

            Console.WriteLine("Deleting vertex 3,6,7 from the graph...");
            int[] deletedVertices = { 3, 6, 7 };
            Graph newGraph = G.DeleteVertex(deletedVertices);

            Console.WriteLine("Printing new graph infos...");
            ShowGraphInfos(newGraph);

            Console.WriteLine("*** END TEST DELETE VERTEX (MULTIPLE ELEMENTS) ***");
        }

        //********** TEST ALGORITHMS **************//

        // These methods have same body with the original methods. Only some print functions added for tracking
        // what does the algorithm in each step
        public static void TestAlgorithmeCouplage(Graph G)
        {
            Console.WriteLine("*** TEST ALGORITHME COUPLAGE ***");
            List<int> vertexCover = new List<int>();
 
            // Traverse all the edges in the graph
            foreach (Edge e in G.m_edges)
            {
                Console.Write("Vertex Cover : { ");
                foreach(int i in vertexCover)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine("}");
                Console.WriteLine("Current edge : { " + e.startVertexValue + " " + e.endVertexValue + " }");

                // Check if any of these endpoints is in the set
                if (vertexCover.Contains(e.startVertexValue) || vertexCover.Contains(e.endVertexValue))
                {
                    Console.WriteLine("Vertex cover already contains : " + e.startVertexValue + " or " + e.endVertexValue);
                    Console.WriteLine("No vertex added to the vertex cover.");
                    continue;
                }

                // if not, then add these endpoints to the set
                vertexCover.Add(e.startVertexValue);
                vertexCover.Add(e.endVertexValue);
                Console.WriteLine("Vertex cover doesn't contain : " + e.startVertexValue + " and " + e.endVertexValue);
                Console.WriteLine("Vertex : " + e.startVertexValue + " and " + "Vertex : " + e.endVertexValue + " added to the vertex cover.");
            }
            Console.WriteLine("Algorithm has finished");
            Console.Write("Final State Of The Vertex Cover : { ");
            foreach (int i in vertexCover)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("}");
            Console.WriteLine("*** END TEST ALGORITHME COUPLAGE ***");
        }

        public static void TestAlgorithmeGlouton(Graph G)
        {
            Console.WriteLine("*** TEST ALGORITHME GLOUTON ***");
            List<int> vertexCover = new List<int>();
            Graph tempGraph = G;

            // Keep removing max degree vertices until there is no edge in the graph
            // Save each vertex removed from the graph 
            while (tempGraph.m_edges.Length > 0)
            {
                Tester.ShowGraphInfos(tempGraph);
                Console.Write("\n" + "Vertex Cover : { ");
                foreach (int i in vertexCover)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine("}");

                Vertex maxDegreeVertex = tempGraph.GetMaxDegreeVertex();
                vertexCover.Add(maxDegreeVertex.value);
                tempGraph = tempGraph.DeleteVertex(maxDegreeVertex.value);
                Console.WriteLine("Max degree vertex : " + maxDegreeVertex.value + " is deleted from the graph and added to the vertex cover." + "\n");
            }

            Console.WriteLine("Algorithm has finished.");
            Console.Write("Final Vertex Cover : { ");
            foreach (int i in vertexCover)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("}");
        }

        // For classic time observation (test each algorithm for n is [minN, maxN] and probability p)
        public static void TestCalculationTime_Classic(string algorithm, int minN, int maxN, float p)
        {
            var watch = new System.Diagnostics.Stopwatch();
            Graph G = new Graph();
            string[] measuredTimes = new string[maxN-minN+1];

            switch (algorithm)
            {
                case "Glouton":
                    for (int n = minN; n <= maxN; n++)
                    {
                        G.GenerateRandomGraph(n, p);
                        watch.Restart();
                        VertexCover.AlgorithmeGlouton(G);
                        watch.Stop();
                        measuredTimes[n - minN] = watch.ElapsedMilliseconds.ToString();
                    }
                    break;
                case "Couplage":
                    for (int n = minN; n <= maxN; n++)
                    {
                        G.GenerateRandomGraph(n, p);
                        watch.Restart();
                        VertexCover.AlgorithmeCouplage(G);
                        watch.Stop();
                        measuredTimes[n - minN] = watch.ElapsedMilliseconds.ToString();
                    }
                    break;
            }

            string[] nValues = new string[maxN - minN + 1];
            for (int i = minN; i <= maxN; i++)
            {
                nValues[i - minN] = i.ToString();
            }

            string fullPathTime = @"C:\tmp\TimeResult_" + algorithm + "_Classic.txt";
            string fullPathN = @"C:\tmp\TimeResult2_" + algorithm + "_Classic.txt";

            File.WriteAllLines(fullPathTime, measuredTimes);
            File.WriteAllLines(fullPathN, nValues);
        }


        // Time observation using more advanced technique
        // For BAB_DFS and BAB_BFS, no matter which value is assigned, p is automatically assigned to 1/Sqrt(N)
        public static void TestCalculationTime_Advanced(string algorithm, int maxN, float p)
        {
            var watch = new System.Diagnostics.Stopwatch();
            // We will create 20 instance for each n value (10 different value), in total 20*10 = 200 instances
            int instanceCount = 20;
            int NmaxCount = 10;
            Graph[] graphs = new Graph[instanceCount * NmaxCount];
            long[] measuredTimes = new long[instanceCount * NmaxCount];
            int[] NmaxValues = new int[NmaxCount];

            // Instance generation
            for (int i = 0; i < NmaxCount; i++)
            {
                int currentN = (int)((i+1) * maxN / (float)NmaxCount);
                NmaxValues[i] = currentN;

                if (algorithm == "BAB_DFS" || algorithm == "BAB_BFS")
                    p = (float)(1 / Math.Sqrt(currentN));

                for (int j = 0; j < instanceCount; j++)
                {
                    graphs[i * instanceCount + j] = new Graph();
                    graphs[i * instanceCount + j].GenerateRandomGraph(currentN, p);
                }
            }

            
            switch (algorithm)
            {
                case "Glouton":
                    for (int i=0 ; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the time result
                        watch.Restart();
                        VertexCover.AlgorithmeGlouton(graphs[i]);
                        watch.Stop();
                        measuredTimes[i] = watch.ElapsedMilliseconds;
                    }
                    break;
                case "Couplage":
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the time result
                        watch.Restart();
                        VertexCover.AlgorithmeCouplage(graphs[i]);
                        watch.Stop();
                        measuredTimes[i] = watch.ElapsedMilliseconds;
                    }
                    break;
                case "Custom":
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the time result
                        watch.Restart();
                        VertexCover.CustomAprxAlgorithm(graphs[i]);
                        watch.Stop();
                        measuredTimes[i] = watch.ElapsedMilliseconds;
                    }
                    break;
                case "BAB_DFS":
                    // Change the settings to get desired BAB_DFS time results
                    DFS_Settings settings = new DFS_Settings
                    {
                        withBound = true,
                        heuristicAlgorithm = "Couplage",
                        advancedBranching = true,
                        ordered_AB = true,
                        eliminateDegreeOne = true
                    };
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the time result
                        watch.Restart();
                        VertexCover.BranchAndBound_DFS(graphs[i],settings);
                        watch.Stop();
                        measuredTimes[i] = watch.ElapsedMilliseconds;
                    }
                    break;
                case "BAB_BFS":
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the time result
                        watch.Restart();
                        VertexCover.BranchAndBound_BFS(graphs[i],false);
                        watch.Stop();
                        measuredTimes[i] = watch.ElapsedMilliseconds;
                    }
                    break;
            }

            // Calculate Tn(avarage time for each Nmax value) values
            long[] averageTimeValues = new long[NmaxCount];
            for(int i = 0; i < NmaxCount; i++)
            {
                long tempTime = 0;
                for (int j = 0; j < instanceCount; j++)
                {
                    tempTime += measuredTimes[i * instanceCount + j];
                }
                averageTimeValues[i] = (long)(tempTime / (float)instanceCount);
            }

            // Convert time values to string
            string[] averageTimeValues_S = new string[NmaxCount];
            string[] NmaxValues_S = new string[NmaxCount];

            for(int i = 0; i < NmaxCount; i++)
            {
                averageTimeValues_S[i] = averageTimeValues[i].ToString();
                NmaxValues_S[i] = NmaxValues[i].ToString();
            }

            // Write results to text file
            string fullPathTime = @"C:\tmp\TimeResult_Time_" + algorithm + "_Advanced.txt";
            string fullPathN = @"C:\tmp\TimeResult_N_" + algorithm + "_Advanced.txt";

            File.WriteAllLines(fullPathTime, averageTimeValues_S);
            File.WriteAllLines(fullPathN, NmaxValues_S);
        }

        // Compares the algorithm with exact solutions and gives quality percentages of the results.
        // Same advanced technique in the time calculation
        public static void TestSolutionQuality(string algorithm, int maxN, float p)
        {
            // We will create 20 instance for each n value (10 different value), in total 20*10 = 200 instances
            int instanceCount = 20;
            int NmaxCount = 10;
            Graph[] graphs = new Graph[instanceCount * NmaxCount];
            int[] NmaxValues = new int[NmaxCount];
            float[] qualityPercentages = new float[NmaxCount];
            double[] standardDeviations = new double[NmaxCount];

            // Instance generation
            for (int i = 0; i < NmaxCount; i++)
            {
                int currentN = (int)((i + 1) * maxN / (float)NmaxCount);
                NmaxValues[i] = currentN;

                for (int j = 0; j < instanceCount; j++)
                {
                    graphs[i * instanceCount + j] = new Graph();
                    graphs[i * instanceCount + j].GenerateRandomGraph(currentN, p);
                }
            }

            for (int i = 0; i < NmaxCount; i++)
            {
                int numCorrectResults = 0;
                double standardDev = 0;
                for (int j = 0; j < instanceCount; j++)
                {
                    // Execute the algorithms
                    List<int> aprxResult = new List<int>();
                    switch (algorithm)
                    {
                        case "Glouton":
                            aprxResult = VertexCover.AlgorithmeGlouton(graphs[i * NmaxCount + j]);
                            break;
                        case "Couplage":
                            aprxResult = VertexCover.AlgorithmeCouplage(graphs[i * NmaxCount + j]);
                            break;
                        case "Custom":
                            aprxResult = VertexCover.CustomAprxAlgorithm(graphs[i * NmaxCount + j]);
                            break;
                    }

                    DFS_Settings settings = new DFS_Settings
                    {
                        withBound = true,
                        heuristicAlgorithm = "Couplage",
                        advancedBranching = true,
                        ordered_AB = true,
                        eliminateDegreeOne = true
                    };
                    List<int> exactSolution = VertexCover.BranchAndBound_DFS(graphs[i * NmaxCount + j],settings);

                    // Test the correctness
                    if (aprxResult.Count == exactSolution.Count) numCorrectResults += 1;
                    // Standard deviation
                    standardDev += Math.Pow(aprxResult.Count - exactSolution.Count, 2);
                }

                // Calculate the quality percentage and save it
                qualityPercentages[i] = 100 * (numCorrectResults / (float)instanceCount);
                standardDeviations[i] = Math.Sqrt(standardDev / (instanceCount - 1));
            }

            // Convert percentage values to string
            string[] qualityPercentages_S = new string[NmaxCount];
            string[] standardDeviations_S = new string[NmaxCount];
            string[] NmaxValues_S = new string[NmaxCount];

            for (int i = 0; i < NmaxCount; i++)
            {
                qualityPercentages_S[i] = qualityPercentages[i].ToString();
                standardDeviations_S[i] = standardDeviations[i].ToString();
                NmaxValues_S[i] = NmaxValues[i].ToString();
            }

            // Write results to text file
            string fullPathPer = @"C:\tmp\QualityResult_Per_" + algorithm + ".txt";
            string fullPathDev = @"C:\tmp\QualityResult_Dev_" + algorithm + ".txt";
            string fullPathN = @"C:\tmp\QualityResult_N_" + algorithm + ".txt";

            File.WriteAllLines(fullPathPer, qualityPercentages_S);
            File.WriteAllLines(fullPathDev, standardDeviations_S);
            File.WriteAllLines(fullPathN, NmaxValues_S);
        }


        // Tester function for calculate generated nodes with same advanced technique in the time calculation.
        public static void TestGeneratedNodes(string algorithm, int maxN, float p)
        {
            // We will create 20 instance for each n value (10 different value), in total 20*10 = 200 instances
            int instanceCount = 20;
            int NmaxCount = 10;
            Graph[] graphs = new Graph[instanceCount * NmaxCount];
            long[] generatedNodes = new long[instanceCount * NmaxCount];
            int[] NmaxValues = new int[NmaxCount];

            // Instance generation
            for (int i = 0; i < NmaxCount; i++)
            {
                int currentN = (int)((i + 1) * maxN / (float)NmaxCount);
                NmaxValues[i] = currentN;

                if (algorithm == "BAB_DFS" || algorithm == "BAB_BFS")
                    p = (float)(1 / Math.Sqrt(currentN));

                for (int j = 0; j < instanceCount; j++)
                {
                    graphs[i * instanceCount + j] = new Graph();
                    graphs[i * instanceCount + j].GenerateRandomGraph(currentN, p);
                }
            }


            switch (algorithm)
            {
                case "BAB_DFS":
                    // Change the settings to get desired BAB_DFS time results
                    DFS_Settings settings = new DFS_Settings
                    {
                        withBound = true,
                        heuristicAlgorithm = "Couplage",
                        advancedBranching = true,
                        ordered_AB = true,
                        eliminateDegreeOne = true,
                    };
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the result
                        VertexCover.BranchAndBound_DFS(graphs[i], settings);
                        generatedNodes[i] = VertexCover.generatedNodeCount;
                    }
                    break;
                case "BAB_BFS":
                    for (int i = 0; i < instanceCount * NmaxCount; i++)
                    {
                        // Test each instance and save the result
                        VertexCover.BranchAndBound_BFS(graphs[i], false);
                        generatedNodes[i] = VertexCover.generatedNodeCount;
                    }
                    break;
            }

            // Calculate Tn(avarage time for each Nmax value) values
            long[] averageNodeCounts = new long[NmaxCount];
            for (int i = 0; i < NmaxCount; i++)
            {
                long tempCount = 0;
                for (int j = 0; j < instanceCount; j++)
                {
                    tempCount += generatedNodes[i * instanceCount + j];
                }
                averageNodeCounts[i] = (long)(tempCount / (float)instanceCount);
            }

            // Convert time values to string
            string[] averageNodeCounts_S = new string[NmaxCount];
            string[] NmaxValues_S = new string[NmaxCount];

            for (int i = 0; i < NmaxCount; i++)
            {
                averageNodeCounts_S[i] = averageNodeCounts[i].ToString();
                NmaxValues_S[i] = NmaxValues[i].ToString();
            }

            // Write results to text file
            string fullPathTime = @"C:\tmp\GeneratedNodeResult_NodeCount_" + algorithm + "_Advanced.txt";
            string fullPathN = @"C:\tmp\GeneratedNodeResult_N_" + algorithm + "_Advanced.txt";

            File.WriteAllLines(fullPathTime, averageNodeCounts_S);
            File.WriteAllLines(fullPathN, NmaxValues_S);
        }


        // Tester function for get experimental worst results
        public static void TestApproximationAlgorithmsResults()
        {
            Console.WriteLine("****** APPROXIMATION ALGORITHMS RESULTS ******");
            Console.WriteLine("Number of vertices(n) => 10 to 60, Instance count for each n => 20");

            for(int a = 0; a < 3; a++) // 0 => couplage, 1 => glouton, 2 => custom
            {
                switch (a)
                {
                    case 0:
                        Console.WriteLine("ALGORITHME COUPLAGE RESULTS");
                        break;
                    case 1:
                        Console.WriteLine("ALGORITHME GLOUTON RESULTS");
                        break;
                    case 2:
                        Console.WriteLine("CUSTOM ALGORITHM RESULTS");
                        break;
                }

                int difference = 0;
                List<int> worstResult = new List<int>();
                List<int> exactResult = new List<int>();

                for (int n = 10; n <= 40; n++)
                {
                    Console.WriteLine("For n : " + n);
                    for (int i = 0; i < 20; i++)
                    {
                        Graph G = new Graph();
                        G.GenerateRandomGraph(n, (float)(1 / Math.Sqrt(n)));
                        List<int> aprx = new List<int>();
                        switch (a) 
                        {
                            case 0:
                                aprx = VertexCover.AlgorithmeCouplage(G);
                                break;
                            case 1:
                                aprx = VertexCover.AlgorithmeGlouton(G);
                                break;
                            case 2:
                                aprx = VertexCover.CustomAprxAlgorithm(G);
                                break;
                        }

                        DFS_Settings settings = new DFS_Settings
                        {
                            withBound = true,
                            heuristicAlgorithm = "Couplage",
                            advancedBranching = true,
                            ordered_AB = true,
                            eliminateDegreeOne = true
                        };
                        List<int> exact = VertexCover.BranchAndBound_DFS(G, settings);

                        if (difference < aprx.Count - exact.Count)
                        {
                            difference = aprx.Count - exact.Count;
                            worstResult = aprx;
                            exactResult = exact;
                        }
                    }

                    Console.Write("Worst approximated result : { ");
                    foreach (int v in worstResult)
                    {
                        Console.Write(v + " ");
                    }
                    Console.WriteLine(" }");

                    Console.Write("Exact result : { ");
                    foreach (int v in exactResult)
                    {
                        Console.Write(v + " ");
                    }
                    Console.WriteLine(" }");
                }
                Console.WriteLine("END ALGORITHM");
            }
            Console.WriteLine("****** END APPROXIMATION ALGORITHMS RESULTS ******");
        }

        // Tester function for check whether the branch and bound algorithms works correctly or not and get the generated nodes count.
        public static void TestCorrectnessBranchAndBound(Graph G)
        {
            Console.WriteLine("****** TEST CORRECTNESS BRANCH AND BOUND ******\n\n");
            DFS_Settings settings = new DFS_Settings
            {
                withBound = false,
                heuristicAlgorithm = "Couplage",
                advancedBranching = false,
                ordered_AB = false,
                eliminateDegreeOne = false,
                debugActive = true
            };

            // DFS simple
            Console.WriteLine("TEST DFS SIMPLE\n");
            VertexCover.BranchAndBound_DFS(G, settings);
            Console.Write("\n");
            Console.WriteLine("END TEST DFS SIMPLE");

            Console.WriteLine("\n\n");

            // DFS with minimum bound
            Console.WriteLine("TEST DFS MIN BOUND\n");
            settings.withBound = true;
            VertexCover.BranchAndBound_DFS(G, settings);
            Console.Write("\n");
            Console.WriteLine("END TEST DFS MIN BOUND");

            Console.WriteLine("\n\n");

            // DFS with minimum bound and advanced branching
            Console.WriteLine("TEST DFS (MIN BOUND, ADVANCED BRANCHING)\n");
            settings.advancedBranching = true;
            VertexCover.BranchAndBound_DFS(G, settings);
            Console.Write("\n");
            Console.WriteLine("END TEST DFS (MIN BOUND, ADVANCED BRANCHING)");

            Console.WriteLine("\n\n");

            // DFS with minimum bound and advanced branching(orderedMaxDegree)
            Console.WriteLine("TEST DFS (MIN BOUND, ADVANCED BRANCHING(Ordered Max Degree))\n");
            settings.ordered_AB = true;
            VertexCover.BranchAndBound_DFS(G, settings);
            Console.Write("\n");
            Console.WriteLine("END TEST DFS (MIN BOUND, ADVANCED BRANCHING(Ordered Max Degree))");

            Console.WriteLine("\n\n");

            // DFS with minimum bound, advanced branching(orderedMaxDegree) and elimination
            Console.WriteLine("TEST DFS (MIN BOUND, ADVANCED BRANCHING(Ordered Max Degree), ELIM DEG 1)\n");
            settings.eliminateDegreeOne = true;
            VertexCover.BranchAndBound_DFS(G, settings);
            Console.Write("\n");
            Console.WriteLine("END TEST DFS (MIN BOUND, ADVANCED BRANCHING(Ordered Max Degree), ELIM DEG 1)");

            Console.WriteLine("\n\n");

            // BFS
            Console.WriteLine("TEST BFS\n");
            VertexCover.BranchAndBound_BFS(G, true);
            Console.Write("\n");
            Console.WriteLine("END TEST BFS");
            Console.WriteLine("\n");
            Console.WriteLine("****** END TEST CORRECTNESS BRANCH AND BOUND ******");
        }
    }
}
