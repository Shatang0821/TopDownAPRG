using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class UIHomeCtrl : UICtrl
{
    // UI関連のコンポーネントを格納するための変数
    public Slider BgmSlider; // BGMの音量を調整するためのスライダー
    public Slider GseSlider; // 効果音の音量を調整するためのスライダー
    public Text BGMPercentage; // BGMの音量を表示するテキスト
    public Text GSEPercentage; // 効果音の音量を表示するテキスト
    public Text BGM; // BGMのラベルを表示するテキスト
    public Text GSE; // 効果音のラベルを表示するテキスト

    private Button _gameStart; // ゲームを開始するボタン
    private Button _lastSelectedButton; // 最後に選択されたボタンを格納するための変数
    private Image _blackImage; // Black Image を格納するための変数

    private bool panelDelayInProgress = false; // パネルの遅延処理が進行中かどうかを示すフラグ
    private int currentButtonIndex = 0; // 現在選択されているボタンのインデックス
    private Button[] buttons; // すべてのボタンを格納する配列

    // UIで選択されている要素を示す列挙型
    private enum SelectedElement { Button, BgmSlider, GseSlider }
    private SelectedElement selectedElement = SelectedElement.Button; // 初期状態ではボタンが選択されている

    // Awakeメソッドをオーバーライドして、UIコンポーネントを初期化する
    public override void Awake()
    {
        base.Awake();

        // ボタンに関数を関連付ける
        AddButtonListener("GameStart", () => StartCoroutine(PanelDelayCoroutine(GameStart)));
        AddButtonListener("Settings", () => StartCoroutine(PanelDelayCoroutine(Settings)));
        AddButtonListener("Operation", () => StartCoroutine(PanelDelayCoroutine(Operation)));
        AddButtonListener("Exit", () => StartCoroutine(PanelDelayCoroutine(Exit)));
        AddButtonListener("SettingsPanel/SBack", () => StartCoroutine(PanelDelayCoroutine(Back)));
        AddButtonListener("OperationPanel/OBack", () => StartCoroutine(PanelDelayCoroutine(Back)));

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

        _gameStart = View["GameStart"].GetComponent<Button>(); // ゲーム開始ボタンを取得
        _blackImage = View["Black"].GetComponent<Image>();// Black Image を取得

        // ボタンの配列を初期化
        buttons = new Button[]
        {
        View["GameStart"].GetComponent<Button>(), // ゲーム開始ボタン
        View["Settings"].GetComponent<Button>(), // 設定ボタン
        View["Operation"].GetComponent<Button>(), // 操作説明ボタン
        View["Exit"].GetComponent<Button>(), // 終了ボタン
        View["SettingsPanel/SBack"].GetComponent<Button>(), // 設定パネルの戻るボタン
        View["OperationPanel/OBack"].GetComponent<Button>() // 操作説明パネルの戻るボタン
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
    }


    // スクリプトが有効になったときに呼び出される
    private void OnEnable()
    {
        // スライダーの値が変更されたときのリスナーを追加
        BgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });

        // ゲーム開始ボタンを選択状態にする
        _gameStart.Select();
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
                RestoreLastSelectedButton();
            }
            if (View["OperationPanel"].activeSelf)
            {
                View["OperationPanel"].SetActive(false);
                RestoreLastSelectedButton();
            }
        }

        // キーボードナビゲーションの処理を行う
        HandleKeyboardNavigation();
    }

    // キーボードによるナビゲーションの処理を行う
    private void HandleKeyboardNavigation()
    {
        if (selectedElement == SelectedElement.Button)
        {
            // 右矢印またはDキーが押された場合
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
                // 如果当前按钮是 "Exit"，则跳到 "GameStart" 按钮上
                if (buttons[currentButtonIndex].name == "SBack")
                {
                    currentButtonIndex = Array.IndexOf(buttons, View["GameStart"].GetComponent<Button>());
                }
                SelectButton(currentButtonIndex);
            }
            // 左矢印またはAキーが押された場合
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
                // 如果当前按钮是 "Exit"，则跳到 "GameStart" 按钮上
                if (buttons[currentButtonIndex].name == "OBack")
                {
                    currentButtonIndex = Array.IndexOf(buttons, View["Exit"].GetComponent<Button>());
                }
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
            // 上矢印またはWキーが押された場合
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedElement = SelectedElement.Button;
                // 如果当前处于 SettingsPanel 中，将焦点移到 Back 按钮上
                currentButtonIndex = Array.IndexOf(buttons, View["SettingsPanel/SBack"].GetComponent<Button>());
                SelectButton(currentButtonIndex);
                ScaleText(BGM, false);
            }
            // 下矢印またはSキーが押された場合
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
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
            // 上矢印またはWキーが押された場合
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedElement = SelectedElement.BgmSlider;
                BgmSlider.Select();
                ScaleText(GSE, false);
                ScaleText(BGM, true);
            }
            // 下矢印またはSキーが押された場合
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedElement = SelectedElement.Button;
                // 如果当前处于 SettingsPanel 中，将焦点移到 Back 按钮上
                currentButtonIndex = Array.IndexOf(buttons, View["SettingsPanel/SBack"].GetComponent<Button>());
                SelectButton(currentButtonIndex);
                ScaleText(GSE, false);
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

    // 最後に選択されたボタンを保存する
    private void SaveLastSelectedButton(Button button)
    {
        _lastSelectedButton = button;
    }

    // 最後に選択されたボタンを復元する
    private void RestoreLastSelectedButton()
    {
        if (_lastSelectedButton != null)
        {
            _lastSelectedButton.Select();
            _lastSelectedButton = null;
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
    }

    // 設定画面を表示する処理
    private void Settings()
    {
        Debug.Log("Settings");
        bool currentStatus = View["SettingsPanel"].activeSelf;
        if (!currentStatus)
        {
            SaveLastSelectedButton(buttons[currentButtonIndex]);
            currentButtonIndex = System.Array.IndexOf(buttons, View["SettingsPanel/SBack"].GetComponent<Button>());
        }
        View["SettingsPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    // 操作説明画面を表示する処理
    private void Operation()
    {
        Debug.Log("Operation");
        bool currentStatus = View["OperationPanel"].activeSelf;
        if (!currentStatus)
        {
            SaveLastSelectedButton(buttons[currentButtonIndex]);
            currentButtonIndex = System.Array.IndexOf(buttons, View["OperationPanel/OBack"].GetComponent<Button>());
        }
        View["OperationPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    // アプリケーションを終了する処理
    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    // 戻るボタンが押されたときの処理
    private void Back()
    {
        if (View["SettingsPanel"].activeSelf)
        {
            View["SettingsPanel"].SetActive(false);
            RestoreLastSelectedButton();
        }
        if (View["OperationPanel"].activeSelf)
        {
            View["OperationPanel"].SetActive(false);
            RestoreLastSelectedButton();
        }
        Debug.Log("Back");
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
