using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;

public class UIWinCtrl : UICtrl
{
    private Image _blackImage; // Black Image を格納するための変数
    private Button[] _buttons; // 按钮数组
    private int _currentButtonIndex = 0; // 当前选中的按钮索引
    private ButtonHoverEffect _currentHoverEffect; // 当前按钮的 HoverEffect

    public override void Awake()
    {
        base.Awake();

        // 按钮事件和效果
        AddButtonListener("Exit", Exit);
        AddButtonListener("Title", Title);

        // 初始化按钮数组
        _buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(),
            View["Exit"].GetComponent<Button>()
        };

        // 确保按钮已正确添加
        Debug.Log("Title Button: " + _buttons[0]);
        Debug.Log("Exit Button: " + _buttons[1]);

        // 为每个按钮添加特效
        foreach (Button button in _buttons)
        {
            AddButtonHoverEffect(button);
            Debug.Log("ButtonHoverEffect added to: " + button.name);
        }

        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒后隐藏 Black Image
        StartCoroutine(HideBlackImageAfterDelay());

        AudioManager.Instance.PlayWinBgm();
        AudioManager.Instance.StopAllNonWinBgms();

    }

    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
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

    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
        Debug.Log("HoverEffect added to: " + button.name);
    }

    private void Update()
    {
        // 向左箭头：选择上一个按钮
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousButton();
        }
        // 向右箭头：选择下一个按钮
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextButton();
        }
        // 回车键按下：模拟按钮按下效果
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 手动触发按下效果，缩小按钮
            ExecuteEvents.Execute(_buttons[_currentButtonIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            _currentHoverEffect.OnPointerDown(null); // 手动调用 OnPointerDown 以缩小按钮
        }
        // 回车键松开：模拟按钮松开效果
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            // 手动触发松开效果，并执行点击事件
            ExecuteEvents.Execute(_buttons[_currentButtonIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
            _currentHoverEffect.OnPointerUp(null); // 手动调用 OnPointerUp 以恢复按钮大小
            _buttons[_currentButtonIndex].onClick.Invoke(); // 调用按钮的点击事件
        }
    }

    // 选择上一个按钮
    private void SelectPreviousButton()
    {
        int previousIndex = _currentButtonIndex;
        _currentButtonIndex--;
        if (_currentButtonIndex < 0)
        {
            _currentButtonIndex = _buttons.Length - 1;
        }
        SelectButton(previousIndex, _currentButtonIndex);
    }

    // 选择下一个按钮
    private void SelectNextButton()
    {
        int previousIndex = _currentButtonIndex;
        _currentButtonIndex++;
        if (_currentButtonIndex >= _buttons.Length)
        {
            _currentButtonIndex = 0;
        }
        SelectButton(previousIndex, _currentButtonIndex);
    }

    // 选中指定索引的按钮并触发动画效果
    private void SelectButton(int previousIndex, int newIndex)
    {
        // 取消之前按钮的选中状态
        if (_currentHoverEffect != null)
        {
            _currentHoverEffect.OnDeselect(null); // 手动取消选中时调用OnDeselect
        }

        // 更新当前选中的按钮
        _currentHoverEffect = _buttons[newIndex].GetComponent<ButtonHoverEffect>();
        _buttons[newIndex].Select();
        _currentHoverEffect.OnSelect(null); // 手动选中时调用OnSelect
    }
}
