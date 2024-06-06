using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Utils;
using FrameWork.Audio;
using UnityEngine.EventSystems;


public class UIEndCtrl : UICtrl
{

    public override void Awake()
    {

        base.Awake();
        AddButtonListener("Title", Title);
        AddButtonListener("Exit", Exit);
        AddButtonListener("GameLose/ReStart", ReStart);
         
        AddButtonHoverEffect("Title");
        AddButtonHoverEffect("Exit");
        AddButtonHoverEffect("GameLose/ReStart");

    }

    private void Title()
    {
        Debug.Log("Title");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        // 手动隐藏UILogin
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
        UIManager.Instance.ShowUI("UIGame");
        UIManager.Instance.ChangeUIPrefab("UIGame");

        // 手动隐藏UILogin
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


