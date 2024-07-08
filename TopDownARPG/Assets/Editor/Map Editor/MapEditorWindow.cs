using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapEditorWindow : EditorWindow
{
    // マップ基本情報
    private static int mapWidth;                //マップの幅        
    private static int mapHeight;               //マップの高さ
    private static GameObject parentObject;     //マップの親オブジェクト
    // グリッド情報
    private int _gridSize = 15;
    private int[,] grids; // グリッドの状態を管理するint型配列
    private bool _isPainting = false;            // 描くトリガー
    private bool _isErasing = false;             // 削除トリガー

    [SerializeField] private GameObject wallPrefab; // 壁のプレハブ
    [SerializeField] private GameObject groundPrefab; // 地面のプレハブ
    
    //ツール
    private enum Tool
    {
        None,
        Wall,       //壁
        Ground,     //地面
        Erase       //削除
    }

    private Tool selectedTool = Tool.None;

    public static void ShowWindow(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;

        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        grids = new int[mapWidth, mapHeight]; // グリッドの状態を0で初期化
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                grids[x, y] = 0; // デフォルトは0
            }
        }

        selectedTool = Tool.Wall;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        // 左側のマップ編集エリア
        GUILayout.BeginVertical();
        DrawGrid();
        GUILayout.EndVertical();

        // 右側
        GUILayout.BeginVertical(GUILayout.Width(350));
        DrawTools();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        HandleMouseEvents();
    }

    private void DrawTools()
    {
        wallPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Prefab", wallPrefab, typeof(GameObject), false);
        groundPrefab = (GameObject)EditorGUILayout.ObjectField("Ground Prefab", groundPrefab, typeof(GameObject), false);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);
        //ボタンの描画
        GUI.backgroundColor = selectedTool == Tool.Ground ? Color.green : Color.white;
        if (GUILayout.Button("Ground"))
        {
            _isErasing = false;
            selectedTool = Tool.Ground;
            _isPainting = true;
        }

        GUI.backgroundColor = selectedTool == Tool.Wall ? Color.green : Color.white;
        if (GUILayout.Button("Wall"))
        {
            _isErasing = false;
            selectedTool = Tool.Wall;
            _isPainting = true;
        }

        GUI.backgroundColor = selectedTool == Tool.Erase ? Color.green : Color.white;
        if (GUILayout.Button("Erase"))
        {
            selectedTool = Tool.Erase;
            _isErasing = true;
            _isPainting = true; // 長押しで削除できるようにするため
        }

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
    
    /// <summary>
    /// グリッドの描画
    /// </summary>
    private void DrawGrid()
    {
        var rect = GUILayoutUtility.GetRect(mapWidth * _gridSize, mapHeight * _gridSize);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Rect gridRect = new Rect(rect.x + x * _gridSize, rect.y + y * _gridSize, _gridSize, _gridSize);
                Color color;
                if (grids[x, y] == 0) // デフォルト（歩けない）
                {
                    color = Color.white;
                }
                else if (grids[x, y] == 1) // 地面（歩ける）
                {
                    color = Color.blue;
                }
                else // 壁（歩けない）
                {
                    color = Color.red;
                }
                EditorGUI.DrawRect(gridRect, color);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, _gridSize, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, 1, _gridSize), Color.gray);
            }
        }
    }
    
    /// <summary>
    /// 入力ハンドル
    /// </summary>
    private void HandleMouseEvents()
    {
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        int x = Mathf.FloorToInt(mousePos.x / _gridSize);
        int y = Mathf.FloorToInt(mousePos.y / _gridSize); // y座標を調整

        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            _isPainting = true;
            if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
            {
                Debug.Log(x + "," + y);
                HandlePainting(x, y);
            }
            e.Use();  // イベントを使用済みとしてマークし、他の操作を防止
        }
        else if (e.type == EventType.MouseUp && e.button == 0)
        {
            _isPainting = false;
        }
    }
    
    /// <summary>
    /// 描画ハンドル
    /// </summary>
    /// <param name="x">生成x座標</param>
    /// <param name="y">y座標</param>
    private void HandlePainting(int x, int y)
    {
        if (selectedTool == Tool.Erase)
        {
            ChangeGrid(x, y, 0); // 削除（デフォルトの白に戻す）
            DestroyGridObject(x, y);
        }
        else if (selectedTool == Tool.Wall)
        {
            if (grids[x, y] == 0) // 既にブロックがない場合のみ生成
            {
                ChangeGrid(x, y, 2); // 壁
                CreateGridObject(x, y, wallPrefab);
            }
        }
        else if (selectedTool == Tool.Ground)
        {
            if (grids[x, y] == 0) // 既にブロックがない場合のみ生成
            {
                ChangeGrid(x, y, 1); // 地面
                CreateGridObject(x, y, groundPrefab);
            }
        }
    }

    private void ChangeGrid(int x, int y, int state)
    {
        grids[x, y] = state; // グリッドの状態を更新
        Repaint();
    }

    private void CreateGridObject(int x, int y, GameObject prefab)
    {
        if (prefab == null || parentObject == null) return;

        Vector3 position = new Vector3(x, 0, -y);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, parentObject.transform);
        instance.name = $"{prefab.name}_{x}_{y}";
    }

    private void DestroyGridObject(int x, int y)
    {
        if (parentObject == null) return;

        Transform child = parentObject.transform.Find($"{wallPrefab.name}_{x}_{y}");
        if (child == null)
        {
            child = parentObject.transform.Find($"{groundPrefab.name}_{x}_{y}");
        }
        if (child != null)
        {
            DestroyImmediate(child.gameObject);
        }
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
                    line += grids[x, y]; // 0: 空白、1: 地面、2: 壁
                    Debug.Log(grids[x,y]);
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
                    
                    grids[x, y] = int.Parse(cells[x]); // 0: 空白、1: 地面、2: 壁
                    Debug.Log(x+","+y+","+grids[x,y]);
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
                    if (grids[x, y] == 1)
                    {
                        CreateGridObject(x, y, groundPrefab);
                    }
                    else if (grids[x, y] == 2)
                    {
                        CreateGridObject(x, y, wallPrefab);
                    }
                }
            }
        }
    }
}
