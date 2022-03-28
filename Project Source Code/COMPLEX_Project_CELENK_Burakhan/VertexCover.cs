using System;
using System.Collections.Generic;
using System.Text;

namespace COMPLEX_Project_CELENK_Burakhan
{
    /// <summary>
    /// Contains vertex cover algorithms
    /// Included Algorithms;
    /// Algorithm Glouton
    /// Algorithm Couplage
    /// Algorithm Branch-And-Bound using Breadth-First-Search (returns all best solutions)
    /// Algorithm Branch-And-bound using Breadth-First-Search (returns one of the best solutions)
    /// Algorithm Branch-And-Bound using Depth-First-Search with DFS settings (see DFS_settings struct)
    /// </summary>

    struct Node
    {
        public List<int> vertexCover;
        public bool solutionFound;
    }

    struct DFS_Settings
    {
        public bool withBound; // true for use min bound check
        public string heuristicAlgorithm; // which algorithm for min bound check, no need to assign if withBound is false
        public bool advancedBranching; // true for use advanced branching
        public bool ordered_AB; // true for use max degree vertex while applying advanced branching, no need to assign if advancedBranching is false
        public bool eliminateDegreeOne; // true for use elimination of degree 1 vertices
        public bool debugActive; // true for debugging(examine the visited nodes and number of generated nodes)

        // Min bounds, will be automatically filled
        public int b1, b2;
        public double b3;
    }

    class VertexCover
    {
        public static int generatedNodeCount; // number of generated nodes by Branch-And-Bound algorithms 
        public static List<int> AlgorithmeCouplage(Graph G)
        {
            List<int> vertexCover = new List<int>();

            // Case only one vertex
            if (G.m_vertices.Length == 1) vertexCover.Add(G.m_vertices[0].value);

            // Traverse all the edges in the graph
            foreach (Edge e in G.m_edges)
            {
                // Check if any of these endpoints is in the set
                if(vertexCover.Contains(e.startVertexValue) || vertexCover.Contains(e.endVertexValue))
                {
                    continue;
                }

                // if not, then add these endpoints to the set
                vertexCover.Add(e.startVertexValue);
                vertexCover.Add(e.endVertexValue);
            }

            return vertexCover;
        }

        public static List<int> AlgorithmeGlouton(Graph G)
        {
            List<int> vertexCover = new List<int>();

            // Case only one vertex
            if (G.m_vertices.Length == 1) vertexCover.Add(G.m_vertices[0].value);

            Graph tempGraph = G;

            // Keep removing max degree vertices until there is no edge in the graph
            // Save each vertex removed from the graph 
            while (tempGraph.m_edges.Length > 0)
            {
                Vertex maxDegreeVertex = tempGraph.GetMaxDegreeVertex();
                vertexCover.Add(maxDegreeVertex.value);
                tempGraph = tempGraph.DeleteVertex(maxDegreeVertex.value);
            }

            return vertexCover;
        }

        // Custom approximation algorithm for vertex cover problem
        // 1 - Find max degree vertex
        // 2 - Find another max degree vertex which isn't connected to max degree vertex
        // 3 - Add them to the vertex cover and delete them from the graph
        // 4 - if there is no edge left then return vertex cover otherwise return back to step 1.
        public static List<int> CustomAprxAlgorithm(Graph G)
        {
            List<int> vertexCover = new List<int>();

            // Only one vertex case
            if (G.m_vertices.Length == 1)
            {
                vertexCover.Add(G.m_vertices[0].value);
                return vertexCover;
            }
            // End only one vertex case

            Graph eliminatedGraph = G;

            bool solutionFound = false; // flag

            while (!solutionFound)
            {
                // First step
                int maxDegreeVertex = eliminatedGraph.GetMaxDegreeVertex().value;

                // Second step
                int unconnectedVertex = -1;
                foreach(Edge e in eliminatedGraph.m_edges)
                {
                    if(e.startVertexValue != maxDegreeVertex && e.endVertexValue != maxDegreeVertex)
                    {
                        int ev1Degree = -1;
                        int ev2Degree = -1;
                        foreach(Vertex v in eliminatedGraph.m_vertices)
                        {
                            if (v.value == e.startVertexValue)
                            {
                                ev1Degree = v.degree;
                            }
                            else if(v.value == e.endVertexValue)
                            {
                                ev2Degree = v.degree;
                            }

                            if (ev1Degree > 0 && ev2Degree > 0) break;
                        }
                        
                        if(ev1Degree >= ev2Degree)
                        {
                            unconnectedVertex = e.startVertexValue;
                        }
                        else
                        {
                            unconnectedVertex = e.endVertexValue;
                        }

                        break;
                    }
                }
                // Third step
                List<int> deletedVertices = new List<int>();
                deletedVertices.Add(maxDegreeVertex);
                vertexCover.Add(maxDegreeVertex);
                if (unconnectedVertex != -1) 
                {
                    deletedVertices.Add(unconnectedVertex);
                    vertexCover.Add(unconnectedVertex);
                }
                eliminatedGraph = eliminatedGraph.DeleteVertex(deletedVertices.ToArray());

                // Last step
                if (eliminatedGraph.m_edges.Length == 0) solutionFound = true;
            }

            return vertexCover;
        }


