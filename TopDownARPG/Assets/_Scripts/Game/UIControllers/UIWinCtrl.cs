using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System.Numerics;
using System;

public class UIWinCtrl : UICtrl
{
    private Image _blackImage; // Black Image を格納するための変数

    public override void Awake()
    {
        base.Awake();

        AddButtonListener("Exit", Exit);
        AddButtonListener("Title", Title);

        AddButtonHoverEffect("Exit");
        AddButtonHoverEffect("Title");

        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());

        AudioManager.Instance.PlayWinBgm();
        AudioManager.Instance.StopAllNonWinBgms();
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void Title()
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
