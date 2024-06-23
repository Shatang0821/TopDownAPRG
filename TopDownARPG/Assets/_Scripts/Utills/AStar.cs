using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private int[,] map;
    private int width;
    private int height;

    public AStar(int[,] map)
    {
        this.map = map;
        this.width = map.GetLength(0);
        this.height = map.GetLength(1);
    }

    public class Node
    {
        public int x;
        public int y;
        public Node parent;
        public float g; // Start to current node cost
        public float h; // Heuristic estimate to goal
        public float f { get { return g + h; } }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.g = float.MaxValue;
            this.h = 0;
            this.parent = null;
        }
    }

    public List<Node> FindPath(Node start, Node goal)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        start.g = 0;
        start.h = Heuristic(start, goal);
        openList.Add(start);

        while (openList.Count > 0)
        {
            openList.Sort((node1, node2) => node1.f.CompareTo(node2.f));
            Node current = openList[0];

            if (current.x == goal.x && current.y == goal.y)
            {
                return ReconstructPath(current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor) || map[neighbor.x, neighbor.y] == 2) continue; // Ignore walls and closed nodes

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }

                float tentativeG = current.g + Distance(current, neighbor);

                if (tentativeG < neighbor.g)
                {
                    neighbor.parent = current;
                    neighbor.g = tentativeG;
                    neighbor.h = Heuristic(neighbor, goal);
                }
            }
        }
        return null; // No path found
    }

    private float Heuristic(Node node, Node goal)
    {
        return Mathf.Abs(node.x - goal.x) + Mathf.Abs(node.y - goal.y);
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        if (node.x - 1 >= 0) neighbors.Add(new Node(node.x - 1, node.y));
        if (node.x + 1 < width) neighbors.Add(new Node(node.x + 1, node.y));
        if (node.y - 1 >= 0) neighbors.Add(new Node(node.x, node.y - 1));
        if (node.y + 1 < height) neighbors.Add(new Node(node.x, node.y + 1));

        return neighbors;
    }

    private List<Node> ReconstructPath(Node node)
    {
        List<Node> path = new List<Node>();
        while (node != null)
        {
            path.Add(node);
            node = node.parent;
        }
        path.Reverse();
        return path;
    }

    private float Distance(Node node1, Node node2)
    {
        return Vector2.Distance(new Vector2(node1.x, node1.y), new Vector2(node2.x, node2.y));
    }
}
