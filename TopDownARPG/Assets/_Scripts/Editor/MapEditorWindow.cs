using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditorWindow : EditorWindow
{
    //マップ基本情報
    private static int mapWidth;
    private static int mapHeight;
    private static GameObject parentObject;

    private static Dictionary<GameObject, bool> prefabTable = new Dictionary<GameObject, bool>();
    //グリッド情報
    private int gridSize = 20;
    private bool[,] grids;
    private bool isPainting = false;            //描くトリガー
    private GameObject selectedPrefab = null;   //選択しているプレハブ
    private bool selectedWalkable = false;
    private bool isErasing = false;             //削除トリガー

    private enum Tool
    {
        None,
        Walkable,
        NonWalkable,
        Erase
    }

    private Tool selectedTool = Tool.None;

    public static void ShowWindow(int width, int height, GameObject parent, List<GameObject> prefabs,List<bool> prefabWalkableList)
    {
        mapWidth = width;
        mapHeight = height;
        parentObject = parent;
        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabTable.Add(prefabs[i],prefabWalkableList[i]);
        }

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
        
        //左側のマップ編集エリア
        GUILayout.BeginVertical();
        //
        DrawGrid();
        GUILayout.EndVertical();
        
        //右側
        GUILayout.BeginVertical(GUILayout.Width(200));
        //
        DrawTools();
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        HandleMouseEvents();
    }

    private void DrawTools()
    {
        GUI.backgroundColor = selectedTool == Tool.Walkable ? Color.green : Color.white;
        if (GUILayout.Button("WalkAble"))
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
            isErasing = true;
        }
        
        GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        int i = 0;
        foreach (var entry in prefabTable)
        {
            if (entry.Key != null)
            {
                GUI.backgroundColor = selectedPrefab == entry.Key ? Color.green : Color.white;
                if (GUILayout.Button(AssetPreview.GetAssetPreview(entry.Key), GUILayout.Height(50)))
                {
                    selectedPrefab = entry.Key;
                    selectedWalkable = entry.Value;
                    selectedTool = Tool.None; // キャンセル描画モード
                }
                i++;
            }
        }
        // GUIの背景色を元に戻す
        GUI.backgroundColor = Color.white;
    }

    //gridを描画
    private void DrawGrid()
    {
        var rect = GUILayoutUtility.GetRect(mapWidth * gridSize, mapHeight * gridSize);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Rect gridRect = new Rect(rect.x + x * gridSize, rect.y + (mapHeight - 1 - y) * gridSize, gridSize,
                    gridSize);
                EditorGUI.DrawRect(gridRect, grids[x, y] ? Color.blue : Color.red);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, gridSize, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, 1, gridSize), Color.gray);
            }
        }
    }
    
    private void HandleMouseEvents()
    {
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        int x = Mathf.FloorToInt(mousePos.x / gridSize);
        int y = Mathf.FloorToInt((mapHeight * gridSize - mousePos.y) / gridSize); // y座標を調整
        
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            isPainting = true;
            if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
            {
                HandlePainting(x, y);
            }
            e.Use();  // イベントを使用済みとしてマークし、他の操作を防止
        }
    }

    //描く処理
    private void HandlePainting(int x, int y)
    {
        if (selectedTool == Tool.Erase)
        {
            ChangeGrid(x,y,false);
        }
    }

    private void ChangeGrid(int x, int y, bool b)
    {
        grids[x, y] = b;
        Repaint();
    }
}