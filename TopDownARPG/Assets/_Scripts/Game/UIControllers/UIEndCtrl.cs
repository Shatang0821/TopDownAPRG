using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;


public class UIEndCtrl : UICtrl
{
    private Image _blackImage; // Black Image を格納するための変数

    public override void Awake()
    {

        base.Awake();
        AddButtonListener("Title", Title);
        AddButtonListener("Exit", Exit);
        AddButtonListener("GameLose/ReStart", ReStart);
         
        AddButtonHoverEffect("Title");
        AddButtonHoverEffect("Exit");
        AddButtonHoverEffect("GameLose/ReStart");

        _blackImage = View["Black"].GetComponent<Image>();// Black Image を取得

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }

    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.RemoveUI("UIEnd");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void ReStart()
    {
        Debug.Log("ReStart");
        UIManager.Instance.RemoveUI("UIEnd");
        UIManager.Instance.ShowUI("UIGame");
        UIManager.Instance.ChangeUIPrefab("UIGame");

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


