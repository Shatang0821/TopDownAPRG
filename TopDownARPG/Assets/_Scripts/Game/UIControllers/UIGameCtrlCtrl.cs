using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.UI;

public class UIGameCtrlCtrl : UICtrl {

	public override void Awake() {

		base.Awake();
        AddButtonListener("Title", Title);
        AddButtonListener("Exit", Exit);
        AddButtonListener("ReStart", ReStart);

    }

	void Start() {
	}

    private void Title()
    {
        Debug.Log("Title");
    }

    private void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }   

    private void ReStart()
    {
        Debug.Log("ReStart");
    }

}
