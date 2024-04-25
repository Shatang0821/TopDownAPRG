using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.UI;
using Unity.VisualScripting;

public class UIHomeCtrlCtrl : UICtrl
{
    public override void Awake()
    {
        base.Awake();
        AddButtonListener("GameStart", GameStart);
        AddButtonListener("Settings", Settings);
        AddButtonListener("Operation", Operation);
        AddButtonListener("Exit", Exit);
        View["SettingsPanel"].SetActive(false);
        View["OperationPanel"].SetActive(false);
    }

    void Start()
    {
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
}