        // Returns all best solutions using Breadth-First-Search, if you want to get all best solutions.
        public static List<List<int>> BranchAndBound_BFS_AllBestSolutions(Graph G)
        {
            // Only one vertex case
            if(G.m_vertices.Length == 1)
            {
                List<List<int>> vertexCovers = new List<List<int>>();
                List<int> vertexCover = new List<int>();
                vertexCover.Add(G.m_vertices[0].value);
                vertexCovers.Add(vertexCover);
                return vertexCovers;
            }
            // End only one vertex case

            // Eliminates degree 1 vertices
            List<int> degreeOneVs = new List<int>();
            G.GetVertexDegrees();
            if(G.GetMaxDegreeVertex().degree == 1)
            {
                List<List<int>> solution = new List<List<int>>();

                degreeOneVs.Add(G.m_vertices[0].value);
                solution.Add(degreeOneVs);
                degreeOneVs.Clear();
                degreeOneVs.Add(G.m_vertices[1].value);
                solution.Add(degreeOneVs);

                return solution;
            }
            foreach(Vertex v in G.m_vertices)
            {
                if (v.degree == 1) degreeOneVs.Add(v.value);
            }
            Graph eliminatedGraph = G.DeleteVertex(degreeOneVs.ToArray());
            // End elimination

            Random rnd = new Random();
            List<Node> exploredNodes = new List<Node>();
            exploredNodes.Add(new Node { vertexCover = new List<int>(), solutionFound = false });

            bool solutionFound = false;
            int currentLevel = -1;
            Node currentNode;
            
            while (!solutionFound)
            {
                // After traverse all the nodes in the same level, increase the level.
                currentLevel += 1;
                for (int i = 0; i < (int)Math.Pow(2, currentLevel); i++)
                {
                    currentNode = exploredNodes[i];

                    // Pick a random edge
                    Graph tempGraph = eliminatedGraph.DeleteVertex(currentNode.vertexCover.ToArray());
                    Edge randomEdge = tempGraph.m_edges[rnd.Next(0, tempGraph.m_edges.Length)];

                    // Delete vertices from the graph and save them separately
                    Graph leftGraph = tempGraph.DeleteVertex(randomEdge.startVertexValue);
                    Graph rightGraph = tempGraph.DeleteVertex(randomEdge.endVertexValue);

                    // Add vertices to the vertex covers
                    List<int> leftVertexCover = new List<int>();
                    List<int> rightVertexCover = new List<int>();
                    foreach(int v in currentNode.vertexCover)
                    {
                        leftVertexCover.Add(v);
                        rightVertexCover.Add(v);
                    }
                    leftVertexCover.Add(randomEdge.startVertexValue);
                    rightVertexCover.Add(randomEdge.endVertexValue);

                    // Is solution found ?
                    bool leftSolutionFound = leftGraph.m_edges.Length == 0;
                    bool rightSolutionFound = rightGraph.m_edges.Length == 0;

                    // if yes
                    if (leftSolutionFound || rightSolutionFound) solutionFound = true;

                    // Prepare the nodes and add them to the explored nodes list
                    Node leftChild = new Node { vertexCover = leftVertexCover, solutionFound = leftSolutionFound };
                    Node rightChild = new Node { vertexCover = rightVertexCover, solutionFound = rightSolutionFound };
                    exploredNodes.Add(leftChild);
                    exploredNodes.Add(rightChild);
                }
                exploredNodes.RemoveRange(0, (int)Math.Pow(2, currentLevel));
            }

            // In the same level, all solutions have same number of vertex. So all of them are best solutions.
            // We will traverse from leaf node to the root node for gather vertices in the vertex covers.
            // In simple version (which is this function), each node holds only one vertex in his vertexCover list

            List<List<int>> solutions = new List<List<int>>();

            // At last level, we have explored the child nodes but current level stayed same after last iteration
            // So this is why we increase current level, for start from the last explored level
            currentLevel += 1;

            // Traverse all the nodes in the current level
            for(int i = 0; i < (int)Math.Pow(2, currentLevel); i++)
            {

                if (exploredNodes[i].solutionFound)
                { 
                    // Add current solution to the solutions list if it isn't added already
                    exploredNodes[i].vertexCover.Sort();

                    foreach (List<int> s in solutions)
                    {
                        bool alreadyIn = true;
                        for (int n = 0; n < s.Count; n++)
                        {
                            if(s[n] != exploredNodes[i].vertexCover[n])
                            {
                                alreadyIn = false;
                            }
                        }

                        if (alreadyIn) goto skipAdd;
                    }

                    solutions.Add(exploredNodes[i].vertexCover);
                    skipAdd:
                    ;
                }  
            }

            // All best solutions are obtained, return the list
            return solutions;
        }


