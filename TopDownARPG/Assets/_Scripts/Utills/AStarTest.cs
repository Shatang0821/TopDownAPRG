using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    public int mapWidth;       // �}�b�v�̕�
    public int mapHeight;      // �}�b�v�̍���
    public float cellSize = 1f;     // �Z���T�C�Y
    public string csvFilePath = "Assets/MapData/map.csv"; // CSV�t�@�C���̃p�X
    public Vector2Int startGrid = new Vector2Int(0, 0);
    public Vector2Int endGrid = new Vector2Int(9, 9);

    private AStar _aStar;
    private int[,] map;
    private List<AStar.AstarNode> path;

    public GameObject movingObjectPrefab;
    private GameObject movingObject;
    private int currentPathIndex = 0;
    private float moveSpeed = 2f;

    private void Start()
    {
        _aStar = new AStar();
        LoadMapFromCSV(csvFilePath);
        _aStar.InitMap(map);
        path = _aStar.FindPath(startGrid, endGrid);

        if (path != null)
        {
            // �ړ��I�u�W�F�N�g�𐶐�
            if (movingObjectPrefab != null)
            {
                movingObject = Instantiate(movingObjectPrefab, GridToWorldPosition(path[0].Pos), Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Path not found.");
        }
    }

    private void Update()
    {
        if (movingObject != null && path != null && currentPathIndex < path.Count)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Vector3 targetPosition = GridToWorldPosition(path[currentPathIndex].Pos);
        movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(movingObject.transform.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
        }
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        // ���[�J�����W�ɕϊ�
        Vector3 localPosition = new Vector3(gridPosition.x * cellSize + cellSize / 2, 0, -gridPosition.y * cellSize - cellSize / 2);
    
        // �}�b�v��Transform���l�����ă��[���h���W�ɕϊ�
        return transform.TransformPoint(localPosition);
        //return new Vector3(gridPosition.x * cellSize + cellSize / 2, 0, -gridPosition.y * cellSize - cellSize / 2);
    }

    private void OnDrawGizmos()
    {
        if (map == null)
        {
            LoadMapFromCSV(csvFilePath);
        }
        DrawPath();
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
                    map[y, x] = int.Parse(values[x]);  // ���̂܂ܓǂݍ���
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
    
    private void DrawPath()
    {
        if (path == null || path.Count == 0)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < path.Count - 1; i++)
        {
            var current = path[i];
            var next = path[i + 1];
            Gizmos.DrawLine(
                GridToWorldPosition(current.Pos),
                GridToWorldPosition(next.Pos)
            );
        }
    }
}
