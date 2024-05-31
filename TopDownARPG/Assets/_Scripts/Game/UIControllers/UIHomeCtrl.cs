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

    private Button _gameStart;

    private bool panelDelayInProgress = false;
    private int currentButtonIndex = 0;
    private Button[] buttons;

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
            }
            if (View["OperationPanel"].activeSelf)
            {
                View["OperationPanel"].SetActive(false);
            }
        }

        HandleKeyboardNavigation();
    }

    private void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // Find the index of the next selectable button excluding Back button
            currentButtonIndex = GetNextSelectableButtonIndex(1);
            SelectButton(currentButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // Find the index of the previous selectable button excluding Back button
            currentButtonIndex = GetNextSelectableButtonIndex(-1);
            SelectButton(currentButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Button currentButton = buttons[currentButtonIndex];
            StartCoroutine(ScaleButtonOnPress(currentButton));
            currentButton.onClick.Invoke();
        }
    }

    private int GetNextSelectableButtonIndex(int direction)
    {
        int nextIndex = currentButtonIndex;

        do
        {
            // Move to the next index
            nextIndex = (nextIndex + direction + buttons.Length) % buttons.Length;

            // Check if the button at the new index is the Back button
            if (!buttons[nextIndex].gameObject.name.Contains("Back"))
            {
                // If not, return the new index
                return nextIndex;
            }
        } while (nextIndex != currentButtonIndex); // Continue until the original index is reached

        // If all buttons are Back buttons, return the original index
        return currentButtonIndex;
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
    }

    private void Settings()
    {
        Debug.Log("Settings");
        bool currentStatus = View["SettingsPanel"].activeSelf;
        View["SettingsPanel"].SetActive(!currentStatus);
    }

    private void Operation()
    {
        Debug.Log("Operation");
        bool currentStatus = View["OperationPanel"].activeSelf;
        View["OperationPanel"].SetActive(!currentStatus);
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
        }
        if (View["OperationPanel"].activeSelf)
        {
            View["OperationPanel"].SetActive(false);
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
}
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void SetOriginalScale(Vector3 scale)
    {
        originalScale = scale;
        button = GetComponent<Button>();  // Ensure button is initialized
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 1.8f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 0.5f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 1.8f);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        if (button.interactable)
        {
            ScaleButton(originalScale * 1.8f);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (button == null) button = GetComponent<Button>();  // Ensure button is not null
        ScaleButton(originalScale);
    }

    private void ScaleButton(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleTo(targetScale, 0.2f));
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
