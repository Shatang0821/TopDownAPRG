using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageDetail", menuName = "Stage/StageConfig", order = 1)]
public class StageDetail : ScriptableObject
{
    public GameObject StagePrefab; // ステージオブジェクト
    public string DataFilePath; // ステージデータ
    public int[,] Map; // ステージ２次元データ
    public int MapWidth; // マップの幅
    public int MapHeight; // マップの高さ

    public Vector3 SpawnPos; //生成位置
    public float CellSize = 1f; //ブロックサイズ

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
            if (lines.Length > 0)
            {
                Map = new int[lines.Length, lines[0].Split(',').Length];

                for (int y = 0; y < lines.Length; y++)
                {
                    string[] values = lines[y].Split(',');
                    for (int x = 0; x < values.Length; x++)
                    {
                        if (int.TryParse(values[x], out int result))
                        {
                            Map[y, x] = result;
                        }
                        else
                        {
                            Debug.LogError($"Invalid format for map value at ({y}, {x}): {values[x]}");
                            // Optionally, set a default value or handle the error as needed
                        }
                    }
                }

                MapHeight = Map.GetLength(0);
                MapWidth = Map.GetLength(1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading map from CSV: " + e.Message);
        }
    }
}