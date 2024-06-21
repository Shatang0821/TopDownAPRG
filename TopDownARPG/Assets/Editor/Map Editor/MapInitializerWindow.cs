using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapInitializerWindow : EditorWindow
{
    private int mapWidth = 16;              //マップの幅
    private int mapHeight = 16;             //マップの高さ
    private GameObject parentObject;        //マップの親オブジェクト
    public GameObject walkablePrefab;       //生成するブロック部分
    
    public GameObject nonWalkablePrefab;

    [MenuItem("Window/Map Initializer")]
    public static void ShowWindow()
    {
        GetWindow<MapInitializerWindow>("Map Initializer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Initialize Map", EditorStyles.boldLabel);

        mapWidth = EditorGUILayout.IntField("Map Width", mapWidth);
        mapHeight = EditorGUILayout.IntField("Map Height", mapHeight);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        walkablePrefab = (GameObject)EditorGUILayout.ObjectField("Walkable Prefab", walkablePrefab, typeof(GameObject), false);
        nonWalkablePrefab = (GameObject)EditorGUILayout.ObjectField("Non-Walkable Prefab", nonWalkablePrefab, typeof(GameObject), false);

        if (GUILayout.Button("Initialize"))
        {
            InitializeMap();
            MapEditorWindow.ShowWindow(mapWidth, mapHeight, parentObject);
            Close();
        }
    }

    private void InitializeMap()
    {
        bool[,] grid = LoadMapData();
        GenerateMap(grid);
    }

    private bool[,] LoadMapData()
    {
        bool[,] grid = new bool[mapWidth, mapHeight];

        string filePath = EditorUtility.OpenFilePanel("Load Map", "", "csv");
        if (!string.IsNullOrEmpty(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    string line = reader.ReadLine();
                    string[] cells = line.Split(',');
                    for (int x = 0; x < mapWidth; x++)
                    {
                        grid[x, y] = cells[x] == "1";
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    grid[x, y] = false; // デフォルトの非移動可能ブロックで埋める
                }
            }
        }

        return grid;
    }

    private void GenerateMap(bool[,] grid)
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent Object is not assigned.");
            return;
        }

        // 既存の子オブジェクトをクリア
        foreach (Transform child in parentObject.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject prefab = grid[x, y] ? walkablePrefab : nonWalkablePrefab;
                GameObject instance = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, parentObject.transform);
                instance.name = $"Cell_{x}_{y}";
            }
        }
    }
}
