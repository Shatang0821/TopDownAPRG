using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using FrameWork.UI;

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

        AddButtonHoverEffect("Register");
        AddButtonHoverEffect("SingIn");
        AddButtonHoverEffect("RegistrationScreenPanel/Complete");
        AddButtonHoverEffect("RegistrationScreenPanel/Back");
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
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
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

    private void AddButtonHoverEffect(string buttonName)
    {
        Button button = View[buttonName].GetComponent<Button>();
        ButtonHoverEffect hoverEffect = button.gameObject.AddComponent<ButtonHoverEffect>();
        hoverEffect.SetOriginalScale(button.transform.localScale);
    }
}
