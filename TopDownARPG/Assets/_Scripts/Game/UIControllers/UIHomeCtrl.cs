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

        // Add EventTrigger for GseSlider
        EventTrigger trigger = GseSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnSfxSliderPointerUp(); });
        trigger.triggers.Add(entry);

        AddButtonHoverEffect("GameStart");
        AddButtonHoverEffect("Settings");
        AddButtonHoverEffect("Operation");
        AddButtonHoverEffect("Exit");
        AddButtonHoverEffect("SettingsPanel/Back");
        AddButtonHoverEffect("OperationPanel/Back");

    }

    void Start()
    {
        BgmSlider.SetValueWithoutNotify(1.0f);
        GseSlider.SetValueWithoutNotify(1.0f);

        BGMPercentage.text = "100%";
        GSEPercentage.text = "100%";
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

    private void AddButtonHoverEffect(string buttonName)
    {
        Button button = View[buttonName].GetComponent<Button>();
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(originalScale * 1.8f, 0.2f));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(originalScale, 0.2f));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(originalScale * 0.5f, 0.2f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(originalScale * 1.8f, 0.2f));
        }
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
