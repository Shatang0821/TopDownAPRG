using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageDetail", menuName = "Stage/StageConfig", order = 1)]
public class StageDetail : ScriptableObject
{
    public GameObject StagePrefab;  // ステージオブジェクト
    public string DataFilePath;     // ステージデータ
    public int[,] Map;              // ステージ２次元データ
    public int MapWidth;            // マップの幅
    public int MapHeight;           // マップの高さ

    public Vector3 SpawnPos;        //生成位置
    public float CellSize = 1f;     //ブロックサイズ
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        LoadMapFromCSV(DataFilePath);
    }
    
    /// <summary>
    /// CSVからMapデータを読み取る
    /// </summary>
    /// <param name="filePath">パス</param>
    private void LoadMapFromCSV(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            Map = new int[lines.Length, lines[0].Split(',').Length];

            for (int y = 0; y < lines.Length; y++)
            {
                string[] values = lines[y].Split(',');
                for (int x = 0; x < values.Length; x++)
                {
                    Map[y, x] = int.Parse(values[x]);  // そのまま読み込む
                }
            }

            MapHeight = Map.GetLength(0);
            MapWidth = Map.GetLength(1);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading map from CSV: " + e.Message);
        }
    }
}
