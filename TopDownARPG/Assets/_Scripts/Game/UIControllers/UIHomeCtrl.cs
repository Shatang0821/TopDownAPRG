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

        AddButtonHoverAnimation("GameStart");
        AddButtonHoverAnimation("Settings");
        AddButtonHoverAnimation("Operation");
        AddButtonHoverAnimation("Exit");
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

    // 添加按钮悬停事件监听
    private void AddButtonHoverAnimation(string buttonName)
    {
        Button button = View[buttonName].GetComponent<Button>();
        Animator animator = button.GetComponent<Animator>();

        // 添加鼠标进入事件监听
        EventTrigger triggerEnter = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => {
            // 检查Animator组件是否为空
            if (animator != null)
            {
                animator.SetTrigger("Selected");

                // 将其他按钮设置为正常状态
                foreach (var btn in View.Values)
                {
                    if (btn != button.gameObject)
                    {
                        var otherAnimator = btn.GetComponent<Animator>();
                        if (otherAnimator != null)
                        {
                            otherAnimator.SetTrigger("Normal");
                        }
                    }
                }
            }
        });
        triggerEnter.triggers.Add(entryEnter);

        // 添加鼠标离开事件监听
        EventTrigger triggerExit = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => {
            // 检查Animator组件是否为空
            if (animator != null)
            {
                animator.SetTrigger("Normal");
            }
        });
        triggerExit.triggers.Add(entryExit);
    }


}
