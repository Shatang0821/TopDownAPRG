using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI; // 自定义的UI框架
using FrameWork.Audio; // 自定义的音频框架
using UnityEngine.EventSystems; // 处理UI事件的系统
using System.Collections.Generic;

// UIEndCtrl类继承自UICtrl（自定义的UI控制器类）
public class UIEndCtrl : UICtrl
{
    private Image _blackImage; // 用于显示黑色遮罩的Image组件
    private List<Selectable> _endSelectables = new List<Selectable>(); // 存储所有可选择的UI元素
    private List<Selectable> _currentSelectables; // 当前选择的UI元素列表
    private int _currentSelectionIndex = 0; // 当前选择的索引
    private Button[] buttons; // 存储所有按钮的数组

    // Awake方法在脚本实例加载时调用，初始化UI元素
    public override void Awake()
    {
        base.Awake(); // 调用父类的Awake方法，初始化基本逻辑

        // 初始化结束界面的可选择元素
        _endSelectables.Add(View["Title"].GetComponent<Button>()); // 获取“Title”按钮
        _endSelectables.Add(View["ReStart"].GetComponent<Button>()); // 获取“ReStart”按钮
        _endSelectables.Add(View["Exit"].GetComponent<Button>()); // 获取“Exit”按钮
        _currentSelectables = _endSelectables; // 将可选择元素列表赋值给当前选择元素列表

        // 为按钮添加点击事件
        AddButtonListener("Title", Title); // 为“Title”按钮添加事件
        AddButtonListener("ReStart", ReStart); // 为“ReStart”按钮添加事件
        AddButtonListener("Exit", Exit); // 为“Exit”按钮添加事件

        // 初始化按钮数组
        buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(), // Title按钮
            View["ReStart"].GetComponent<Button>(), // ReStart按钮
            View["Exit"].GetComponent<Button>() // Exit按钮
        };

        // 播放失败场景的背景音乐，并停止所有非失败场景的音乐
        AudioManager.Instance.PlayLoseBgm(); // 播放失败场景BGM
        AudioManager.Instance.StopAllNonLoseBgms(); // 停止其他非失败场景的背景音乐

        _blackImage = View["Black"].GetComponent<Image>(); // 获取黑色遮罩的Image组件

        // 1.8秒后隐藏黑色遮罩
        StartCoroutine(HideBlackImageAfterDelay());

        // 为每个按钮添加悬停效果
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button); // 添加悬停效果
        }
    }

    // 协程：在延迟1.8秒后隐藏黑色遮罩图片
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f); // 等待1.8秒
        _blackImage.gameObject.SetActive(false); // 隐藏黑色遮罩
    }

    // Start方法在脚本启用时调用，默认选择第一个按钮
    void Start()
    {
        _currentSelectables[_currentSelectionIndex].Select(); // 选中第一个可选择元素
    }

    // 按钮按下时缩放的协程
    private IEnumerator ScaleButtonOnPress(Button button)
    {
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerDown(null); // 触发按钮按下效果
            yield return new WaitForSeconds(0.2f); // 等待0.2秒
            hoverEffect.OnPointerUp(null); // 恢复按钮原始状态
        }
    }

    // 执行延迟操作的协程
    private IEnumerator ExecuteWithDelay(System.Action action, Button button)
    {
        yield return StartCoroutine(ScaleButtonOnPress(button)); // 执行按钮缩放效果
        action.Invoke(); // 执行传入的操作
    }

    // 选择上一个可选元素
    private void SelectPreviousInput()
    {
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中效果
            }
        }

        _currentSelectionIndex--; // 向前选择
        if (_currentSelectionIndex < 0)
        {
            _currentSelectionIndex = _currentSelectables.Count - 1; // 循环回到列表最后一个元素
        }

        _currentSelectables[_currentSelectionIndex].Select(); // 选中新的元素

        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动触发选中效果
            }
        }
    }

    // 选择下一个可选元素
    private void SelectNextInput()
    {
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中效果
            }
        }

        _currentSelectionIndex++; // 向后选择
        if (_currentSelectionIndex >= _currentSelectables.Count)
        {
            _currentSelectionIndex = 0; // 循环回到第一个元素
        }

        _currentSelectables[_currentSelectionIndex].Select(); // 选中新的元素

        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动触发选中效果
            }
        }
    }

    // 执行当前选中的按钮的操作
    private void ExecuteCurrentSelection()
    {
        var current = _currentSelectables[_currentSelectionIndex];

        if (current is Button button)
        {
            StartCoroutine(ScaleButtonOnPress(button)); // 启动按钮缩放效果
            StartCoroutine(HandleButtonClick(button)); // 处理按钮点击效果
        }
    }

    // 处理按钮点击后的延迟效果
    private IEnumerator HandleButtonClick(Button button)
    {
        yield return new WaitForSeconds(0.2f); // 等待与Button缩放效果同步的时间
        if (button.name == "Exit")
        {
            Exit(); // 退出游戏
        }
        else
        {
            button.onClick.Invoke(); // 调用按钮点击事件
        }
    }

    // 每帧调用一次，处理输入
    private void Update()
    {
        // 检测方向键选择上一个或下一个UI元素
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SelectPreviousInput();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SelectNextInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCurrentSelection(); // 确认选择
        }
    }

    // “Title”按钮的回调方法，返回主菜单
    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIEnd"); // 移除当前的结束界面
        UIManager.Instance.ShowUI("UIHome"); // 显示主菜单界面
        UIManager.Instance.ChangeUIPrefab("UIHome"); // 切换UI预制体为主菜单

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false); // 隐藏当前游戏对象
        }
    }

    // “ReStart”按钮的回调方法，重新开始游戏
    private void ReStart()
    {
        Debug.Log("ReStart");
        UIManager.Instance.RemoveUI("UIEnd"); // 移除结束界面
        UIManager.Instance.ShowUI("UIGame"); // 显示游戏界面
        UIManager.Instance.ChangeUIPrefab("UIGame"); // 切换UI预制体为游戏界面
        GameManager.Instance.ChangeState(GameState.Gameplay); // 更改游戏状态为进行中

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false); // 隐藏当前游戏对象
        }
    }

    // “Exit”按钮的回调方法，退出游戏
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit(); // 退出应用程序
    }

    // 为按钮添加悬停效果
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale); // 设置按钮的初始缩放值
    }
}
