using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using TMPro;
using System.Collections;
using System.IO;

public class UILoginCtrl : UICtrl
{
    API _api;
    public override void Awake()
    {
        base.Awake();

        _api = FindObjectOfType<API>();
        string filePath = Application.persistentDataPath + "/LoginAccount.json";
        AccountManager accountmanager = new AccountManager(); // accountmanager を初期化
        if (File.Exists(filePath))
        {
            // ファイルが存在する場合はロードして追加
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);
            var accountname = View["Account"].GetComponent<TMP_InputField>();
            var password = View["Password"].GetComponent<TMP_InputField>();
            accountname.text = accountmanager.accountname;
            password.text = accountmanager.password;
        }

        AddButtonListener("Register", Register);
        AddButtonListener("SingIn", SingIn);
        AddButtonListener("RegistrationScreenPanel/Complete", Complete);
        //AddButtonListener("RegistrationScreenPanel/Back", Back);
        View["RegistrationScreenPanel"].SetActive(false);

        AddButtonHoverEffect("Register");
        AddButtonHoverEffect("SingIn");
        AddButtonHoverEffect("RegistrationScreenPanel/Complete");
        //AddButtonHoverEffect("RegistrationScreenPanel/Back");

        if (!AudioManager.Instance.IsBgmPlaying())
        {
            AudioManager.Instance.PlayBgmPlayer();
        }
        AudioManager.Instance.StopAllNonBgmPlayers();
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
        var accountname = View["Account"].GetComponent<TMP_InputField>();
        var password = View["Password"].GetComponent<TMP_InputField>();
        StartCoroutine(_api.Login(accountname, password));
       
        
    }

    private void Complete()
    {
        var accountname = View["RegistrationScreenPanel/Account"].GetComponent<TMP_InputField>();
        var password = View["RegistrationScreenPanel/Password (1)"].GetComponent<TMP_InputField>();
        StartCoroutine(_api.CreateAccount(accountname, password));
        Debug.Log("Complete");

        if(!_api.isBadrequest)
        {
            StartCoroutine(SignIn());
        }

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

    public IEnumerator SignIn()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("SingIn");
        UIManager.Instance.RemoveUI("UILogin");
        UIManager.Instance.ShowUI("UIHome");
        UIManager.Instance.ChangeUIPrefab("UIHome");
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
