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

    private Image _blackImage; // Black Image を格納するための変数

    private bool panelDelayInProgress = false; // パネルの遅延処理が進行中かどうかを示すフラグ
    private int currentButtonIndex = 0; // 現在選択されているボタンのインデックス
    private Button[] buttons; // すべてのボタンを格納する配列

    private Button[] upDateButtons; // 存储 UpDatePanel 中的按钮
    private int currentUpDateButtonIndex = 0; // 当前选择的 UpDatePanel 中的按钮索引

    // UIで選択されている要素を示す列挙型
    private enum SelectedElement { Button, BgmSlider, GseSlider }
    private SelectedElement selectedElement = SelectedElement.Button; // 初期状態ではボタンが選択されている

    private API _api;

    // Awakeメソッドをオーバーライドして、UIコンポーネントを初期化する
    public override void Awake()
    {
        base.Awake();

        _api = FindObjectOfType<API>();

        // ボタンに関数を関連付ける
        AddButtonListener("GameStart", () => StartCoroutine(PanelDelayCoroutine(GameStart)));
        AddButtonListener("Settings", () => StartCoroutine(PanelDelayCoroutine(Settings)));
        AddButtonListener("Operation", () => StartCoroutine(PanelDelayCoroutine(Operation)));
        AddButtonListener("UpDate", () => StartCoroutine(PanelDelayCoroutine(UpDate)));
        AddButtonListener("Exit", () => StartCoroutine(PanelDelayCoroutine(Exit)));
        AddButtonListener("Logout", () => StartCoroutine(PanelDelayCoroutine(Logout)));

        AddButtonListener("UpDatePanel/status/HEALTH/HEALTH", HEALTH);
        AddButtonListener("UpDatePanel/status/ATTACK/ATTACK", ATTACK);
        AddButtonListener("UpDatePanel/status/DEFENSE/DEFENSE", DEFENSE);
        AddButtonListener("UpDatePanel/status/SPEED/SPEED", SPEED);
        AddButtonListener("UpDatePanel/status/MP/MP", MP);
        AddButtonListener("UpDatePanel/status/DASH/DASH", DASH);

        // スライダーとパネルを初期化する
        BgmSlider = View["SettingsPanel/BGMSlider"].GetComponent<Slider>();
        GseSlider = View["SettingsPanel/GSESlider"].GetComponent<Slider>();

        // 设置滑块的初始值为100%
        BgmSlider.value = 1.0f;
        GseSlider.value = 1.0f;

        View["SettingsPanel"].SetActive(false); // 設定パネルを非表示にする
        View["OperationPanel"].SetActive(false); // 操作説明パネルを非表示にする
        View["UpDatePanel"].SetActive(false); // 操作説明パネルを非表示にする

        BGMPercentage = View["SettingsPanel/BGMPercentage"].GetComponent<Text>(); // BGMの音量を表示するテキストを取得
        GSEPercentage = View["SettingsPanel/GSEPercentage"].GetComponent<Text>(); // 効果音の音量を表示するテキストを取得
        BGM = View["SettingsPanel/BGM"].GetComponent<Text>(); // BGMのラベルを表示するテキストを取得
        GSE = View["SettingsPanel/GSE"].GetComponent<Text>(); // 効果音のラベルを表示するテキストを取得

        // 更新音量百分比文本
        BGMPercentage.text = "100%";
        GSEPercentage.text = "100%";

        _blackImage = View["Black"].GetComponent<Image>();// Black Image を取得

        // ボタンの配列を初期化
        buttons = new Button[]
        {
        View["GameStart"].GetComponent<Button>(), // ゲーム開始ボタン
        View["Settings"].GetComponent<Button>(), // 設定ボタン
        View["Operation"].GetComponent<Button>(), // 操作説明ボタン
        View["UpDate"].GetComponent <Button>(),
        View["Exit"].GetComponent<Button>(), // 終了ボタン
        View["Logout"].GetComponent<Button>(), // 終了ボタン
        };

        // 在 Awake 方法中初始化 upDateButtons
        upDateButtons = new Button[]
        {
        View["UpDatePanel/status/HEALTH/HEALTH"].GetComponent<Button>(),
        View["UpDatePanel/status/ATTACK/ATTACK"].GetComponent<Button>(),
        View["UpDatePanel/status/DEFENSE/DEFENSE"].GetComponent<Button>(),
        View["UpDatePanel/status/SPEED/SPEED"].GetComponent<Button>(),
        View["UpDatePanel/status/MP/MP"].GetComponent<Button>(),
        View["UpDatePanel/status/DASH/DASH"].GetComponent<Button>(),
        };


        // すべてのボタンにホバーエフェクトを追加
        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }

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
    }


    // スクリプトが有効になったときに呼び出される
    private void OnEnable()
    {
        // スライダーの値が変更されたときのリスナーを追加
        BgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });

        // 默认选择 BgmSlider
        if (View["SettingsPanel"].activeSelf)
        {
            selectedElement = SelectedElement.BgmSlider;
            BgmSlider.Select(); // 确保 BgmSlider 被选择
        }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (View["SettingsPanel"].activeSelf)
            {
                View["SettingsPanel"].SetActive(false);

                selectedElement = SelectedElement.Button; // 重置选中的元素
                currentButtonIndex = 0; // 或设置为最后选中的按钮索引
                SelectButton(currentButtonIndex); // 重新选择当前按钮
            }
            if (View["OperationPanel"].activeSelf)
            {
                View["OperationPanel"].SetActive(false);

                selectedElement = SelectedElement.Button; // 重置选中的元素
                currentButtonIndex = 0; // 或设置为最后选中的按钮索引
                SelectButton(currentButtonIndex); // 重新选择当前按钮
            }
            if (View["UpDatePanel"].activeSelf)
            {
                View["UpDatePanel"].SetActive(false);

                selectedElement = SelectedElement.Button; // 重置选中的元素
                currentButtonIndex = 0; // 或设置为最后选中的按钮索引
                SelectButton(currentButtonIndex); // 重新选择当前按钮
            }
        }

        // 如果 UpDatePanel 激活，则处理 UpDatePanel 中的键盘导航
        if (View["UpDatePanel"].activeSelf)
        {
            HandleUpDateKeyboardNavigation();
        }
        else
        {
            // 否则使用默认的键盘导航逻辑
            HandleKeyboardNavigation();
        }
    }


    // 创建一个新的方法来处理 UpDatePanel 中的键盘导航
    private void HandleUpDateKeyboardNavigation()
    {
        // 右矢印またはDキーが押された場合
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentUpDateButtonIndex = (currentUpDateButtonIndex + 1) % upDateButtons.Length;
            SelectUpDateButton(currentUpDateButtonIndex);
        }
        // 左矢印またはAキーが押された場合
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentUpDateButtonIndex = (currentUpDateButtonIndex - 1 + upDateButtons.Length) % upDateButtons.Length;
            SelectUpDateButton(currentUpDateButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentUpDateButtonIndex = (currentUpDateButtonIndex + 3 + upDateButtons.Length) % upDateButtons.Length;
            SelectUpDateButton(currentUpDateButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentUpDateButtonIndex = (currentUpDateButtonIndex - 3 + upDateButtons.Length) % upDateButtons.Length;
            SelectUpDateButton(currentUpDateButtonIndex);
        }
        // リターンキーが押された場合
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Button currentButton = upDateButtons[currentUpDateButtonIndex];
            StartCoroutine(ScaleButtonOnPress(currentButton));
            currentButton.onClick.Invoke();
        }

    }

    // 选择 UpDatePanel 中指定索引的按钮
    private void SelectUpDateButton(int index)
    {
        if (upDateButtons != null && upDateButtons.Length > 0 && index >= 0 && index < upDateButtons.Length)
        {
            // 取消选择所有按钮
            for (int i = 0; i < upDateButtons.Length; i++)
            {
                if (i != index)
                {
                    ExecuteEvents.Execute(upDateButtons[i].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                }
            }

            // 选择当前按钮
            upDateButtons[index].Select();
            ExecuteEvents.Execute(upDateButtons[index].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        }
    }

    // キーボードによるナビゲーションの処理を行う
    private void HandleKeyboardNavigation()
    {
        if (selectedElement == SelectedElement.Button)
        {
            // 右矢印またはDキーが押された場合
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
                SelectButton(currentButtonIndex);
            }
            // 左矢印またはAキーが押された場合
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
                SelectButton(currentButtonIndex);
            }
            // 下矢印またはSキーが押された場合（設定パネルがアクティブならば）
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (View["SettingsPanel"].activeSelf)
                {
                    selectedElement = SelectedElement.BgmSlider;
                    BgmSlider.Select();
                    ScaleText(BGM, true);
                }
            }
            // リターンキーが押された場合
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Button currentButton = buttons[currentButtonIndex];
                StartCoroutine(ScaleButtonOnPress(currentButton));
                currentButton.onClick.Invoke();
            }
        }
        else if (selectedElement == SelectedElement.BgmSlider)
        {
            // Dキーまたは右矢印が押された場合
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                BgmSlider.value = Mathf.Min(BgmSlider.value + 0.01f, 1.0f);
            }
            // Aキーまたは左矢印が押された場合
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                BgmSlider.value = Mathf.Max(BgmSlider.value - 0.01f, 0.0f);
            }
            // 下矢印またはSキーが押された場合
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedElement = SelectedElement.GseSlider;
                GseSlider.Select();
                ScaleText(BGM, false);
                ScaleText(GSE, true);
            }
        }
        else if (selectedElement == SelectedElement.GseSlider)
        {
            // Dキーまたは右矢印が押された場合
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GseSlider.value = Mathf.Min(GseSlider.value + 0.01f, 1.0f);
            }
            // Aキーまたは左矢印が押された場合
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GseSlider.value = Mathf.Max(GseSlider.value - 0.01f, 0.0f);
            }

            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedElement = SelectedElement.BgmSlider;
                BgmSlider.Select();
                ScaleText(GSE, false);
                ScaleText(BGM, true);
            }

            // キーが離された場合、効果音を再生する
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                AudioManager.Instance.PlayGseChangeSound();
            }
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

        if (!currentStatus) // 如果当前面板刚被激活
        {
            selectedElement = SelectedElement.BgmSlider; // 默认选择 BgmSlider
            BgmSlider.Select(); // 选择 BgmSlider
            ScaleText(BGM, true); // 放大 BGM 标签
        }
    }

    // 操作説明画面を表示する処理
    private void Operation()
    {
        Debug.Log("Operation");
        bool currentStatus = View["OperationPanel"].activeSelf;

        View["OperationPanel"].SetActive(!currentStatus);
    }

    // パワーアップ画面を表示する処理
    private void UpDate()
    {
        Debug.Log("UIUpDate");
        bool currentStatus = View["UpDatePanel"].activeSelf;

        View["UpDatePanel"].SetActive(!currentStatus);
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
}
