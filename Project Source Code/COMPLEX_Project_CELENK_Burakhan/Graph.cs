using System;
using System.Collections.Generic;
using System.Text;

namespace COMPLEX_Project_CELENK_Burakhan
{
    /// <summary>
    /// Some basic structures have been created for better handling the Graph class.
    /// GenerateRandomGraph() and ReadFromTextFile() functions have been embedded into the Graph class.
    /// For use one of these functions, create a new empty graph by using default constructor then call
    /// one of these functions. It will prepare the new graph infos and save it into the created graph with default constructor.
    /// </summary>

    struct Vertex
    {
       public int value; // vertex value (given by user)
       public int degree; // degree of the vertex (set automatically)
    }
    struct Edge
    {
        // Two endpoints of the edge
        public int startVertexValue;
        public int endVertexValue;
    }
    class Graph
    {
        public Edge[] m_edges;
        public Vertex[] m_vertices; // sorted in ascending order according to the vertex degree

        private bool degreesCalculated = false; // flag

        public bool AreDegreesCalculated()
        {
            return degreesCalculated;
        }

        public Graph()
        {
            // Default constructor to be able to construct graph from a text file
            // or to be able to generate a random graph
            m_edges = null;
            m_vertices = null;
        }


        public Graph(int[] vertexValues, Edge[] edges)
        {
            // Build graph
            m_vertices = new Vertex[vertexValues.Length];
            m_edges = edges;

            // Copy vertex values
            for (int i = 0; i < m_vertices.Length; i++)
            {
                m_vertices[i].value = vertexValues[i];
            }
        }


        public void ReadFromTextFile(string filePath)
        {
            // Read the text file and split them into lines
            string[] lines = System.IO.File.ReadAllLines(filePath);

            if(lines[0] == "")
            {
                // Text file is empty
                throw new Exception("File is empty");
            }

            // For tracking current state of reading process
            string currentState = "";

            // Graph infos
            int vertexCount;
            int[] vertexValues = new int[0];
            int edgeCount;
            Edge[] edges = new Edge[0];

            // For tracking available slot in the vertex and edge arrays
            int currentIndex = 0;


            foreach (string line in lines)
            {
                if(line == "Nombre de sommets" ||
                    line == "Sommets" ||
                    line == "Nombre d\'aretes" ||
                    line == "Aretes")
                {
                    // Switch to new reading state, this one is title line
                    currentState = line;
                    continue;
                }

                switch (currentState)
                {
                    case "Nombre de sommets":
                        // Save the vertex count and create an array for vertices
                        vertexCount = Convert.ToInt32(line);
                        vertexValues = new int[vertexCount];
                        break;
                    case "Sommets":
                        // Save the current vertex value and increase the index
                        vertexValues[currentIndex] = Convert.ToInt32(line);
                        currentIndex += 1;
                        break;
                    case "Nombre d\'aretes":
                        // Save the edge count, create an array for edges and reset the index
                        edgeCount = Convert.ToInt32(line);
                        edges = new Edge[edgeCount];
                        currentIndex = 0;
                        break;
                    case "Aretes":
                        // Save the current edge infos(endpoint values) and increase the index 
                        string[] endPoints = line.Split(' ');
                        edges[currentIndex].startVertexValue = Convert.ToInt32(endPoints[0]);
                        edges[currentIndex].endVertexValue = Convert.ToInt32(endPoints[1]);
                        currentIndex += 1;
                        break;
                }
            }

            // Build graph
            m_vertices = new Vertex[vertexValues.Length];
            m_edges = edges;

            // Copy vertex values
            for (int i = 0; i < m_vertices.Length; i++)
            {
                m_vertices[i].value = vertexValues[i];
            }
        }


        public void GenerateRandomGraph(int n, float p)
        {
            // Arguments range check
            if(p >= 1 || p < 0 || n <= 0)
            {
                throw new ArgumentOutOfRangeException("Probability value must be between 0 and 1, n must be bigger than 0");
            }

            // Vertex preparation step
            int[] vertexValues = new int[n];
            for(int i=0; i < vertexValues.Length; i++)
            {
                vertexValues[i] = i+1;
            }

            // Edge preparation step
            List<Edge> edges = new List<Edge>();
            Random rnd = new Random();

            while(edges.Count == 0 && n > 1)
            {
                // Execute the probability for each possible vertex couple
                for (int i = 1; i <= vertexValues.Length; i++)
                {
                    for (int j = i + 1; j <= vertexValues.Length; j++)
                    {
                        // Sensitivity of probability => 3 digits (0.000)

                        // Pick a random integer number between [0,1000[ and compare it with p*1000
                        int number = rnd.Next(1, 1000);
                        if ((int)(p * 1000) >= number)
                        {
                            Edge e = new Edge { startVertexValue = i, endVertexValue = j };
                            edges.Add(e);
                        }
                    }
                }
            }

            // Build graph
            m_vertices = new Vertex[vertexValues.Length];
            m_edges = edges.ToArray();

            // Copy vertex values
            for (int i = 0; i < m_vertices.Length; i++)
            {
                m_vertices[i].value = vertexValues[i];
            }

        }


