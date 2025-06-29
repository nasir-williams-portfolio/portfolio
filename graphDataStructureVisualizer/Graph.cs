using System.Collections.Generic;

namespace graphDataStructureVisualizer
{
    internal class Graph
    {
        private List<Vertex> vertices;
        private Dictionary<string, Dictionary<Vertex, Direction>> adjacencyDictionary;

        public List<Vertex> Vertices { get { return vertices; } }

        public Graph(List<Vertex> vertices, Dictionary<string, Dictionary<Vertex, Direction>> adjacencyDictionary)
        {
            this.vertices = vertices;
            this.adjacencyDictionary = adjacencyDictionary;
        }

        public bool MapContainsRoom(string room)
        {
            bool result = false;

            foreach (Vertex mapRoom in vertices)
            {
                if (mapRoom.Name == room)
                {
                    result = true;
                }
            }

            return result;
        }

        public Dictionary<Vertex, Direction> GetAdjacentDictionary(string room)
        {
            if (adjacencyDictionary[room].Count != 0)
            {
                return adjacencyDictionary[room];
            }

            else
            {
                return null;
            }
        }
    }
}
