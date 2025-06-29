using System.Collections.Generic;

namespace graphDataStructureVisualizer
{
    internal class Graph
    {
        private List<Vertex> vertices;
        private Dictionary<string, List<Vertex>> adjacencyList;

        public List<Vertex> Vertices { get { return vertices; } }

        public Graph(List<Vertex> vertices, Dictionary<string, List<Vertex>> adjacencyList)
        {
            this.vertices = vertices;
            this.adjacencyList = adjacencyList;
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

        public bool AreAdjacent(string firstRoom, string secondRoom)
        {
            bool result = false;

            foreach (Vertex door in adjacencyList[firstRoom])
            {
                if (door.Name == secondRoom)
                {
                    result = true;
                }
            }

            return result;
        }

        public List<Vertex> GetAdjacentList(string room)
        {
            if (adjacencyList[room].Count != 0)
            {
                return adjacencyList[room];
            }

            else
            {
                return null;
            }
        }
    }
}
