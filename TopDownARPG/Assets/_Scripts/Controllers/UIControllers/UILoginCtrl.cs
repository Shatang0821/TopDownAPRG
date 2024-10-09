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
        _registerSelectables.Add(View["RegistrationScreenPanel/Back"].GetComponent<Button>());

        // 添加按钮事件
        AddButtonListener("Register", Register);
        AddButtonListener("SingIn", SingIn);
        AddButtonListener("RegistrationScreenPanel/Complete", Complete);
        AddButtonListener("RegistrationScreenPanel/Back", Back);

        // 隐藏注册界面
        View["RegistrationScreenPanel"].SetActive(false);

        // 添加按钮悬停效果
        AddButtonHoverEffect("Register");
        AddButtonHoverEffect("SingIn");
        AddButtonHoverEffect("RegistrationScreenPanel/Complete");
        AddButtonHoverEffect("RegistrationScreenPanel/Back");

        // 播放背景音乐
        if (!AudioManager.Instance.IsBgmPlaying())
        {
            AudioManager.Instance.PlayBgmPlayer();
        }
        AudioManager.Instance.StopAllNonBgmPlayers();
    }

    void Start()
    {
        // 默认选中登录界面的第一个元素
        _currentSelectables[_currentSelectionIndex].Select();
    }

    private void SelectPreviousInput()
    {
        _currentSelectionIndex--;
        if (_currentSelectionIndex < 0)
        {
            _currentSelectionIndex = _currentSelectables.Count - 1;
        }
        _currentSelectables[_currentSelectionIndex].Select();
    }

    private void SelectNextInput()
    {
        _currentSelectionIndex++;
        if (_currentSelectionIndex >= _currentSelectables.Count)
        {
            _currentSelectionIndex = 0;
        }
        _currentSelectables[_currentSelectionIndex].Select();
    }

    private void ExecuteCurrentSelection()
    {
        var current = _currentSelectables[_currentSelectionIndex];

        if (current is Button)
        {
            Button button = current as Button;

            if (button.name == "Register")
            {
                Register();
            }
            else
            {
                button.onClick.Invoke();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (View["RegistrationScreenPanel"].activeSelf)
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

        // 检测回车键来执行当前选择的按钮
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCurrentSelection();
        }
    }

    private void Register()
    {
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

        // 确保切换到注册界面后不再触发登录操作
        StopAllCoroutines();
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

    private void Back()
    {
        if (View["RegistrationScreenPanel"].activeSelf)
        {
            View["RegistrationScreenPanel"].SetActive(false);
            _currentSelectables = _loginSelectables;
            _currentSelectionIndex = 0;

            // 手动设置选中对象
            GameObject defaultButton = _currentSelectables[_currentSelectionIndex].gameObject;
            EventSystem.current.SetSelectedGameObject(defaultButton);

            // 确保放大效果被触发
            Button selectedButton = defaultButton.GetComponent<Button>();
            if (selectedButton != null)
            {
                selectedButton.OnSelect(null);  // 手动触发选择事件
            }
        }
        Debug.Log("Back");
    }




    private void AddButtonHoverEffect(string buttonName)
    {
        Button button = View[buttonName].GetComponent<Button>();
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
