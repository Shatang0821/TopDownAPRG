using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FrameWork.UI;
using Unity.VisualScripting;
using System;

public class UILoginCtrl : UICtrl
{
    public override void Awake()
    {
        base.Awake();
        AddButtonListener("Register", Register);
        AddButtonListener("SingIn", SingIn);
        AddButtonListener("RegistrationScreenPanel/Complete", Complete);
        AddButtonListener("RegistrationScreenPanel/Back", Back);
        View["RegistrationScreenPanel"].SetActive(false);
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (View["RegistrationScreenPanel"].activeSelf)
            {
                View["RegistrationScreenPanel"].SetActive(false);
            }
        }
    }

    private void Register()
    {
        Debug.Log("Register");
        bool currentStatus = View["RegistrationScreenPanel"].activeSelf;
        View["RegistrationScreenPanel"].SetActive(!currentStatus);
    }

    private void SingIn()
    {
        Debug.Log("SingIn");
    }

    private void Complete()
    {
        Debug.Log("Complete");
    }
    
    private void Back()
    {
        if (View["RegistrationScreenPanel"].activeSelf)
        {
            View["RegistrationScreenPanel"].SetActive(false);
        }
        Debug.Log("Back");
    }
}
