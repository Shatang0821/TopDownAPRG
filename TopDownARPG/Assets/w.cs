using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using TMPro;
using System.Collections.Generic;

public class UIHomeCtrl : UICtrl
{
    private Slider BgmSlider;
    private Slider GseSlider;
    private Text BGMPercentage;
    private Text GSEPercentage;
    private Button _gameStart;
    private Button[] buttons;
    private API _api;

    // 可选择的UI元素
    private List<Selectable> _homeSelectables = new List<Selectable>();
    private List<Selectable> _settingsSelectables = new List<Selectable>();
    private List<Selectable> _operationSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables;

    // Awake方法初始化UI元素
    public override void Awake()
    {
        base.Awake();
        _api = FindObjectOfType<API>();

        // 初始化主页可选择元素
        _homeSelectables.Add(View["GameStart"].GetComponent<Button>());
        _homeSelectables.Add(View["Settings"].GetComponent<Button>());
        _homeSelectables.Add(View["Operation"].GetComponent<Button>());
        _homeSelectables.Add(View["Exit"].GetComponent<Button>());
        _homeSelectables.Add(View["Logout"].GetComponent<Button>());
        _currentSelectables = _homeSelectables;

        // 初始化设置界面可选择元素
        _settingsSelectables.Add(View["SettingsPanel/BGMSlider"].GetComponent<Slider>());
        _settingsSelectables.Add(View["SettingsPanel/GSESlider"].GetComponent<Slider>());
        _settingsSelectables.Add(View["SettingsPanel/SBack"].GetComponent<Button>());

        // 初始化操作界面可选择元素
        _operationSelectables.Add(View["OperationPanel/OBack"].GetComponent<Button>());

        // 添加按钮事件
        AddButtonListener("GameStart", GameStart);
        AddButtonListener("Settings", OpenSettings);
        AddButtonListener("Operation", OpenOperation);
        AddButtonListener("Exit", Exit);
        AddButtonListener("Logout", Logout);
        AddButtonListener("SettingsPanel/SBack", BackToHome);
        AddButtonListener("OperationPanel/OBack", BackToHome);

        // 初始化滑块
        BgmSlider = View["SettingsPanel/BGMSlider"].GetComponent<Slider>();
        GseSlider = View["SettingsPanel/GSESlider"].GetComponent<Slider>();
        BGMPercentage = View["SettingsPanel/BGMPercentage"].GetComponent<Text>();
        GSEPercentage = View["SettingsPanel/GSEPercentage"].GetComponent<Text>();

        // 隐藏面板
        View["SettingsPanel"].SetActive(false);
        View["OperationPanel"].SetActive(false);

        // 初始化按钮数组
        buttons = new Button[]
        {
            View["GameStart"].GetComponent<Button>(),
            View["Settings"].GetComponent<Button>(),
            View["Operation"].GetComponent<Button>(),
            View["Exit"].GetComponent<Button>(),
            View["Logout"].GetComponent<Button>(),
            View["SettingsPanel/SBack"].GetComponent<Button>(),
            View["OperationPanel/OBack"].GetComponent<Button>()
        };

        // 播放背景音乐
        if (!AudioManager.Instance.IsBgmPlaying())
        {
            AudioManager.Instance.PlayBgmPlayer();
        }
        AudioManager.Instance.StopAllNonBgmPlayers();

        // 为所有按钮添加悬停效果
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }
    }

    // 返回主页
    private void BackToHome()
    {
        View["SettingsPanel"].SetActive(false);
        View["OperationPanel"].SetActive(false);
        _currentSelectables = _homeSelectables;
    }

    // 游戏开始逻辑
    private void GameStart()
    {
        Debug.Log("GameStart");
        UIManager.Instance.RemoveUI("UIHome");
        UIManager.Instance.ShowUI("UIGame");
    }

    // 打开设置界面
    private void OpenSettings()
    {
        Debug.Log("Settings");
        View["SettingsPanel"].SetActive(true);
        _currentSelectables = _settingsSelectables;
    }

    // 打开操作说明界面
    private void OpenOperation()
    {
        Debug.Log("Operation");
        View["OperationPanel"].SetActive(true);
        _currentSelectables = _operationSelectables;
    }

    // 退出游戏
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    // 登出逻辑
    private void Logout()
    {
        Debug.Log("Logout");
        _api.Logout();
        UIManager.Instance.RemoveUI("UIHome");
        UIManager.Instance.ShowUI("UILogin");
    }

    // 添加按钮悬停效果
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }

    // 更新音量显示
    private void UpdateVolumeDisplay()
    {
        BGMPercentage.text = $"{(BgmSlider.value * 100f):F0}%";
        GSEPercentage.text = $"{(GseSlider.value * 100f):F0}%";
    }
}