        // Finds the best solution using Breath-First-Search
        // Returns one of the best solutions (if there are more than one)
        public static List<int> BranchAndBound_BFS(Graph G, bool debugActive)
        {
            // Eliminates degree 1 vertices
            List<int> degreeOneVs = new List<int>();
            G.GetVertexDegrees();
            if(G.GetMaxDegreeVertex().degree == 1)
            {
                degreeOneVs.Add(G.m_vertices[0].value);
                return degreeOneVs;
            }
            foreach(Vertex v in G.m_vertices)
            {
                if (v.degree == 1) degreeOneVs.Add(v.value);
            }
            Graph eliminatedGraph = G.DeleteVertex(degreeOneVs.ToArray());
            // End elimination

            // Debug for elimination degree 1
            if (debugActive)
            {
                Console.Write("Eliminated vertices : { ");
                foreach (int v in degreeOneVs)
                {
                    Console.Write(v + " ");
                }
                Console.WriteLine("}");
            }

            // Reset generated node count
            generatedNodeCount = 0;

            Random rnd = new Random();
            LinkedList<Node> exploredNodes = new LinkedList<Node>();
            exploredNodes.AddLast(new Node { vertexCover = new List<int>(), solutionFound = false });

            Node currentNode = exploredNodes.First.Value;

            while (true)
            {
                // Pick a random edge
                Graph tempGraph = eliminatedGraph.DeleteVertex(currentNode.vertexCover.ToArray());
                Edge randomEdge = tempGraph.m_edges[rnd.Next(0, tempGraph.m_edges.Length)];

                // Delete vertices from the graph and save them separately
                Graph leftGraph = tempGraph.DeleteVertex(randomEdge.startVertexValue);
                Graph rightGraph = tempGraph.DeleteVertex(randomEdge.endVertexValue);

                // Add vertices to the vertex covers
                List<int> leftVertexCover = new List<int>();
                List<int> rightVertexCover = new List<int>();
                foreach (int v in currentNode.vertexCover)
                {
                    leftVertexCover.Add(v);
                    rightVertexCover.Add(v);
                }
                leftVertexCover.Add(randomEdge.startVertexValue);
                rightVertexCover.Add(randomEdge.endVertexValue);

                generatedNodeCount += 2;
                // Debug
                if (debugActive)
                {
                    Console.WriteLine("Picked edge : " + randomEdge.startVertexValue + " " + randomEdge.endVertexValue);
                    Console.WriteLine((generatedNodeCount-1) + ". visited node : " + randomEdge.startVertexValue);
                    Console.WriteLine(generatedNodeCount + ". visited node : " + randomEdge.endVertexValue);
                }

                // Is solution found ?
                bool leftSolutionFound = leftGraph.m_edges.Length == 0;
                bool rightSolutionFound = rightGraph.m_edges.Length == 0;

                // Debug
                if((leftSolutionFound || rightSolutionFound) && debugActive)
                {
                    Console.WriteLine("Best solution is found");
                    Console.WriteLine("Total number of generated nodes : " + generatedNodeCount);
                }

                // if yes
                if (leftSolutionFound)
                {
                    leftVertexCover.Sort();
                    return leftVertexCover;
                }
                else if (rightSolutionFound)
                {
                    rightVertexCover.Sort();
                    return rightVertexCover;
                }

                // Prepare the nodes and add them to the explored nodes list
                Node leftChild = new Node { vertexCover = leftVertexCover, solutionFound = leftSolutionFound };
                Node rightChild = new Node { vertexCover = rightVertexCover, solutionFound = rightSolutionFound };
                exploredNodes.AddLast(leftChild);
                exploredNodes.AddLast(rightChild);
                exploredNodes.RemoveFirst();
                currentNode = exploredNodes.First.Value;
            }
        }


