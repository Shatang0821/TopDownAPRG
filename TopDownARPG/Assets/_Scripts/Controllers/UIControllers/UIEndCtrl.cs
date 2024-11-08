using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI; // 自定义的UI框架
using FrameWork.Audio; // 自定义的音频框架
using UnityEngine.EventSystems; // 处理UI事件的系统

// UIEndCtrl类继承自UICtrl（自定义的UI控制器类）
public class UIEndCtrl : UICtrl
{
    // 私有变量，用来存储UI组件
    private Image _blackImage; // 用于存储黑色遮罩图片的引用
    private Button[] _buttons; // 按钮数组，用于存储多个按钮的引用
    private int _currentButtonIndex = 0; // 当前选中的按钮索引
    private ButtonHoverEffect _currentHoverEffect; // 当前选中按钮的悬停特效

    // Awake方法在脚本实例加载时调用
    public override void Awake()
    {
        base.Awake(); // 调用父类的Awake方法

        // 为按钮“Title”、“Exit”和“ReStart”添加事件监听器
        AddButtonListener("Title", Title); // 为“Title”按钮添加点击事件
        AddButtonListener("Exit", Exit); // 为“Exit”按钮添加点击事件
        AddButtonListener("ReStart", ReStart); // 为“ReStart”按钮添加点击事件

        // 初始化按钮数组，并获取相应的按钮组件
        _buttons = new Button[]
        {
            View["Title"].GetComponent<Button>(), // 获取“Title”按钮的Button组件
            View["ReStart"].GetComponent<Button>(), // 获取“ReStart”按钮的Button组件
            View["Exit"].GetComponent<Button>() // 获取“Exit”按钮的Button组件
        };

        // 为每个按钮添加悬停特效
        foreach (var button in _buttons)
        {
            AddButtonHoverEffect(button); // 调用自定义方法为每个按钮添加悬停效果
        }

        // 获取黑色遮罩图片（Black Image）
        _blackImage = View["Black"].GetComponent<Image>();

        // 播放失败场景的背景音乐，并停止所有非失败场景的音乐
        AudioManager.Instance.PlayLoseBgm();
        AudioManager.Instance.StopAllNonLoseBgms();

        // 启动协程，在1.8秒后隐藏黑色遮罩图片
        StartCoroutine(HideBlackImageAfterDelay());
    }

    // 协程：在延迟1.8秒后隐藏黑色遮罩图片
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f); // 等待1.8秒
        _blackImage.gameObject.SetActive(false); // 隐藏黑色遮罩图片
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

    // “Exit”按钮的回调方法，退出游戏
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit(); // 退出应用程序
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

    // 为指定的按钮添加悬停特效
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>(); // 给按钮添加悬停效果组件
        hoverEffect.SetOriginalScale(button.transform.localScale); // 设置按钮的原始缩放值，用于悬停效果
    }

    // Update方法在每一帧都会调用，处理用户输入
    private void Update()
    {
        // 按向上或向左键时选择上一个按钮
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousButton(); // 选择上一个按钮
        }
        // 按向下或向右键时选择下一个按钮
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextButton(); // 选择下一个按钮
        }
        // 按下回车键时，模拟按钮按下效果
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteEvents.Execute(_buttons[_currentButtonIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler); // 模拟按钮按下事件
            _currentHoverEffect.OnPointerDown(null); // 手动调用按钮的OnPointerDown方法，缩小按钮
        }
        // 松开回车键时，模拟按钮松开效果
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            ExecuteEvents.Execute(_buttons[_currentButtonIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler); // 模拟按钮松开事件
            _currentHoverEffect.OnPointerUp(null); // 手动调用按钮的OnPointerUp方法，恢复按钮大小
            _buttons[_currentButtonIndex].onClick.Invoke(); // 调用当前按钮的点击事件
        }
    }

    // 选择上一个按钮
    private void SelectPreviousButton()
    {
        int previousIndex = _currentButtonIndex; // 记录当前的按钮索引
        _currentButtonIndex--; // 索引减1，移动到上一个按钮
        if (_currentButtonIndex < 0)
        {
            _currentButtonIndex = _buttons.Length - 1; // 如果越界，则回到最后一个按钮
        }
        SelectButton(previousIndex, _currentButtonIndex); // 更新选中按钮
    }

    // 选择下一个按钮
    private void SelectNextButton()
    {
        int previousIndex = _currentButtonIndex; // 记录当前的按钮索引
        _currentButtonIndex++; // 索引加1，移动到下一个按钮
        if (_currentButtonIndex >= _buttons.Length)
        {
            _currentButtonIndex = 0; // 如果越界，则回到第一个按钮
        }
        SelectButton(previousIndex, _currentButtonIndex); // 更新选中按钮
    }

    // 选中指定索引的按钮并触发动画效果
    private void SelectButton(int previousIndex, int newIndex)
    {
        // 取消之前按钮的选中状态
        if (_currentHoverEffect != null)
        {
            _currentHoverEffect.OnDeselect(null); // 手动调用OnDeselect取消选中状态
        }

        // 更新当前选中的按钮
        _currentButtonIndex = newIndex; // 更新当前按钮索引
        _currentHoverEffect = _buttons[newIndex].GetComponent<ButtonHoverEffect>(); // 获取新的悬停效果组件
        _buttons[newIndex].Select(); // 选中新的按钮
        _currentHoverEffect.OnSelect(null); // 手动调用OnSelect选中效果
    }
}
