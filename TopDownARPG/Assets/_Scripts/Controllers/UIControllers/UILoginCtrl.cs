using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using TMPro;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class UILoginCtrl : UICtrl
{
    API _api;
    private List<Selectable> _loginSelectables = new List<Selectable>();
    private List<Selectable> _registerSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables;
    private int _currentSelectionIndex = 0;

    private Button[] buttons; // すべてのボタンを格納する配列

    public override void Awake()
    {
        base.Awake();

        _api = FindObjectOfType<API>();
        string filePath = Application.persistentDataPath + "/LoginAccount.json";
        AccountManager accountmanager = new AccountManager(); // accountmanager を初期化

        if (File.Exists(filePath))
        {
            // ファイルが存在する場合はロードして追加
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);
            var accountname = View["Account"].GetComponent<TMP_InputField>();
            var password = View["Password"].GetComponent<TMP_InputField>();
            accountname.text = accountmanager.accountname;
            password.text = accountmanager.password;
        }

        // 初始化登录界面的可选择元素
        _loginSelectables.Add(View["Account"].GetComponent<TMP_InputField>());
        _loginSelectables.Add(View["Password"].GetComponent<TMP_InputField>());
        _loginSelectables.Add(View["Register"].GetComponent<Button>());
        _loginSelectables.Add(View["SingIn"].GetComponent<Button>());
        _currentSelectables = _loginSelectables;

        // 初始化注册界面的可选择元素
        _registerSelectables.Add(View["RegistrationScreenPanel/Account"].GetComponent<TMP_InputField>());
        _registerSelectables.Add(View["RegistrationScreenPanel/Password (1)"].GetComponent<TMP_InputField>());
        _registerSelectables.Add(View["RegistrationScreenPanel/Password (2)"].GetComponent<TMP_InputField>());
        _registerSelectables.Add(View["RegistrationScreenPanel/Complete"].GetComponent<Button>());

        // 添加按钮事件
        AddButtonListener("Register", Register);
        AddButtonListener("SingIn", SingIn);
        AddButtonListener("RegistrationScreenPanel/Complete", Complete);

        // 隐藏注册界面
        View["RegistrationScreenPanel"].SetActive(false);

        // ボタンの配列を初期化
        buttons = new Button[]
        {
        View["Register"].GetComponent<Button>(),
        View["SingIn"].GetComponent<Button>(),
        View["RegistrationScreenPanel/Complete"].GetComponent<Button>(),
        };

        // 播放背景音乐
        if (!AudioManager.Instance.IsBgmPlaying())
        {
            AudioManager.Instance.PlayBgmPlayer();
        }
        AudioManager.Instance.StopAllNonBgmPlayers();

        // すべてのボタンにホバーエフェクトを追加
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }
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

        if (button.name == "Register")
        {
            Register();
        }
        else
        {
            button.onClick.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 检查键盘面板是否激活
            if (View["KeyboardPanel"].activeSelf)
            {
                // 隐藏键盘面板
                View["KeyboardPanel"].SetActive(false);
            }
            else if (View["RegistrationScreenPanel"].activeSelf)
            {
                View["RegistrationScreenPanel"].SetActive(false);
                _currentSelectables = _loginSelectables;  // 切换回登录界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }
        }

        // 检测上下方向键来选择输入框或按钮
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectPreviousInput();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectNextInput();
        }

        // 检测回车键来执行当前选择的按钮或唤出键盘面板
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var current = _currentSelectables[_currentSelectionIndex];
            if (current is TMP_InputField)
            {
                // 如果当前选择的是输入框，按下回车键则唤出键盘面板
                ShowKeyboardPanel();
            }
            else
            {
                ExecuteCurrentSelection();
            }
        }
    }


    private void ShowKeyboardPanel()
    {
        // 显示键盘面板
        View["KeyboardPanel"].SetActive(true);

        // 如果需要，你可以在键盘面板唤出后自动聚焦某个按键
        // View["KeyboardPanel/SomeKey"].GetComponent<Button>().Select();
    }

    private void Register()
    {
        StartCoroutine(ExecuteWithDelay(() => {
            Debug.Log("Register");
            bool currentStatus = View["RegistrationScreenPanel"].activeSelf;
            View["RegistrationScreenPanel"].SetActive(!currentStatus);

            if (!currentStatus)
            {
                _currentSelectables = _registerSelectables;  // 切换到注册界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }
            else
            {
                _currentSelectables = _loginSelectables;  // 切换回登录界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }

            _currentSelectables[_currentSelectionIndex].Select();  // 选中第一个元素
            StopAllCoroutines();
        }, View["Register"].GetComponent<Button>()));
    }

    private void SingIn()
    {
        var accountname = View["Account"].GetComponent<TMP_InputField>();
        var password = View["Password"].GetComponent<TMP_InputField>();
        StartCoroutine(_api.Login(accountname, password));
        StartCoroutine(SignIn());

        if (_api.isLogin)
        {
            StartCoroutine(SignIn());
        }
    }

    private void Complete()
    {
        var accountname = View["RegistrationScreenPanel/Account"].GetComponent<TMP_InputField>();
        var password = View["RegistrationScreenPanel/Password (1)"].GetComponent<TMP_InputField>();
        StartCoroutine(_api.CreateAccount(accountname, password));
        Debug.Log("Complete");

        if (!_api.isBadrequest)
        {
            StartCoroutine(SignIn());
        }
    }

    // ボタンにホバーエフェクトを追加する
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }

    IEnumerator SignIn()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("SingIn");
        UIManager.Instance.RemoveUI("UILogin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