        // Finds the best solution using Depth-First-Search
        // Returns one of the best solutions(if there are more than one)
        public static List<int> BranchAndBound_DFS(Graph G, DFS_Settings settings)
        {
            List<int[]> solutions = new List<int[]>();
            Stack<int> solution = new Stack<int>();
            int[] bestSolution = new int[0];

            if (settings.withBound)
            {
                int vN = G.m_vertices.Length;
                int eN = G.m_edges.Length;
                settings.b1 = (int)Math.Ceiling(eN / (float)G.GetMaxDegreeVertex().degree);
                settings.b3 = (2 * vN - 1 - Math.Sqrt(Math.Pow(2 * vN - 1, 2) - 8 * eN)) / 2;
                switch (settings.heuristicAlgorithm)
                {
                    case "Glouton":
                        settings.b2 = AlgorithmeGlouton(G).Count;
                        break;
                    case "Couplage":
                        settings.b2 = AlgorithmeCouplage(G).Count;
                        break;
                }
            }

            // Reset generated node count
            generatedNodeCount = 0;

            if (settings.eliminateDegreeOne)
            {
                List<int> degreeOneVs = new List<int>();
                G.GetVertexDegrees();

                if(G.GetMaxDegreeVertex().degree == 1)
                {
                    bestSolution = new int[1];
                    bestSolution[0] = G.m_vertices[0].value;
                    return new List<int>(bestSolution);
                }

                foreach(Vertex v in G.m_vertices)
                {
                    if (v.degree == 1) degreeOneVs.Add(v.value);
                }

                // Debug for elimination degree 1
                if (settings.debugActive)
                {
                    Console.Write("Eliminated vertices : { ");
                    foreach (int v in degreeOneVs)
                    {
                        Console.Write(v + " ");
                    }
                    Console.WriteLine("}");
                    
                }

                Graph eliminatedGraph = G.DeleteVertex(degreeOneVs.ToArray());
                // Start branching
                BAB_DFS_Recursive(  eliminatedGraph,
                                    ref solution,
                                    ref solutions,
                                    eliminatedGraph.m_vertices.Length,
                                    settings);
            }
            else
            {
                // Start branching
                BAB_DFS_Recursive(  G, 
                                    ref solution,
                                    ref solutions,
                                    G.m_vertices.Length,
                                    settings);
            }

            // Debug
            if (settings.debugActive)
            {
                Console.WriteLine("Total number of generated nodes : " + generatedNodeCount);
            }

            // We don't keep the solutions which are same amount of vertices as the graph G
            // So if there is no element in the solutions, it means the solution is all vertices.
            if (solutions.Count == 0)
            {
                // best solution is all vertices in the graph
                bestSolution = new int[G.m_vertices.Length];
                for(int i = 0; i < G.m_vertices.Length; i++)
                {
                    bestSolution[i] = G.m_vertices[i].value;
                }
            }
            else
            {
                // Find the best solution among the solutions
                foreach (int[] s in solutions)
                {
                    if (bestSolution.Length == 0)
                    {
                        bestSolution = s;
                        continue;
                    }

                    if (bestSolution.Length > s.Length)
                    {
                        bestSolution = s;
                    }
                }
            }
            
            return new List<int>(bestSolution);
        }


