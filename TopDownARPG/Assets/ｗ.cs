/*
using FrameWork.Audio;
using FrameWork.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ｗ : MonoBehaviour
{
    private Image _blackImage; // Black Image を格納するための変数

    private List<Selectable> _endSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables;
    private int _currentSelectionIndex = 0;

    private Button[] buttons; // すべてのボタンを格納する配列

    // Awake方法在脚本实例加载时调用
    public override void Awake()
    {
        base.Awake(); // 调用父类的Awake方法

        // 初始化登录界面的可选择元素
        _endSelectables.Add(View["Title"].GetComponent<Button>());
        _endSelectables.Add(View["Exit"].GetComponent<Button>());
        _endSelectables.Add(View["ReStart"].GetComponent<Button>());
        _currentSelectables = _endSelectables;

        // 按钮事件和效果
        AddButtonListener("Title", Title);
        AddButtonListener("Exit", Exit);
        AddButtonListener("ReStart", ReStart);

        // ボタンの配列を初期化
        buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(),
            View["ReStart"].GetComponent<Button>()
            View["Exit"].GetComponent<Button>()
        };

        // 播放失败场景的背景音乐，并停止所有非失败场景的音乐
        AudioManager.Instance.PlayLoseBgm();
        AudioManager.Instance.StopAllNonLoseBgms();

        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒后隐藏 Black Image
        StartCoroutine(HideBlackImageAfterDelay());

        // すべてのボタンにホバーエフェクトを追加
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }
    }

    // 协程：在延迟1.8秒后隐藏黑色遮罩图片
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }

    void Start()
    {
        // 默认选中登录界面的第一个元素
        _currentSelectables[_currentSelectionIndex].Select();
    }

    private IEnumerator ScaleButtonOnPress(Button button)
    {
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerDown(null);
            yield return new WaitForSeconds(0.2f);
            hoverEffect.OnPointerUp(null);
        }
    }

    private IEnumerator ExecuteWithDelay(System.Action action, Button button)
    {
        // 触发按钮的缩放效果
        yield return StartCoroutine(ScaleButtonOnPress(button));

        // 在缩放效果完成后执行传入的action
        action.Invoke();

        // 恢复按钮的缩放状态
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerUp(null); // 恢复按钮状态
        }
    }
    private void SelectPreviousInput()
    {
        // 手动取消当前选择的元素（如果是按钮）
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动调用 Deselect
            }
        }

        _currentSelectionIndex--;
        if (_currentSelectionIndex < 0)
        {
            _currentSelectionIndex = _currentSelectables.Count - 1;
        }

        _currentSelectables[_currentSelectionIndex].Select();

        // 手动选中新的元素（如果是按钮）
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动调用 Select
            }
        }
    }

    private void SelectNextInput()
    {
        // 手动取消当前选择的元素（如果是按钮）
        var current = _currentSelectables[_currentSelectionIndex];
        if (current is Button button)
        {
            var hoverEffect = button.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnDeselect(null); // 手动调用 Deselect
            }
        }

        _currentSelectionIndex++;
        if (_currentSelectionIndex >= _currentSelectables.Count)
        {
            _currentSelectionIndex = 0;
        }

        _currentSelectables[_currentSelectionIndex].Select();

        // 手动选中新的元素（如果是按钮）
        var newCurrent = _currentSelectables[_currentSelectionIndex];
        if (newCurrent is Button newButton)
        {
            var hoverEffect = newButton.GetComponent<ButtonHoverEffect>();
            if (hoverEffect != null)
            {
                hoverEffect.OnSelect(null); // 手动调用 Select
            }
        }
    }

    private void ExecuteCurrentSelection()
    {
        var current = _currentSelectables[_currentSelectionIndex];

        if (current is Button)
        {
            Button button = current as Button;

            // 启动缩放效果
            StartCoroutine(ScaleButtonOnPress(button));

            // 等待特效完成后再执行跳转效果
            StartCoroutine(HandleButtonClick(button));
        }
    }

    // 处理按钮点击后的延迟效果
    private IEnumerator HandleButtonClick(Button button)
    {
        // 等待与 ButtonHoverEffect 中的 ScaleTo 一样的时间
        yield return new WaitForSeconds(0.2f); // 调整此时间与 ScaleTo 方法的延迟一致

        if (button.name == "Exit")
        {
            Exit();
        }
        else
        {
            button.onClick.Invoke();
        }
    }

    private void Update()
    {
        // 检测上下方向键来选择输入框或按钮
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousInput();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCurrentSelection();
        }
    }

    // “Title”按钮的回调方法，返回主菜单
    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIEnd"); // 移除当前的“UIEnd”界面
        UIManager.Instance.ShowUI("UIHome"); // 显示主菜单界面“UIHome”
        UIManager.Instance.ChangeUIPrefab("UIHome"); // 切换UI预制体为“UIHome”

        // 隐藏当前游戏对象
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    // “ReStart”按钮的回调方法，重新开始游戏
    private void ReStart()
    {
        Debug.Log("ReStart");
        UIManager.Instance.RemoveUI("UIEnd"); // 移除当前的“UIEnd”界面
        UIManager.Instance.ShowUI("UIGame"); // 显示游戏界面“UIGame”
        UIManager.Instance.ChangeUIPrefab("UIGame"); // 切换UI预制体为“UIGame”

        // 改变游戏状态为游戏进行中
        GameManager.Instance.ChangeState(GameState.Gameplay);

        // 隐藏当前游戏对象
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    // “Exit”按钮的回调方法，退出游戏
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit(); // 退出应用程序
    }

    // ボタンにホバーエフェクトを追加する
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }
}
*/