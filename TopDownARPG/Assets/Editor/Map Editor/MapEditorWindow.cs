using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapEditorWindow : EditorWindow
{
    // マップ基本情報
    private static int mapWidth;
    private static int mapHeight;
    private static GameObject parentObject;
    // グリッド情報
    private int _gridSize = 20;
    private bool[,] grids;
    private bool _isPainting = false;            // 描くトリガー
    private bool _selectedWalkable = false;
    private bool _isErasing = false;             // 削除トリガー

    private enum Tool
    {
        None,
        Walkable,
        NonWalkable,
        Erase
    }

    private Tool selectedTool = Tool.None;

    public static void ShowWindow(int width, int height, GameObject parent)
    {
        mapWidth = width;
        mapHeight = height;
        parentObject = parent;

        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        grids = new bool[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                grids[x, y] = false;
            }
        }
        selectedTool = Tool.Walkable;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        // 左側のマップ編集エリア
        GUILayout.BeginVertical();
        DrawGrid();
        GUILayout.EndVertical();

        // 右側
        GUILayout.BeginVertical(GUILayout.Width(200));
        DrawTools();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        HandleMouseEvents();
    }

    private void DrawTools()
    {
        GUI.backgroundColor = selectedTool == Tool.Walkable ? Color.green : Color.white;
        if (GUILayout.Button("Walkable"))
        {
            selectedTool = Tool.Walkable;
        }

        GUI.backgroundColor = selectedTool == Tool.NonWalkable ? Color.green : Color.white;
        if (GUILayout.Button("NonWalkable"))
        {
            selectedTool = Tool.NonWalkable;
        }

        GUI.backgroundColor = selectedTool == Tool.Erase ? Color.green : Color.white;
        if (GUILayout.Button("Erase"))
        {
            selectedTool = Tool.Erase;
            _isErasing = true;
        }

        GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        // GUIの背景色を元に戻す
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Save Map"))
        {
            SaveMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            LoadMap();
        }
    }

    private void DrawGrid()
    {
        var rect = GUILayoutUtility.GetRect(mapWidth * _gridSize, mapHeight * _gridSize);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Rect gridRect = new Rect(rect.x + x * _gridSize, rect.y + (mapHeight - 1 - y) * _gridSize, _gridSize, _gridSize);
                EditorGUI.DrawRect(gridRect, grids[x, y] ? Color.blue : Color.red);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, _gridSize, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, 1, _gridSize), Color.gray);
            }
        }
    }

    private void HandleMouseEvents()
    {
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        int x = Mathf.FloorToInt(mousePos.x / _gridSize);
        int y = Mathf.FloorToInt((mapHeight * _gridSize - mousePos.y) / _gridSize); // y座標を調整

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            _isPainting = true;
            if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
            {
                HandlePainting(x, y);
            }
            e.Use();  // イベントを使用済みとしてマークし、他の操作を防止
        }
    }

    private void HandlePainting(int x, int y)
    {
        if (selectedTool == Tool.Erase)
        {
            ChangeGrid(x, y, false);
        }
        else if (selectedTool == Tool.Walkable)
        {
            ChangeGrid(x, y, true);
        }
    }

    private void ChangeGrid(int x, int y, bool b)
    {
        grids[x, y] = b;
        Repaint();
    }

    private void SaveMap()
    {
        string filePath = EditorUtility.SaveFilePanel("Save Map", "", "map.csv", "csv");
        if (string.IsNullOrEmpty(filePath)) return;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int y = 0; y < mapHeight; y++)
            {
                string line = "";
                for (int x = 0; x < mapWidth; x++)
                {
                    line += grids[x, y] ? "1" : "0";
                    if (x < mapWidth - 1)
                    {
                        line += ",";
                    }
                }
                writer.WriteLine(line);
            }
        }
        AssetDatabase.Refresh();
    }

    private void LoadMap()
    {
        string filePath = EditorUtility.OpenFilePanel("Load Map", "", "csv");
        if (string.IsNullOrEmpty(filePath)) return;

        using (StreamReader reader = new StreamReader(filePath))
        {
            for (int y = 0; y < mapHeight; y++)
            {
                string line = reader.ReadLine();
                string[] cells = line.Split(',');
                for (int x = 0; x < mapWidth; x++)
                {
                    grids[x, y] = cells[x] == "1";
                }
            }
        }
        Repaint();

        // マップ生成
        if (parentObject != null)
        {
            foreach (Transform child in parentObject.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    GameObject prefab = grids[x, y] ? Selection.activeGameObject : Selection.activeGameObject;
                    GameObject instance = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, parentObject.transform);
                    instance.name = $"Cell_{x}_{y}";
                }
            }
        }
    }
}
