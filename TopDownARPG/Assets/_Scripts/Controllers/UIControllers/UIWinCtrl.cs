using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIWinCtrl : UICtrl
{

    private Image _blackImage; // Black Image を格納するための変数

    private List<Selectable> _winSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables;
    private int _currentSelectionIndex = 0;

    private Button[] buttons; // すべてのボタンを格納する配列

    public override void Awake()
    {
        base.Awake();

        // 初始化登录界面的可选择元素
        _winSelectables.Add(View["Title"].GetComponent<Button>());
        _winSelectables.Add(View["Exit"].GetComponent<Button>());
        _currentSelectables = _winSelectables;

        // 按钮事件和效果
        AddButtonListener("Exit", Exit);
        AddButtonListener("Title", Title);

        // ボタンの配列を初期化
        buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(),
            View["Exit"].GetComponent<Button>()
        };

        AudioManager.Instance.PlayWinBgm();
        AudioManager.Instance.StopAllNonWinBgms();

        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒后隐藏 Black Image
        StartCoroutine(HideBlackImageAfterDelay());

        // すべてのボタンにホバーエフェクトを追加
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }
    }

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

    // ボタンが押されたときのスケール効果を実装するコルーチン
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

    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIWin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    // ボタンにホバーエフェクトを追加する
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }

}