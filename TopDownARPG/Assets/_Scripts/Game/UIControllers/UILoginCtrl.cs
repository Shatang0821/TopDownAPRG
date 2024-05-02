using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.UI;
using Unity.VisualScripting;
using System;


public class UILoginCtrl : UICtrl
{

	public override void Awake() {

		base.Awake();
        AddButtonListener("Register", Register);
        AddButtonListener("SingIn", SingIn);
        AddButtonListener("Registration Screen/Complete", Complete);
    }

	void Start() {
	}

    private void Register()
    {
        Debug.Log("Register");
    }

    private void SingIn()
    {
        Debug.Log("SingIn");
    }

    private void Complete()
    {
        Debug.Log("Complete");
    }
}
