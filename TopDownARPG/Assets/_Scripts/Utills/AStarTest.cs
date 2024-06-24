using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class AStarTest : MonoBehaviour
{
    public int mapWidth;       // マップの幅
    public int mapHeight;      // マップの高さ
    public float cellSize = 1f;     // セルサイズ
    public string csvFilePath = "Assets/MapData/map.csv"; // CSVファイルのパス
    public Vector2Int startGrid = new Vector2Int(0, 0);
    public Vector2Int endGrid = new Vector2Int(9, 9);

    private int[,] map;
    private List<AStar.AstarNode> path;

    private void Start()
    {
        LoadMapFromCSV(csvFilePath);
    }

    private void Update()
    {
       
    }

    private void OnDrawGizmos()
    {
        if (map == null)
        {
            LoadMapFromCSV(csvFilePath);
        }
        DrawGrid();
    }

    private void LoadMapFromCSV(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            map = new int[lines.Length, lines[0].Split(',').Length];

            for (int y = 0; y < lines.Length; y++)
            {
                string[] values = lines[y].Split(',');
                for (int x = 0; x < values.Length; x++)
                {
                    map[y, x] = int.Parse(values[x]);  // そのまま読み込む
                }
            }

            mapHeight = map.GetLength(0);
            mapWidth = map.GetLength(1);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading map from CSV: " + e.Message);
        }
    }

    private void DrawGrid()
    {
        Gizmos.color = Color.gray;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (map[y, x] == 1)
                {
                    Gizmos.color = Color.blue;
                }
                else if (map[y, x] == 2)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawCube(transform.position + new Vector3(x * cellSize + cellSize / 2, 0, -y * cellSize + cellSize / 2), new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }

    
}
