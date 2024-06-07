using UnityEditor;
using UnityEngine;

public class MapEditorWindow : EditorWindow
{
    private static int mapWidth;
    private static int mapHeight;
    private static float cubeSize;
    private static GameObject parentObject;

    private int gridSize = 20;
    private Color[,] gridColors;
    private bool isPainting = false;
    private bool isErasing = false;
    private Color selectedColor = Color.red; // 默认颜色为红色

    public static void ShowWindow(int width, int height, float size, GameObject parent)
    {
        mapWidth = width;
        mapHeight = height;
        cubeSize = size;
        parentObject = parent;
        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        gridColors = new Color[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                gridColors[x, y] = Color.white;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        // 左侧地图编辑区域
        GUILayout.BeginVertical();
        DrawGrid();
        GUILayout.EndVertical();

        // 右侧工具区域
        GUILayout.BeginVertical(GUILayout.Width(100));
        DrawTools();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        HandleMouseEvents();
    }

    private void DrawGrid()
    {
        var rect = GUILayoutUtility.GetRect(mapWidth * gridSize, mapHeight * gridSize);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Rect gridRect = new Rect(rect.x + x * gridSize, rect.y + y * gridSize, gridSize, gridSize);
                EditorGUI.DrawRect(gridRect, gridColors[x, y]);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, gridSize, 1), Color.gray);
                EditorGUI.DrawRect(new Rect(gridRect.x, gridRect.y, 1, gridSize), Color.gray);
            }
        }
    }

    private void DrawTools()
    {
        if (GUILayout.Button("绘制"))
        {
            isErasing = false;
        }
        if (GUILayout.Button("消除"))
        {
            isErasing = true;
        }

        selectedColor = EditorGUILayout.ColorField("选择颜色", selectedColor);
    }

    private void HandleMouseEvents()
    {
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        int x = Mathf.FloorToInt(mousePos.x / gridSize);
        int y = Mathf.FloorToInt(mousePos.y / gridSize);

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            isPainting = true;
            if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
            {
                if (!isErasing && gridColors[x, y] == Color.white)
                {
                    ChangeGridColor(x, y, selectedColor);
                    CreateCubeAtPosition(x, y, selectedColor);
                }
                else if (isErasing && gridColors[x, y] != Color.white)
                {
                    ChangeGridColor(x, y, Color.white);
                    DestroyCubeAtPosition(x, y);
                }
            }
            e.Use();  // 标记事件为已使用，防止其他操作
        }
        else if (e.type == EventType.MouseDrag && e.button == 0 && isPainting)
        {
            if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
            {
                if (!isErasing && gridColors[x, y] == Color.white)
                {
                    ChangeGridColor(x, y, selectedColor);
                    CreateCubeAtPosition(x, y, selectedColor);
                }
                else if (isErasing && gridColors[x, y] != Color.white)
                {
                    ChangeGridColor(x, y, Color.white);
                    DestroyCubeAtPosition(x, y);
                }
            }
            e.Use();  // 标记事件为已使用，防止其他操作
        }
        else if (e.type == EventType.MouseUp && e.button == 0)
        {
            isPainting = false;
            e.Use();  // 标记事件为已使用，防止其他操作
        }
    }

    private void ChangeGridColor(int x, int y, Color color)
    {
        gridColors[x, y] = color;
        Repaint(); // 重新绘制窗口
    }

    private void CreateCubeAtPosition(int x, int y, Color color)
    {
        Vector3 position = new Vector3(x * cubeSize, 0, y * cubeSize);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = new Vector3(cubeSize, 1, cubeSize); // 高度默认为1
        cube.GetComponent<Renderer>().sharedMaterial.color = color; // 使用sharedMaterial
        cube.transform.parent = parentObject.transform;
        cube.name = $"Cube_{x}_{y}";
    }

    private void DestroyCubeAtPosition(int x, int y)
    {
        string cubeName = $"Cube_{x}_{y}";
        Transform child = parentObject.transform.Find(cubeName);
        if (child != null)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}