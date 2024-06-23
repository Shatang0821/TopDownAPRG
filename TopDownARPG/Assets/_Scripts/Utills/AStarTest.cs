using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class AStarTest : MonoBehaviour
{
    public int mapWidth = 5;       // マップの幅
    public int mapHeight = 5;      // マップの高さ
    public float cellSize = 1f;     // セルサイズ
    public string csvFilePath = "Assets/MapData/"; // CSVファイルのパス
    public Vector2Int startGrid = new Vector2Int(0, 0);
    public Vector2Int endGrid = new Vector2Int(9, 9);

    private int[,] map;
    private List<AStar.Node> path;

    private void Start()
    {
        LoadMapFromCSV(csvFilePath);
        _ = FindPathAsync(); // 非同期でFindPathを呼び出す
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ = FindPathAsync(); // 非同期でFindPathを呼び出す
        }
    }

    private void OnDrawGizmos()
    {
        if (map == null)
        {
            LoadMapFromCSV(csvFilePath);
        }
        DrawGrid();
        DrawPath();
    }

    private void LoadMapFromCSV(string filePath)
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

    private void DrawGrid()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x <= mapWidth; x++)
        {
            Vector3 start = transform.position + new Vector3(x * cellSize, 0, 0);
            Vector3 end = transform.position + new Vector3(x * cellSize, 0, mapHeight * cellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= mapHeight; y++)
        {
            Vector3 start = transform.position + new Vector3(0, 0, y * cellSize);
            Vector3 end = transform.position + new Vector3(mapWidth * cellSize, 0, y * cellSize);
            Gizmos.DrawLine(start, end);
        }

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
                Gizmos.DrawCube(transform.position + new Vector3(x * cellSize + cellSize / 2, 0, y * cellSize + cellSize / 2), new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }

    private void DrawPath()
    {
        if (path == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 start = transform.position + new Vector3(path[i].x * cellSize + cellSize / 2, 0, path[i].y * cellSize + cellSize / 2);
            Vector3 end = transform.position + new Vector3(path[i + 1].x * cellSize + cellSize / 2, 0, path[i + 1].y * cellSize + cellSize / 2);
            Gizmos.DrawLine(start, end);
        }
    }

    private async Task FindPathAsync()
    {
        if (startGrid.x >= 0 && startGrid.x < mapWidth && startGrid.y >= 0 && startGrid.y < mapHeight &&
            endGrid.x >= 0 && endGrid.x < mapWidth && endGrid.y >= 0 && endGrid.y < mapHeight)
        {
            AStar aStar = new AStar(map);
            AStar.Node startNode = new AStar.Node(startGrid.x, startGrid.y);
            AStar.Node endNode = new AStar.Node(endGrid.x, endGrid.y);

            // 重い処理を別スレッドで実行
            path = await Task.Run(() => aStar.FindPath(startNode, endNode));
        }
        else
        {
            Debug.LogWarning("Start or end grid is out of bounds!");
        }
    }
}
