using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;

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
}