using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;

public class UIHomeCtrl : UICtrl
{
    public Slider BgmSlider;
    public Slider GseSlider;
    public Text BGMPercentage;
    public Text GSEPercentage;
    public Text BGM;
    public Text GSE;

    private Button _gameStart;
    private Button _lastSelectedButton;

    private bool panelDelayInProgress = false;
    private int currentButtonIndex = 0;
    private Button[] buttons;

    private enum SelectedElement { Button, BgmSlider, GseSlider }
    private SelectedElement selectedElement = SelectedElement.Button;

    public override void Awake()
    {
        base.Awake();
        AddButtonListener("GameStart", () => StartCoroutine(PanelDelayCoroutine(GameStart)));
        AddButtonListener("Settings", () => StartCoroutine(PanelDelayCoroutine(Settings)));
        AddButtonListener("Operation", () => StartCoroutine(PanelDelayCoroutine(Operation)));
        AddButtonListener("Exit", () => StartCoroutine(PanelDelayCoroutine(Exit)));
        AddButtonListener("SettingsPanel/Back", () => StartCoroutine(PanelDelayCoroutine(Back)));
        AddButtonListener("OperationPanel/Back", () => StartCoroutine(PanelDelayCoroutine(Back)));

        BgmSlider = View["SettingsPanel/BGMSlider"].GetComponent<Slider>();
        GseSlider = View["SettingsPanel/GSESlider"].GetComponent<Slider>();
        View["SettingsPanel"].SetActive(false);
        View["OperationPanel"].SetActive(false);

        BGMPercentage = View["SettingsPanel/BGMPercentage"].GetComponent<Text>();
        GSEPercentage = View["SettingsPanel/GSEPercentage"].GetComponent<Text>();
        BGM = View["SettingsPanel/BGM"].GetComponent<Text>();
        GSE = View["SettingsPanel/GSE"].GetComponent<Text>();

        _gameStart = View["GameStart"].GetComponent<Button>();

        // Get all buttons
        buttons = new Button[]
        {
            View["GameStart"].GetComponent<Button>(),
            View["Settings"].GetComponent<Button>(),
            View["Operation"].GetComponent<Button>(),
            View["Exit"].GetComponent<Button>(),
            View["SettingsPanel/Back"].GetComponent<Button>(),
            View["OperationPanel/Back"].GetComponent<Button>()
        };

        foreach (var button in buttons)
        {
            AddButtonHoverEffect(button);
        }

        // Add EventTrigger for GseSlider
        EventTrigger trigger = GseSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp; 
        entry.callback.AddListener((data) => { OnSfxSliderPointerUp(); });
        trigger.triggers.Add(entry);
    }

    void Start()
    {
        BgmSlider.SetValueWithoutNotify(1.0f);
        GseSlider.SetValueWithoutNotify(1.0f);

        BGMPercentage.text = "100%";
        GSEPercentage.text = "100%";

        // Select the first button
        SelectButton(currentButtonIndex);
    }

