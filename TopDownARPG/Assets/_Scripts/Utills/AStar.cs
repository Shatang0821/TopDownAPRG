using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar
{
    private int width;  //幅
    private int height; //高さ
    public AstarNode[,] nodeArray;

    private List<AstarNode> _openList;
    private List<AstarNode> _closeList;

    public enum BlockType
    {
        Walk,
        Stop
    }

    public void InitMap(int[,] map)
    {
        this.width = map.GetLength(1);
        this.height = map.GetLength(0);
        nodeArray = new AstarNode[width, height];

        InitAstarNodeArray(map);
        _openList = new List<AstarNode>(width * height);
        _closeList = new List<AstarNode>(width * height);
    }

    private void InitAstarNodeArray(int[,] map)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var type = map[y, x] == 1 ? BlockType.Walk : BlockType.Stop;
                nodeArray[x, y] = new AstarNode(x, y, type);
                Debug.Log($"Node initialized at ({x}, {y}) with type {type}");
            }
        }
    }
    
    private void ResetNodes()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodeArray[x, y].parent = null;
                nodeArray[x, y].G = float.MaxValue;
                nodeArray[x, y].H = 0;
                nodeArray[x, y].F = 0;
            }
        }
    }
    
    public class AstarNode
    {
        public BlockType BlockType;
        public Vector2Int Pos;
        public AstarNode parent;
        public float G;
        public float H;
        public float F;

        public AstarNode(int x, int y, BlockType blockType)
        {
            this.Pos.x = x;
            this.Pos.y = y;
            this.BlockType = blockType;
            this.G = float.MaxValue;
            this.H = 0;
            this.parent = null;
        }
    }

    public List<AstarNode> FindPath(Vector2Int start, Vector2Int goal)
    {
        _openList.Clear();
        _closeList.Clear();
        ResetNodes();
        var startNode = nodeArray[start.x, start.y];
        var goalNode = nodeArray[goal.x, goal.y];

        Debug.Log("Start Node: " + startNode.Pos + " Goal Node: " + goalNode.Pos);
        Debug.Log("Start Type: " + startNode.BlockType + " Goal Type: " + goalNode.BlockType);
    
        if (IsMapExternal(startNode.Pos.x, startNode.Pos.y) || IsMapExternal(goalNode.Pos.x, goalNode.Pos.y))
        {
            Debug.LogWarning("Start or Goal is out of bounds");
            return null;
        }

        if (startNode.BlockType == BlockType.Stop || goalNode.BlockType == BlockType.Stop)
        {
            Debug.LogWarning("Start or Goal is a blocking type");
            return null;
        }

        FindEndPoint(startNode, goalNode);
        return _closeList;
    }

    private void FindEndPoint(AstarNode startNode, AstarNode goalNode)
    {
        startNode.G = 0;
        startNode.H = Vector2Int.Distance(startNode.Pos, goalNode.Pos);
        startNode.F = startNode.G + startNode.H;
        _openList.Add(startNode);

        int maxSteps = width * height * 10; // 適切な最大ステップ数を設定
        int steps = 0;

        while (_openList.Count > 0 && steps < maxSteps)
        {
            _openList.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            var currentNode = _openList[0];

//            Debug.Log("Current Node: " + currentNode.Pos);
                
            if (currentNode.Pos == goalNode.Pos)
            {
                Debug.Log("Current Node: " + currentNode.Pos);
                Debug.Log("ここまで");
                var path = new List<AstarNode>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                _closeList = path;

//                Debug.Log("Path found: " + string.Join(" -> ", path.Select(n => n.Pos.ToString()).ToArray()));
                return;
            }
            
            
            _openList.Remove(currentNode);
            _closeList.Add(currentNode);

            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (_closeList.Contains(neighbor) || neighbor.BlockType == BlockType.Stop)
                    continue;

                float tentativeG = currentNode.G + Vector2Int.Distance(currentNode.Pos, neighbor.Pos);

                if (!_openList.Contains(neighbor) || tentativeG < neighbor.G)
                {
                    neighbor.parent = currentNode;
                    neighbor.G = tentativeG;
                    neighbor.H = Vector2Int.Distance(neighbor.Pos, goalNode.Pos);
                    neighbor.F = neighbor.G + neighbor.H;

                    if (!_openList.Contains(neighbor))
                        _openList.Add(neighbor);
                }
            }
           

            steps++;
        }

        if (steps >= maxSteps)
        {
            Debug.LogWarning("Pathfinding terminated due to exceeding max steps.");
        }
    }

    private List<AstarNode> GetNeighbors(AstarNode node)
    {
        var neighbors = new List<AstarNode>(4); // 初期容量を設定

        int[,] directions = new int[,]
        {
            { 0, 1 },  // 上
            { 1, 0 },  // 右
            { 0, -1 }, // 下
            { -1, 0 }  // 左
        };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.Pos.x + directions[i, 0];
            int checkY = node.Pos.y + directions[i, 1];

            if (IsMapExternal(checkX, checkY))
                continue;

            neighbors.Add(nodeArray[checkX, checkY]);
        }

        return neighbors;
    }

    private bool IsMapExternal(int x, int y)
    {
        return x < 0 || x >= width || y < 0 || y >= height;
    }
}
