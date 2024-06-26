using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private int width;  //幅
    private int height; //高さ
    public AstarNode[,] nodeArray;

    private List<AstarNode> _openList = new List<AstarNode>();
    private List<AstarNode> _closeList = new List<AstarNode>();
    
    public enum BlockType
    {
        Walk,
        Stop
    }
    
    /// <summary>
    /// マップ初期化
    /// </summary>
    /// <param name="map">マップデータ</param>
    public void InitMap(int[,] map)
    {
        //マップデータ初期化
        this.width = map.GetLength(1);
        this.height = map.GetLength(0);
        nodeArray = new AstarNode[width, height];
        
        InitAstarNodeArray(map);
        _openList.Clear();
        _closeList.Clear();
    }

    /// <summary>
    /// ステージを作成するたびにリセットもしくは初期化させる 
    /// </summary>
    private void InitAstarNodeArray(int[,] map)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var type = map[y, x] == 1 ? BlockType.Walk : BlockType.Stop;
                nodeArray[x, y] = new AstarNode(x, y, type);
            }
        }
    }

    /// <summary>
    /// 位置ノード
    /// </summary>
    public class AstarNode
    {
        public BlockType BlockType; //ブロックタイプ
        public Vector2Int Pos;  //グリッド位置
        public AstarNode parent;     //親ノード
        /// <summary>
        /// スタートから現在の距離コスト
        /// </summary>
        public float G;
        /// <summary>
        /// 現在から終点までの距離コスト
        /// </summary>
        public float H; 
        /// <summary>
        /// コーストの合計
        /// </summary>
        public float F;
        
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="x">x位置</param>
        /// <param name="y">y位置</param>
        /// <param name="blockType">ブロックタイプ</param>
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

        var startNode = nodeArray[start.x, start.y];
        var goalNode = nodeArray[goal.x, goal.y];
        
        var startX = startNode.Pos.x;
        var startY = startNode.Pos.y;
        var goalX = goalNode.Pos.x;
        var goalY = goalNode.Pos.y;
        
        //ノードが範囲内かをチェック
        if (IsMapExternal(startX, startY) || IsMapExternal(goalX, goalY))
        {
            Debug.LogWarning("スタートもしくはゴールが範囲外");
            return null;
        }

        
        //ノードが障害物なのかをチェック
        if (startNode.BlockType == BlockType.Stop || goalNode.BlockType == BlockType.Stop)
        {
            Debug.LogWarning("スタートもしくはゴールが障害物型");
            return null;
        }
        
        FindEndPoint(startNode, goalNode);
        return _closeList;
    }

    /// <summary>
    /// ルート探索処理メソッド
    /// </summary>
    /// <param name="startNode">スタートノード</param>
    /// <param name="goalNode">ゴールノード</param>
    private void FindEndPoint(AstarNode startNode, AstarNode goalNode)
    {
        startNode.G = 0;
        startNode.H = Vector2Int.Distance(startNode.Pos, goalNode.Pos);
        startNode.F = startNode.G + startNode.H;
        _openList.Add(startNode);

        while (_openList.Count > 0)
        {
            _openList.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            var currentNode = _openList[0];

            if (currentNode.Pos == goalNode.Pos)
            {
                // ゴールに到達したので経路を復元
                var path = new List<AstarNode>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                _closeList = path;
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
        }
    }

    /// <summary>
    /// 隣接ブロックの取得
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private List<AstarNode> GetNeighbors(AstarNode node)
    {
        var neighbors = new List<AstarNode>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.Pos.x + x;
                int checkY = node.Pos.y + y;

                if (IsMapExternal(checkX, checkY))
                    continue;

                neighbors.Add(nodeArray[checkX, checkY]);
            }
        }

        return neighbors;
    }

    /// <summary>
    /// マップ外チェック
    /// </summary>
    /// <param name="x">x nodeポイント</param>
    /// <param name="y">y nodeポイント</param>
    /// <returns>マップ外true マップ内false</returns>
    private bool IsMapExternal(int x, int y)
    {
        return x < 0 || x >= width || y < 0 || y >= height;
    }
}
