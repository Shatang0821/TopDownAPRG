using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System.Numerics;
using System;

public class UIGameCtrl : UICtrl
{
    private Image _blackImage; // Black Image を格納するための変数
    private bool isGamePaused = false;

    public override void Awake()
    {
        base.Awake();
        AddButtonListener("ChoosePanel/SettingsPanel", SettingsPanel);
        AddButtonListener("ChoosePanel/OperationPanel", OperationPanel);
        AddButtonListener("ChoosePanel/Exit", Exit);

        AddButtonHoverEffect("ChoosePanel/SettingsPanel");
        AddButtonHoverEffect("ChoosePanel/OperationPanel");
        AddButtonHoverEffect("ChoosePanel/Exit");

        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());

        // 播放游戏背景音乐并禁用其他 BGM
        AudioManager.Instance.PlayGameBgmPlayer();
        AudioManager.Instance.StopAllNonGameBgmPlayers();
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleChoosePanel();
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            UIManager.Instance.RemoveUI("UIGame");
            UIManager.Instance.ShowUI("UIEnd");
            UIManager.Instance.ChangeUIPrefab("UIEnd");

            if (this.gameObject != null)
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            UIManager.Instance.RemoveUI("UIGame");
            UIManager.Instance.ShowUI("UIWin");
            UIManager.Instance.ChangeUIPrefab("UIWin");

            if (this.gameObject != null)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private void ToggleChoosePanel()
    {
        bool isActive = View["ChoosePanel"].activeSelf;
        View["ChoosePanel"].SetActive(!isActive);

        if (!isActive)
        {
            // パネルを表示するときにゲームを一時停止
            PauseGame();
        }
        else
        {
            // パネルを非表示にするときにゲームを再開
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // ゲームを一時停止
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // ゲームを再開
    }

    private void SettingsPanel()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIWin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OperationPanel()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIWin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Exit()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIWin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void AddButtonHoverEffect(string buttonName)
    {
        Button button = View[buttonName].GetComponent<Button>();
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }
}