        // Recursive part of the previous method (Simple branching, No min bound)
        private static void BAB_DFS_Recursive(Graph G, ref Stack<int> solution,
                                                ref List<int[]> solutions,
                                                int totalVertexCount,
                                                DFS_Settings settings)
        {
            Random rnd = new Random();

            // Pick a random edge
            if (G.m_edges.Length == 0) return;
            Edge randomEdge = G.m_edges[rnd.Next(0, G.m_edges.Length)];

            // Debug
            if (settings.debugActive)
                Console.WriteLine("Picked edge : " + randomEdge.startVertexValue + " " + randomEdge.endVertexValue);

            //***** BRANCH LEFT SIDE *****//
            // Delete one vertex of the edge from the graph and save it
            Graph leftGraph = G.DeleteVertex(randomEdge.startVertexValue);
            solution.Push(randomEdge.startVertexValue);

            generatedNodeCount += 1;

            // Debug
            if (settings.debugActive)
            {
                Console.WriteLine(generatedNodeCount + ". visited node : " + randomEdge.startVertexValue);
            }

            // For advanced branching
            List<int> includedVertices = new List<int>();

            // For minimum bound check
            int vN = 0, eN = 0, b1 = 0, b2 = 0;
            double b3 = 0;

            // Minimum bound check (LeftChild)
            if(solution.Count < Math.Max(Math.Max(settings.b1, settings.b2), settings.b3) || !settings.withBound)
            {
                // is solution found ?
                bool leftSolutionFound = leftGraph.m_edges.Length == 0;

                if (leftSolutionFound)
                {
                    // Debug
                    if (settings.debugActive)
                        Console.WriteLine("One solution is found!");

                    // Add solution to the solutions list
                    if (solution.Count != totalVertexCount)
                    {
                        solutions.Add(solution.ToArray());
                    }

                    // Debug
                    if (settings.debugActive)
                        Console.WriteLine("Backtracked to parent node");

                    // Backtracking from left node to root node without branching right node
                    solution.Pop();
                    return;
                }
                else
                {
                    // Keep branching left node
                    BAB_DFS_Recursive(leftGraph,
                                        ref solution,
                                        ref solutions,
                                        totalVertexCount,
                                        settings);
                }
            }
            //***** BRANCH LEFT SIDE END *****//

            // Backtracking from left node to root node
            solution.Pop();

            // Debug
            if (settings.debugActive)
                Console.WriteLine("Backtracked to parent node");

            //***** BRANCH RIGHT SIDE *****//
            // Same codes but for right side
            Graph rightGraph = G.DeleteVertex(randomEdge.endVertexValue);
            solution.Push(randomEdge.endVertexValue);

            generatedNodeCount += 1;

            // Debug
            if (settings.debugActive)
            {                
                Console.WriteLine("Previuosly picked edge : " + randomEdge.startVertexValue + " " + randomEdge.endVertexValue);
                Console.WriteLine(generatedNodeCount + ". visited node : " + randomEdge.startVertexValue);
            }

            // Minimum bound check (RightChild)
            if (solution.Count < Math.Max(Math.Max(settings.b1, settings.b2), settings.b3) || !settings.withBound)
            {
                bool rightSolutionFound = rightGraph.m_edges.Length == 0;

                // Apply advanced branching
                if (settings.advancedBranching && !rightSolutionFound)
                {
                    // Add other vertex of the edges connected to the picked vertex
                    foreach (Edge e in G.m_edges)
                    {
                        if (e.startVertexValue == randomEdge.startVertexValue)
                        {
                            if (e.endVertexValue == randomEdge.endVertexValue) continue;
                            solution.Push(e.endVertexValue);
                            includedVertices.Add(e.endVertexValue);
                        }
                        else if (e.endVertexValue == randomEdge.startVertexValue)
                        {
                            if (e.startVertexValue == randomEdge.endVertexValue) continue;
                            solution.Push(e.startVertexValue);
                            includedVertices.Add(e.startVertexValue);
                        }
                    }

                    if (settings.ordered_AB) includedVertices.Sort(new Comparison<int>((i1, i2) => i2.CompareTo(i1)));

                    foreach (int v in includedVertices)
                    {
                        // Debug
                        if (settings.debugActive)
                            Console.WriteLine("Included vertex(Advanced Branching) : " + v);

                        rightGraph = rightGraph.DeleteVertex(v);
                        if (rightGraph.m_edges.Length == 0)
                        {
                            //Debug
                            if (settings.debugActive)
                                Console.WriteLine("One solution is found!");

                            rightSolutionFound = true;
                            break;
                        }
                    }
                }

                if (rightSolutionFound)
                {
                    if (solution.Count != totalVertexCount)
                    {
                        solutions.Add(solution.ToArray());
                    }

                    // Debug
                    if (settings.debugActive)
                        Console.WriteLine("Backtracked to parent node");

                    solution.Pop();
                    if (settings.advancedBranching)
                    {
                        for (int i = 0; i < includedVertices.Count; i++) solution.Pop();
                    }
                    return;
                }
                else
                {
                    BAB_DFS_Recursive(rightGraph,
                                        ref solution,
                                        ref solutions,
                                        totalVertexCount,
                                        settings);
                }
            }
            //***** BRANCH RIGHT SIDE END *****//

            // Debug
            if (settings.debugActive)
                Console.WriteLine("Backtracked to parent node");

            // Backtracking from right node to root node
            solution.Pop();
            if (settings.advancedBranching)
            {
                for (int i = 0; i < includedVertices.Count; i++) solution.Pop();
            }
        }
    }
}
