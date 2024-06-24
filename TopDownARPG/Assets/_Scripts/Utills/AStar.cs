using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private int[,] map; //マップデータ
    private int width;  //幅
    private int height; //高さ

    public AStar(int[,] map)
    {
        this.map = map;
        this.width = map.GetLength(1);
        this.height = map.GetLength(0);
    }
    
    /// <summary>
    /// 位置ノード
    /// </summary>
    public class Node
    {
        public Vector2Int Pos;  //グリッド位置
        public Node parent;     //親ノード
        public float g; // スタートから現在の距離コスト
        public float h; // 現在から終点までの距離コスト
        public float f => g + h;//コーストの合計

        public Node(int x, int y)
        {
            this.Pos.x = x;
            this.Pos.y = y;
            this.g = float.MaxValue;
            this.h = 0;
            this.parent = null;
        }
    }

}
