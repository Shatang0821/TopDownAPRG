using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;

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

        // 播放背景音乐
        PlayBackgroundMusic();
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
        UIManager.Instance.RemoveUI("UILogin");
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

    private void PlayBackgroundMusic()
    {
        // 这里假设 AudioManager 是你的背景音乐管理类，并且有一个 PlayBackgroundMusic 方法来播放背景音乐
        AudioManager.Instance.PlayBgmPlayer();
    }
}
