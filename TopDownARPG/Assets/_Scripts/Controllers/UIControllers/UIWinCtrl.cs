using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIWinCtrl : UICtrl
{
    private Image _blackImage; // 用于存储黑色图像（Black Image）的变量
    private List<Selectable> _winSelectables = new List<Selectable>(); // 存储可选择的UI元素（如按钮）的列表
    private List<Selectable> _currentSelectables; // 当前活动的可选择元素列表
    private int _currentSelectionIndex = 0; // 当前选择的元素索引
    private Button[] buttons; // 用于存储所有按钮的数组

    // 重写Awake方法
    public override void Awake()
    {
        base.Awake();

        // 初始化登录界面的可选择元素，将“Title”和“Exit”按钮添加到_winSelectables列表中
        _winSelectables.Add(View["Title"].GetComponent<Button>());
        _winSelectables.Add(View["Exit"].GetComponent<Button>());
        _currentSelectables = _winSelectables; // 将当前可选择元素设为登录界面的按钮

        // 为“Exit”和“Title”按钮添加点击事件
        AddButtonListener("Exit", Exit);
        AddButtonListener("Title", Title);

        // 初始化按钮数组
        buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(),
            View["Exit"].GetComponent<Button>()
        };

        // 播放胜利背景音乐并停止其他背景音乐
        AudioManager.Instance.PlayWinBgm();
        AudioManager.Instance.StopAllNonWinBgms();

        // 获取黑色图像（Black Image）
        _blackImage = View["Black"].GetComponent<Image>();

        // 启动协程，延迟1.8秒后隐藏黑色图像
        StartCoroutine(HideBlackImageAfterDelay());

        // 为所有按钮添加鼠标悬停效果
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }
    }

    // 隐藏黑色图像的协程，延迟1.8秒
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false); // 隐藏黑色图像
    }

    // Unity的Start方法，初始化时会调用
    void Start()
    {
        // 默认选中登录界面的第一个元素
        _currentSelectables[_currentSelectionIndex].Select();
    }

    // 当按钮被按下时执行缩放效果的协程
    private IEnumerator ScaleButtonOnPress(Button button)
    {
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerDown(null); // 模拟按下效果
            yield return new WaitForSeconds(0.2f); // 等待0.2秒
            hoverEffect.OnPointerUp(null); // 模拟松开效果
        }
    }

    // 延迟执行某个操作的协程，并在此之前触发按钮的缩放效果
    private IEnumerator ExecuteWithDelay(System.Action action, Button button)
    {
        // 先执行按钮按下的缩放效果
        yield return StartCoroutine(ScaleButtonOnPress(button));

        // 缩放效果结束后执行传入的操作
        action.Invoke();

        // 恢复按钮的缩放状态
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerUp(null); // 恢复按钮状态
        }
    }

    // 选择上一个可选择的UI元素
    private void SelectPreviousInput()
    {
        // 如果当前选择的是按钮，取消其选中状态
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中
            }
        }

        // 更新选择索引
        _currentSelectionIndex--;
        if (_currentSelectionIndex < 0)
        {
            _currentSelectionIndex = _currentSelectables.Count - 1;
        }

        // 选择新的UI元素
        _currentSelectables[_currentSelectionIndex].Select();

        // 如果新的选择是按钮，手动选中它
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动选中
            }
        }
    }

    // 选择下一个可选择的UI元素
    private void SelectNextInput()
    {
        // 如果当前选择的是按钮，取消其选中状态
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动取消选中
            }
        }

        // 更新选择索引
        _currentSelectionIndex++;
        if (_currentSelectionIndex >= _currentSelectables.Count)
        {
            _currentSelectionIndex = 0;
        }

        // 选择新的UI元素
        _currentSelectables[_currentSelectionIndex].Select();

        // 如果新的选择是按钮，手动选中它
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动选中
            }
        }
    }

    // 执行当前选中的UI元素的操作
    private void ExecuteCurrentSelection()
    {
        var current = _currentSelectables[_currentSelectionIndex];

        if (current is Button)
        {
            Button button = current as Button;

            // 启动缩放效果
            StartCoroutine(ScaleButtonOnPress(button));

            // 等待特效完成后再执行按钮的点击事件
            StartCoroutine(HandleButtonClick(button));
        }
    }

    // 处理按钮点击后的延迟效果
    private IEnumerator HandleButtonClick(Button button)
    {
        // 等待与缩放效果一致的时间
        yield return new WaitForSeconds(0.2f);

        // 根据按钮名称执行不同操作
        if (button.name == "Exit")
        {
            Exit(); // 退出游戏
        }
        else
        {
            button.onClick.Invoke(); // 触发按钮的点击事件
        }
    }

    // 每帧更新，用于检测用户输入
    private void Update()
    {
        // 检测方向键选择上一个或下一个UI元素
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W))
        {
            SelectPreviousInput();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))
        {
            SelectNextInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCurrentSelection(); // 确认选择
        }
    }

    // 处理标题按钮点击的函数
    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIWin"); // 移除胜利界面
        UIManager.Instance.ShowUI("UIHome"); // 显示主界面
        UIManager.Instance.ChangeUIPrefab("UIHome"); // 更换UI预制体

        // 如果当前对象不为空，将其禁用
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    // 处理退出按钮点击的函数
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit(); // 退出应用程序
    }

    // 为按钮添加鼠标悬停效果
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale); // 设置按钮的原始缩放比例
    }
}
