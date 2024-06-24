using System.Collections.Generic;
using FrameWork.Utils;
using UnityEngine;

public class AStar : Singleton<AStar>
{
    private int[,] map; //マップデータ
    private int width;  //幅
    private int height; //高さ
    public AstarNode[,] nodeArray;
    
    public enum BlockType
    {
        Walk,
        Stop
    }
    
    public void InitMap(int[,] map)
    {
        this.map = map;
        this.width = map.GetLength(1);
        this.height = map.GetLength(0);
        InitAstartNodeArray();
        nodeArray = new AstarNode[width, height];
    }

    /// <summary>
    /// ステージを作成するたびにリセットもしくは初期化させる
    /// </summary>
    private void InitAstartNodeArray()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodeArray[x, y].BlockType = map[x, y] == 1 ? BlockType.Walk : BlockType.Stop;
            }
        }
    }

    public AStar()
    {
        
    }

    /// <summary>
    /// 位置ノード
    /// </summary>
    public class AstarNode
    {
        public BlockType BlockType; //ブロックタイプ
        public Vector2Int Pos;  //グリッド位置
        public AstarNode parent;     //親ノード
        private float g; // スタートから現在の距離コスト
        private float h; // 現在から終点までの距離コスト
        public float f => g + h;//コーストの合計
        
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="x">x位置</param>
        /// <param name="y">y位置</param>
        /// <param name="blockType">ブロックタイプ</param>
        public AstarNode(int x, int y,BlockType blockType)
        {
            this.Pos.x = x;
            this.Pos.y = y;
            this.BlockType = blockType;
            this.g = float.MaxValue;
            this.h = 0;
            this.parent = null;
        }
    }

    public List<AstarNode> FindPath(AstarNode start, AstarNode goal)
    {
        
        return null;
    }
}