        public Vertex[] GetVertexDegrees()
        {
            // Calculate degrees for each vertex and save it
            for (int i = 0; i < m_vertices.Length; i++)
            {
                int currentValue = m_vertices[i].value;
                int currentDegree = 0;

                for (int j = 0; j < m_edges.Length; j++)
                {
                    if (currentValue == m_edges[j].endVertexValue
                        || currentValue == m_edges[j].startVertexValue)
                    {
                        currentDegree += 1;
                    }
                }

                m_vertices[i].degree = currentDegree;
            }
            // Mark the graph
            degreesCalculated = true;

            return m_vertices;
        }


        public Vertex GetMaxDegreeVertex()
        {
            List<Vertex> maxDegreeVertices = new List<Vertex>();
            maxDegreeVertices.Add( new Vertex { value = -1, degree = -1 });

            if (!degreesCalculated)
            {
                GetVertexDegrees();
            }

            // Traverse all vertices
            for (int i = 0; i < m_vertices.Length; i++)
            {
                if (maxDegreeVertices[0].degree < m_vertices[i].degree)
                {
                    // Bigger degree have found
                    maxDegreeVertices.Clear();
                    maxDegreeVertices.Add(m_vertices[i]);
                }
                else if(maxDegreeVertices[0].degree == m_vertices[i].degree)
                {
                    maxDegreeVertices.Add(m_vertices[i]);
                }
            }

            // If there are more than one max degree vertex, pick one of them randomly
            Random rnd = new Random();
            int rndIndice = rnd.Next(0, maxDegreeVertices.Count);
            return maxDegreeVertices[rndIndice];
        }

        public Vertex[] GetAllMaxDegreeVertices()
        {
            List<Vertex> maxDegreeVertices = new List<Vertex>();
            maxDegreeVertices.Add(new Vertex { value = -1, degree = -1 });

            if (!degreesCalculated)
            {
                GetVertexDegrees();
            }

            // Traverse all vertices
            for (int i = 0; i < m_vertices.Length; i++)
            {
                if (maxDegreeVertices[0].degree < m_vertices[i].degree)
                {
                    // Bigger degree have found
                    maxDegreeVertices.Clear();
                    maxDegreeVertices.Add(m_vertices[i]);
                }
                else if (maxDegreeVertices[0].degree == m_vertices[i].degree)
                {
                    maxDegreeVertices.Add(m_vertices[i]);
                }
            }

            return maxDegreeVertices.ToArray();
        }


        // One element deletion case
        public Graph DeleteVertex(int v)
        {
            // Handling vertices //

            int[] newVertexValues = new int[m_vertices.Length - 1];
            
            int deletedIndex = -1;
            // Traverse in the vertices
            for(int i = 0; i < m_vertices.Length; i++)
            {
                if(m_vertices[i].value == v)
                {
                    // Vertex have found, save the index and break the loop
                    deletedIndex = i;
                    break;
                }
                // Copy elements untill encounter the deleted vertex
                newVertexValues[i] = m_vertices[i].value;
            }

            // Copy the rest of them
            for (int i = deletedIndex; i + 1 < m_vertices.Length; i++)
            {
                newVertexValues[i] = m_vertices[i + 1].value;
            }

            // Handling vertices end //

            // Handling Edges // 

            List<Edge> newEdges = new List<Edge>();
            for (int i = 0; i < m_edges.Length; i++)
            {
                if (m_edges[i].startVertexValue == v || m_edges[i].endVertexValue == v)
                {
                    // Deleted edge, skip!
                    continue;
                }

                newEdges.Add(m_edges[i]);
            }

            // Handling edges end //

            // Prepare the new graph
            Graph newGraph = new Graph();
            newGraph.m_vertices = new Vertex[newVertexValues.Length];
            newGraph.m_edges = newEdges.ToArray();

            for (int i = 0; i < newGraph.m_vertices.Length; i++)
            {
                newGraph.m_vertices[i].value = newVertexValues[i];
            }

            return newGraph;
        }


        // Multiple element deletion case (same method overloaded)
        public Graph DeleteVertex(int[] vertices)
        {
            // Handling vertices //

            List<int> newVertexValues = new List<int>();

            // Traverse all the vertices
            for (int i = 0; i < m_vertices.Length; i++)
            {
                // Compare each vertex
                foreach (int v in vertices)
                {
                    if (m_vertices[i].value == v)
                    {
                        // Deleted vertex, skip!
                        goto end_of_loop;
                    }
                }

                newVertexValues.Add(m_vertices[i].value);
                end_of_loop:
                ;
            }

            // Handling vertices end //

            // Handling Edges // 

            List<Edge> newEdges = new List<Edge>();

            // Traverse all the edges
            for (int i = 0; i < m_edges.Length; i++)
            {
                // Compare each vertex with endpoints
                foreach (int v in vertices)
                {
                    if (m_edges[i].startVertexValue == v || m_edges[i].endVertexValue == v)
                    {
                        // Deleted edge, skip!
                        goto end_of_loop;
                    }
                }

                newEdges.Add(m_edges[i]);
                end_of_loop:
                ;
            }

            // Handling edges end //

            // Prepare the new graph
            Graph newGraph = new Graph();
            newGraph.m_vertices = new Vertex[newVertexValues.Count];
            newGraph.m_edges = newEdges.ToArray();

            for (int i = 0; i < newVertexValues.Count; i++)
            {
                newGraph.m_vertices[i].value = newVertexValues[i];
            }

            return newGraph;
        }
    }
}