    private void OnEnable()
    {
        BgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });

        _gameStart.Select();
    }

    private void OnDisable()
    {
        BgmSlider.onValueChanged.RemoveListener(delegate { OnBgmVolumeChanged(); });
        GseSlider.onValueChanged.RemoveListener(delegate { OnSfxVolumeChanged(); });
    }

    void Update()
    {
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

        HandleKeyboardNavigation();
    }

    private void HandleKeyboardNavigation()
    {
        if (selectedElement == SelectedElement.Button)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
                SelectButton(currentButtonIndex);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
                SelectButton(currentButtonIndex);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (View["SettingsPanel"].activeSelf)
                {
                    selectedElement = SelectedElement.BgmSlider;
                    BgmSlider.Select();
                    ScaleText(BGM, true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Button currentButton = buttons[currentButtonIndex];
                StartCoroutine(ScaleButtonOnPress(currentButton));
                currentButton.onClick.Invoke();
            }
        }
        else if (selectedElement == SelectedElement.BgmSlider)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                BgmSlider.value = Mathf.Min(BgmSlider.value + 0.01f, 1.0f);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                BgmSlider.value = Mathf.Max(BgmSlider.value - 0.01f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedElement = SelectedElement.Button;
                SelectButton(currentButtonIndex);
                ScaleText(BGM, false);
            }
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
            if(Input.GetKey(KeyCode.Return))
            {
                AudioManager.Instance.PlayGseChangeSound();
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GseSlider.value = Mathf.Min(GseSlider.value + 0.01f, 1.0f);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GseSlider.value = Mathf.Max(GseSlider.value - 0.01f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedElement = SelectedElement.BgmSlider;
                BgmSlider.Select();
                ScaleText(GSE, false);
                ScaleText(BGM, true);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedElement = SelectedElement.Button;
                SelectButton(currentButtonIndex);
                ScaleText(GSE, false);
            }
        }
    }

    private IEnumerator ScaleButtonOnPress(Button button)
    {
        ButtonHoverEffect hoverEffect = button.GetComponent<ButtonHoverEffect>();
        if (hoverEffect != null)
        {
            hoverEffect.OnPointerDown(null);
            yield return new WaitForSeconds(0.2f);  // Duration of the scale animation
            hoverEffect.OnPointerUp(null);
        }
    }

    private void SelectButton(int index)
    {
        if (buttons != null && buttons.Length > 0 && index >= 0 && index < buttons.Length)
        {
            // Deselect all buttons to reset their scale
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != index)
                {
                    ExecuteEvents.Execute(buttons[i].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                }
            }

            // Select the current button
            buttons[index].Select();

            // Trigger the pointer enter event to simulate hover effect
            ExecuteEvents.Execute(buttons[index].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        }
    }

    private void SaveLastSelectedButton(Button button)
    {
        _lastSelectedButton = button;
    }

    private void RestoreLastSelectedButton()
    {
        if (_lastSelectedButton != null)
        {
            _lastSelectedButton.Select();
            _lastSelectedButton = null;
        }
    }

    private IEnumerator PanelDelayCoroutine(System.Action action)
    {
        if (panelDelayInProgress)
            yield break;

        panelDelayInProgress = true;
        yield return new WaitForSeconds(0.5f);
        action.Invoke();
        panelDelayInProgress = false;
    }

    private void GameStart()
    {
        Debug.Log("GameStart");
        UIManager.Instance.ShowUI("UIGame");
        UIManager.Instance.ChangeUIPrefab("UIGame");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Settings()
    {
        Debug.Log("Settings");
        bool currentStatus = View["SettingsPanel"].activeSelf;
        if (!currentStatus)
        {
            SaveLastSelectedButton(buttons[currentButtonIndex]);
            currentButtonIndex = System.Array.IndexOf(buttons, View["SettingsPanel/Back"].GetComponent<Button>());
        }
        View["SettingsPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    private void Operation()
    {
        Debug.Log("Operation");
        bool currentStatus = View["OperationPanel"].activeSelf;
        if (!currentStatus)
        {
            SaveLastSelectedButton(buttons[currentButtonIndex]);
            currentButtonIndex = System.Array.IndexOf(buttons, View["OperationPanel/Back"].GetComponent<Button>());
        }
        View["OperationPanel"].SetActive(!currentStatus);
        SelectButton(currentButtonIndex);
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Back()
    {
        if (View["SettingsPanel"].activeSelf)
        {
            View["SettingsPanel"].SetActive(false);
            RestoreLastSelectedButton(); // 恢复上次选择的按钮
        }
        if (View["OperationPanel"].activeSelf)
        {
            View["OperationPanel"].SetActive(false);
            RestoreLastSelectedButton(); // 恢复上次选择的按钮
        }
        Debug.Log("Back");
    }

    public void OnBgmVolumeChanged()
    {
        AudioManager.Instance.BgmValue = BgmSlider.value;
        DebugLogger.Log("BgmValue: " + AudioManager.Instance.BgmValue);

        BGMPercentage.text = (BgmSlider.value * 100f).ToString("F0") + "%";
    }

    public void OnSfxVolumeChanged()
    {
        AudioManager.Instance.GseValue = GseSlider.value;
        DebugLogger.Log("GseValue: " + AudioManager.Instance.GseValue);

        GSEPercentage.text = (GseSlider.value * 100f).ToString("F0") + "%";
    }

    private void OnSfxSliderPointerUp()
    {
        AudioManager.Instance.PlayGseChangeSound();
    }

    private void AddButtonHoverEffect(Button button)
    {
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }

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
