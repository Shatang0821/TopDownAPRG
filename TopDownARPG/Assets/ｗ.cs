/*
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIHomeCtrl : UICtrl
{
    // UI関連のコンポーネントを格納するための変数
    public Slider BgmSlider; // BGMの音量を調整するためのスライダー
    public Slider GseSlider; // 効果音の音量を調整するためのスライダー
    public Text BGMPercentage; // BGMの音量を表示するテキスト
    public Text GSEPercentage; // 効果音の音量を表示するテキスト
    public Text BGM; // BGMのラベルを表示するテキスト
    public Text GSE; // 効果音のラベルを表示するテキスト

    private bool panelDelayInProgress = false; // パネルの遅延処理が進行中かどうかを示すフラグ

    private Image _blackImage; // 用于存储黑色图像（Black Image）的变量
    private List<Selectable> _homeSelectables = new List<Selectable>(); // 存储可选择的UI元素（如按钮）的列表
    private List<Selectable> _setSelectables = new List<Selectable>();
    private List<Selectable> _upSelectables = new List<Selectable>();
    private List<Selectable> _currentSelectables; // 当前活动的可选择元素列表
    private int _currentSelectionIndex = 0; // 当前选择的元素索引
    private Button[] buttons; // 用于存储所有按钮的数组

    private enum SelectedElement { Button, BgmSlider, GseSlider }
    private SelectedElement selectedElement = SelectedElement.Button; // 初期状態ではボタンが選択されている

    private API _api;

    // Awakeメソッドをオーバーライドして、UIコンポーネントを初期化する
    public override void Awake()
    {
        base.Awake();

        _api = FindObjectOfType<API>();

        // 初始化登录界面的可选择元素
        _homeSelectables.Add(View["GameStart"].GetComponent<Button>());
        _homeSelectables.Add(View["Settings"].GetComponent<Button>());
        _homeSelectables.Add(View["Operation"].GetComponent<Button>());
        _homeSelectables.Add(View["UpDate"].GetComponent<Button>());
        _homeSelectables.Add(View["Exit"].GetComponent<Button>());
        _homeSelectables.Add(View["Logout"].GetComponent<Button>());
        _currentSelectables = _homeSelectables;

        _setSelectables.Add(View["SettingsPanel/BGMSlider"].GetComponent<Slider>());
        _setSelectables.Add(View["SettingsPanel/GSESlider"].GetComponent<Slider>());

        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/HEALTH/HEALTH"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/ATTACK/ATTACK"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/DEFENSE/DEFENSE"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/SPEED/SPEED"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/MP/MP"].GetComponent<Button>());
        _setSelectables.Add(View["UIUpDate/UpDatePanel/status/DASH/DASH"].GetComponent<Button>());

        // 添加按钮事件
        AddButtonListener("GameStart", GameStart);
        AddButtonListener("Settings", Settings);
        AddButtonListener("Operation", Operation);
        AddButtonListener("UpDate", UpDate);
        AddButtonListener("Exit", Exit);
        AddButtonListener("Logout", Logout);

        AddButtonListener("UIUpDate/UpDatePanel/status/HEALTH/HEALTH", HEALTH);
        AddButtonListener("UIUpDate/UpDatePanel/status/ATTACK/ATTACK", ATTACK);
        AddButtonListener("UIUpDate/UpDatePanel/status/DEFENSE/DEFENSE", DEFENSE);
        AddButtonListener("UIUpDate/UpDatePanel/status/SPEED/SPEED", SPEED);
        AddButtonListener("UIUpDate/UpDatePanel/status/MP/MP", MP);
        AddButtonListener("UIUpDate/UpDatePanel/status/DASH/DASH", DASH);

        // 隐藏注册界面
        View["SettingsPanel"].SetActive(false);
        View["OperationPanel"].SetActive(false);
        View["UIUpDate"].SetActive(false);

        // ボタンの配列を初期化
        buttons = new Button[]
        {
        View["GameStart"].GetComponent<Button>(),
        View["Settings"].GetComponent<Button>(),
        View["Operation"].GetComponent<Button>(),
        View["UpDate"].GetComponent<Button>(),
        View["Exit"].GetComponent<Button>(),
        View["Logout"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/HEALTH/HEALTH"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/ATTACK/ATTACK"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/DEFENSE/DEFENSE"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/SPEED/SPEED"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/MP/MP"].GetComponent<Button>(),
        View["UIUpDate/UpDatePanel/status/DASH/DASH"].GetComponent<Button>(),

        };

        // スライダーとパネルを初期化する
        BgmSlider = View["SettingsPanel/BGMSlider"].GetComponent<Slider>();
        GseSlider = View["SettingsPanel/GSESlider"].GetComponent<Slider>();

        // 设置滑块的初始值为100%
        BgmSlider.value = 1.0f;
        GseSlider.value = 1.0f;

        View["SettingsPanel"].SetActive(false); // 設定パネルを非表示にする
        View["OperationPanel"].SetActive(false); // 操作説明パネルを非表示にする

        BGMPercentage = View["SettingsPanel/BGMPercentage"].GetComponent<Text>(); // BGMの音量を表示するテキストを取得
        GSEPercentage = View["SettingsPanel/GSEPercentage"].GetComponent<Text>(); // 効果音の音量を表示するテキストを取得
        BGM = View["SettingsPanel/BGM"].GetComponent<Text>(); // BGMのラベルを表示するテキストを取得
        GSE = View["SettingsPanel/GSE"].GetComponent<Text>(); // 効果音のラベルを表示するテキストを取得

        // 更新音量百分比文本
        BGMPercentage.text = "100%";
        GSEPercentage.text = "100%";

        _blackImage = View["Black"].GetComponent<Image>();// Black Image を取得

        // GseSliderにイベントトリガーを追加して、ポインターが離された時のイベントを検知する
        EventTrigger trigger = GseSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnSfxSliderPointerUp(); });
        trigger.triggers.Add(entry);

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());

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

    // スクリプトが有効になったときに呼び出される
    private void OnEnable()
    {
        // スライダーの値が変更されたときのリスナーを追加
        BgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });
    }

    // スクリプトが無効になったときに呼び出される
    private void OnDisable()
    {
        // スライダーの値が変更されたときのリスナーを削除
        BgmSlider.onValueChanged.RemoveListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.RemoveListener(delegate { OnSfxVolumeChanged(); });
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        if (_blackImage != null)
        {
            _blackImage.gameObject.SetActive(false);
        }
    }

    // フレームごとに呼び出される
    void Update()
    {
        // エスケープキーが押された場合の処理
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (View["SettingsPanel"].activeSelf)
            {
                View["SettingsPanel"].SetActive(false);
                _currentSelectables = _homeSelectables;  // 切换回登录界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }
            if (View["OperationPanel"].activeSelf)
            {
                View["OperationPanel"].SetActive(false);
                _currentSelectables = _homeSelectables;  // 切换回登录界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }
            if (View["UIUpDate"].activeSelf)
            {
                View["UIUpDate"].SetActive(false);
                _currentSelectables = _homeSelectables;  // 切换回登录界面的可选择元素
                _currentSelectionIndex = 0; // 重置选择索引
            }
        }

        // キーボードナビゲーションの処理を行う
        HandleKeyboardNavigation();
    }

    // キーボードによるナビゲーションの処理を行う
    private void HandleKeyboardNavigation()
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

        if (selectedElement == SelectedElement.Button)
        {
            // 处理右方向键或D键的输入
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                _currentSelectionIndex = (_currentSelectionIndex + 1) % _currentSelectables.Count;
                SelectNewInput();
            }
            // 处理左方向键或A键的输入
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                _currentSelectionIndex = (_currentSelectionIndex - 1 + _currentSelectables.Count) % _currentSelectables.Count;
                SelectNewInput();
            }
            // 处理下方向键或S键的输入
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (View["SettingsPanel"].activeSelf)
                {
                    selectedElement = SelectedElement.BgmSlider;
                    BgmSlider.Select();
                    ScaleText(BGM, true);
                }
            }
            // 处理回车键的输入
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Button currentButton = _currentSelectables[_currentSelectionIndex] as Button;
                if (currentButton != null)
                {
                    StartCoroutine(ScaleButtonOnPress(currentButton));
                    currentButton.onClick.Invoke();
                }
            }
        }
        else if (selectedElement == SelectedElement.BgmSlider)
        {
            HandleSliderInput(BgmSlider, GSE, BGM);
        }
        else if (selectedElement == SelectedElement.GseSlider)
        {
            HandleSliderInput(GseSlider, BGM, GSE);
        }
    }

    // 选择新的UI元素并应用选中效果
    private void SelectNewInput()
    {
        _currentSelectables[_currentSelectionIndex].Select();
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

    // 处理滑块输入逻辑
    private void HandleSliderInput(Slider slider, TextMeshProUGUI currentText, TextMeshProUGUI previousText)
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            slider.value = Mathf.Min(slider.value + 0.01f, 1.0f);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            slider.value = Mathf.Max(slider.value - 0.01f, 0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedElement = SelectedElement.Button;
            currentButtonIndex = Array.IndexOf(buttons, View["SettingsPanel/SBack"].GetComponent<Button>());
            SelectNewInput();
            ScaleText(currentText, false);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedElement = SelectedElement.GseSlider == slider ? SelectedElement.Button : SelectedElement.GseSlider;
            GseSlider.Select();
            ScaleText(previousText, false);
            ScaleText(currentText, true);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            AudioManager.Instance.PlayGseChangeSound();
        }
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

    // 指定されたインデックスのボタンを選択状態にする
    private void SelectButton(int index)
    {
        if (buttons != null && buttons.Length > 0 && index >= 0 && index < buttons.Length)
        {
            // すべてのボタンの選択を解除して、スケールをリセットする
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != index)
                {
                    ExecuteEvents.Execute(buttons[i].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                }
            }

            // 指定されたボタンを選択状態にする
            buttons[index].Select();
            ExecuteEvents.Execute(buttons[index].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        }
    }

    // パネルの遷移に遅延を加えるコルーチン
    private IEnumerator PanelDelayCoroutine(System.Action action)
    {
        if (panelDelayInProgress)
            yield break;

        panelDelayInProgress = true;
        yield return new WaitForSeconds(0.5f);
        action.Invoke();
        panelDelayInProgress = false;
    }


    // BGMの音量が変更されたときの処理
    public void OnBgmVolumeChanged()
    {
        AudioManager.Instance.BgmValue = BgmSlider.value;
        DebugLogger.Log("BgmValue: " + AudioManager.Instance.BgmValue);

        BGMPercentage.text = (BgmSlider.value * 100f).ToString("F0") + "%";
    }

    // 効果音の音量が変更されたときの処理
    public void OnSfxVolumeChanged()
    {
        AudioManager.Instance.GseValue = GseSlider.value;
        DebugLogger.Log("GseValue: " + AudioManager.Instance.GseValue);

        GSEPercentage.text = (GseSlider.value * 100f).ToString("F0") + "%";
    }

    // 効果音スライダーがポインターから離れたときの処理
    private void OnSfxSliderPointerUp()
    {
        AudioManager.Instance.PlayGseChangeSound();
    }

    // ボタンにホバーエフェクトを追加する
    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }

    // テキストのスケーリングを行う
    private void ScaleText(Text text, bool enlarge)
    {
        if (text == null)
        {
            Debug.LogError("Text is null");
            return;
        }

        float scaleFactor = enlarge ? 1.5f : 1.0f;
        text.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }


    // ゲームを開始する処理
    private void GameStart()
    {
        Debug.Log("GameStart");
        UIManager.Instance.RemoveUI("UIHome");
        UIManager.Instance.ShowUI("UIGame");
        UIManager.Instance.ChangeUIPrefab("UIGame");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }

        GameManager.Instance.ChangeState(GameState.Gameplay);
        //SceneManager.LoadScene("MainGame");
    }

    // 設定画面を表示する処理
    private void Settings()
    {
        Debug.Log("Settings");
        bool currentStatus = View["SettingsPanel"].activeSelf;

        View["SettingsPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    // 操作説明画面を表示する処理
    private void Operation()
    {
        Debug.Log("Operation");
        bool currentStatus = View["OperationPanel"].activeSelf;

        View["OperationPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    // パワーアップ画面を表示する処理
    private void UpDate()
    {
        Debug.Log("UIUpDate");
        bool currentStatus = View["UIUpDate"].activeSelf;

        View["UIUpDate"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    // アプリケーションを終了する処理
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Logout()
    {
        string filePath = Application.persistentDataPath + "/LoginAccount.json";
        string json = File.ReadAllText(filePath);
        AccountManager accountManager = JsonUtility.FromJson<AccountManager>(json);


        var accountname = accountManager.accountname;
        var password = accountManager.password;
        _api.isLogout = true;
        _api.StringSaveAccount(accountname, password);
        _api.isLogout = false;
        _api.isLogin = false; //次ログインするためにフラグをfalseにしておく


        Debug.Log("Logout");
        UIManager.Instance.RemoveUI("UIHome");
        UIManager.Instance.ShowUI("UILogin");
        UIManager.Instance.ChangeUIPrefab("UILogin");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void HEALTH()
    {
        Debug.Log("Exit");

    }
    private void ATTACK()
    {
        Debug.Log("Exit");
    }
    private void DEFENSE()
    {
        Debug.Log("Exit");
    }
    private void SPEED()
    {
        Debug.Log("Exit");
    }
    private void MP()
    {
        Debug.Log("Exit");
    }
    private void DASH()
    {
        Debug.Log("Exit");
    }

}
*/