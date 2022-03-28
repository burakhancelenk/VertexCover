using System;
using System.Collections.Generic;

namespace COMPLEX_Project_CELENK_Burakhan
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage, see Graph and VertexCover classes.
            Graph G2 = new Graph();
            G2.GenerateRandomGraph(50, 0.14f);

            DFS_Settings settings = new DFS_Settings
            {
                withBound = true,
                heuristicAlgorithm = "Couplage",
                advancedBranching = false,
                ordered_AB = false,
                eliminateDegreeOne = false
            };

            Tester.TestAlgorithmeGlouton(G2);

        }
    }
}
