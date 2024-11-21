using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualKeyboard : MonoBehaviour
{
    public GameObject keyboardPanel; // 键盘面板
    public TMP_InputField[] inputFields; // 存储所有输入框
    private TMP_InputField activeInputField; // 当前激活的输入框

    private Button[][] keyboardButtons; // 二维数组表示键盘布局
    private int rowIndex = 0, colIndex = 0; // 当前选中键的位置（行和列）
    private bool isKeyBeingClicked = false;  // 防止回车键和按钮点击事件重复触发

    private void Start()
    {
        InitializeKeyboard();

        // 为每个输入框绑定焦点事件
        foreach (var field in inputFields)
        {
            field.onSelect.AddListener(delegate { SetActiveInputField(field); });
        }
    }

    private void InitializeKeyboard()
    {
        // 从 keyboardPanel 中获取所有 Button 组件
        Button[] buttons = keyboardPanel.GetComponentsInChildren<Button>();

        // 根据图片中的布局，定义 4 行不同列数的二维数组
        keyboardButtons = new Button[][]
        {
            new Button[10], // 第一行，10个数字按钮
            new Button[10], // 第二行，10个字母按钮
            new Button[9],  // 第三行，9个字母按钮
            new Button[8]   // 第四行，8个字母按钮
        };

        // 如果按钮数量不足，直接报错
        int expectedButtonCount = 37; // 理论上需要 37 个按钮（10+10+9+8）
        if (buttons.Length < expectedButtonCount)
        {
            Debug.LogError($"Button count mismatch! Found {buttons.Length}, but expected {expectedButtonCount}.");
            return;
        }

        int buttonIndex = 0;
        for (int i = 0; i < keyboardButtons.Length; i++)
        {
            for (int j = 0; j < keyboardButtons[i].Length; j++)
            {
                if (buttonIndex < buttons.Length)
                {
                    keyboardButtons[i][j] = buttons[buttonIndex];
                    int capturedIndex = buttonIndex; // 捕获当前按钮索引以避免闭包问题
                    buttons[buttonIndex].onClick.AddListener(() => TypeCharacter(buttons[capturedIndex]));
                    buttonIndex++;
                }
                else
                {
                    Debug.LogError($"Not enough buttons for keyboard layout at row {i}, col {j}.");
                    return;
                }
            }
        }
    }

    private void Update()
    {
        // 判断回车键是否按下
        if (Input.GetKeyDown(KeyCode.Return) && !isKeyBeingClicked)
        {
            isKeyBeingClicked = true;  // 标记回车键已按下，防止重复触发

            // 检查是否有一个按钮被选中
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
            {
                // 防止重复触发，通过按钮的点击事件处理输入
                return;
            }

            // 如果不是按钮触发，则手动输入字符
            TypeCharacter(keyboardButtons[rowIndex][colIndex]);
        }

        // 如果回车键被释放，重置标志位
        if (Input.GetKeyUp(KeyCode.Return))
        {
            isKeyBeingClicked = false;
        }

        // 方向键导航，仅在键盘面板激活时生效
        if (keyboardPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Navigate(-1, 0);
            if (Input.GetKeyDown(KeyCode.DownArrow)) Navigate(1, 0);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Navigate(0, -1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) Navigate(0, 1);
        }
    }

    private void Navigate(int rowChange, int colChange)
    {
        // 取消当前选中按钮的视觉效果
        DeselectButton(rowIndex, colIndex);

        // 更新行索引
        int newRowIndex = Mathf.Clamp(rowIndex + rowChange, 0, keyboardButtons.Length - 1);

        // 更新列索引，确保在新行的列范围内
        int newColIndex = Mathf.Clamp(colIndex + colChange, 0, keyboardButtons[newRowIndex].Length - 1);

        // 更新索引
        rowIndex = newRowIndex;
        colIndex = newColIndex;

        // 选中新位置的按钮
        SelectButton(rowIndex, colIndex);
    }

    private void SelectButton(int row, int col)
    {
        var button = keyboardButtons[row][col];
        if (button != null)
        {
            button.Select(); // 设置按钮的选中状态
            // 可添加其他视觉效果，如改变按钮背景色等
        }
        else
        {
            Debug.LogError($"Button at row {row}, col {col} is null.");
        }
    }

    private void DeselectButton(int row, int col)
    {
        var button = keyboardButtons[row][col];
        if (button != null)
        {
            // 移除按钮的视觉效果（如还原背景色）
        }
    }

    private void TypeCharacter(Button button)
    {
        if (activeInputField == null)
        {
            Debug.LogWarning("No active InputField is selected.");
            return;
        }

        var textComponent = button.GetComponentInChildren<Text>();
        if (textComponent == null)
        {
            Debug.LogError($"Button {button.name} does not have a Text component.");
            return;
        }

        string character = textComponent.text;
        if (character == "Delete")
        {
            if (activeInputField.text.Length > 0)
            {
                activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1); // 删除最后一个字符
            }
        }
        else
        {
            activeInputField.text += character; // 向当前激活的输入框添加字符
        }
        activeInputField.caretPosition = activeInputField.text.Length; // 移动光标到末尾
    }

    public void SetActiveInputField(TMP_InputField input)
    {
        activeInputField = input;
    }
}
