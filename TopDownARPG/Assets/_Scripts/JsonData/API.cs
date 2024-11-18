using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class API : MonoBehaviour
{

    [SerializeField]
    TMP_InputField accountname;

    [SerializeField]
    TMP_InputField password;

    [SerializeField]
    //Text _username;

    public bool isLogout;
    public bool isLogin = false;
    public bool isBadrequest = false;

    DataManager _dataManager; //�f�[�^�}�l�[�W���[�̃C���X�^���X
    UILoginCtrl _loginCtrl;

    public IEnumerator CreateAccount(TMP_InputField accountname,TMP_InputField password)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POST���N�G�X�g���쐬
        UnityWebRequest request = UnityWebRequest.Post("http://10.22.53.100/r06/3n/ARPGDataManagement/api/create_accout", form);
        UnityWebRequest getrequest = UnityWebRequest.Get("http://10.22.53.100/r06/3n/ARPGDataManagement/api/create_accout");
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();
        yield return getrequest.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�G���[: " + request.error);
            isBadrequest = true;
        }
        else
        {
            Debug.Log("�A�J�E���g�쐬����: " + request.downloadHandler.text);
            Debug.Log(getrequest.downloadHandler.text);
            //������A�J�E���g�Ń��O�C���������s��
            StartCoroutine(Login(accountname,password));
            isBadrequest=false;
        }


    }

    public IEnumerator Login(TMP_InputField accountname, TMP_InputField password)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POST���N�G�X�g���쐬
        //UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/login", form);
        UnityWebRequest request = UnityWebRequest.Post("http://10.22.53.100/r06/3n/ARPGDataManagement/api/login", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���O�C�����s: " + request.error);
            isLogin = false;
        }
        else
        {
            Debug.Log("���O�C������: " + request.downloadHandler.text);
            _loginCtrl = FindObjectOfType<UILoginCtrl>();
            //�A�J�E���g�̕ۑ�
            SaveAccount(accountname,password);
            //�ꏏ�ɃQ�[���̏��������Ă���
            StartCoroutine(Get_Game_Info(accountname));
            isLogin = true;

            StartCoroutine(_loginCtrl.SignIn());
            
            //_username.text ="name�F" + accountname.text;
        }
    }

    IEnumerator Get_Game_Info(TMP_InputField accountname)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        // POST���N�G�X�g���쐬
        //UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/get_game_info", form);
        UnityWebRequest request = UnityWebRequest.Post("http://10.22.53.100/r06/3n/ARPGDataManagement/api/get_game_info", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�f�[�^�擾���s: " + request.error);

        }
        else
        {
            Debug.Log("�f�[�^�擾���� ");

            _dataManager = FindObjectOfType<DataManager>();
            // JSON������𒼐ڎ擾
            string json = request.downloadHandler.text;

            // JSON��������N���X�Ƀf�V���A���C�Y����
            JsonGameData playerData = JsonUtility.FromJson<JsonGameData>(json);

            _dataManager.GameDataSet(playerData);

            //StartCoroutine(UpDate_Status(accountname));

            PowerUpSystem.Instance.SetSavedValues(_dataManager.maxhealth, _dataManager.mp, _dataManager.power, _dataManager.defense, _dataManager.speed, _dataManager.dashcooltime);
            CoinSystem.Instance.InitCoin(_dataManager.coin);
        }
    }
    
    public IEnumerator UpDate_Status(TMP_InputField accountname)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("maxhealth", _dataManager.maxhealth);
        form.AddField("power", _dataManager.power);
        form.AddField("defense", _dataManager.defense);
        form.AddField("speed", _dataManager.speed);
        form.AddField("mp", _dataManager.mp);
        form.AddField("dashcooltime", _dataManager.dashcooltime);
        form.AddField("coin", _dataManager.coin);

        Debug.Log(_dataManager.maxhealth);
        // POST���N�G�X�g���쐬
        //UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/update_status", form);
        UnityWebRequest request = UnityWebRequest.Post("http://10.22.53.100/r06/3n/ARPGDataManagement/api/update_status", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�A�b�v�f�[�g���s: " + request.error);

        }
        else
        {
            Debug.Log("�A�b�v�f�[�g���� ");

        }
    }

    public IEnumerator UpDate_Status_String(string accountname)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname);
        form.AddField("maxhealth", _dataManager.maxhealth);
        form.AddField("power", _dataManager.power);
        form.AddField("defense", _dataManager.defense);
        form.AddField("speed", _dataManager.speed);
        form.AddField("mp", _dataManager.mp);
        form.AddField("dashcooltime", _dataManager.dashcooltime);
        form.AddField("coin", CoinSystem.Instance.Coin);

        Debug.Log(_dataManager.maxhealth);
        // POST���N�G�X�g���쐬
        //UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/update_status", form);
        UnityWebRequest request = UnityWebRequest.Post("http://10.22.53.100/r06/3n/ARPGDataManagement/api/update_status", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�A�b�v�f�[�g���s: " + request.error);

        }
        else
        {
            Debug.Log("�A�b�v�f�[�g���� ");

        }
    }
    #region �A�J�E���g�̕ۑ�
    public void SaveAccount(TMP_InputField accountname, TMP_InputField password)
    {
        string filePath = Application.persistentDataPath + "/LoginAccount.json";

        AccountManager accountmanager;
        if (File.Exists(filePath))
        {
            // �t�@�C�������݂���ꍇ�̓��[�h���Ēǉ�
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);
            if (isLogout)
            {
                accountmanager.accountname = null;
                accountmanager.password = null;
                
                Debug.Log("���O�A�E�g");
            }
            else
            {
                accountmanager.accountname = accountname.text;
                accountmanager.password = password.text;
                Debug.Log("�ۑ�����");
            }

            Debug.Log(filePath);
        }
        else
        {
            // �t�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
            accountmanager = new AccountManager();
            accountmanager.accountname = accountname.text;
            accountmanager.password = password.text;
            Debug.Log(filePath);
        }

        // JSON�t�@�C���ɕۑ�
        string AccountsJson = JsonUtility.ToJson(accountmanager, true);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(AccountsJson);
        streamWriter.Flush();
        streamWriter.Close();
    }
    #endregion

    //�ۑ�����Json�f�[�^���烍�O�A�E�g�o����悤�Ɉ�����string�^�̂������
    public void StringSaveAccount(string accountname, string password)
    {
        string filePath = Application.persistentDataPath + "/LoginAccount.json";

        AccountManager accountmanager = new AccountManager(); // accountmanager ��������

        if (File.Exists(filePath))
        {
            // �t�@�C�������݂���ꍇ�̓��[�h���Ēǉ�
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);
            if (isLogout)
            {
                accountmanager.accountname = null;
                accountmanager.password = null;
                //_username.text = "name�F";
                Debug.Log("���O�A�E�g");
            }
            else
            {
                Debug.Log("�ۑ�����");
            }

            Debug.Log(filePath);
        }
        else
        {
            Debug.Log(filePath);
        }

        // JSON�t�@�C���ɕۑ�
        string AccountsJson = JsonUtility.ToJson(accountmanager, true);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(AccountsJson);
        streamWriter.Flush();
        streamWriter.Close();
    }


    public void CheckSessionStatus()
    {
        StartCoroutine(CheckSessionCoroutine());
    }

    IEnumerator CheckSessionCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://example.com/check-session");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ���X�|���X�̏���
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Server response: " + jsonResponse);

            // ���O�A�E�g�̃V�O�i������M�����ꍇ
            if (jsonResponse.Contains("logout"))
            {
                isLogout = true;
                SaveAccount(accountname, password);
                isLogout = false;
            }
        }
        else
        {
            Debug.LogError("Failed to check session status: " + request.error);
        }
    }


    public void CreateAccountButton()
    {
        StartCoroutine(CreateAccount(accountname,password));
    }

    public void LoginButton()
    {
        isLogout = false;
        StartCoroutine(Login(accountname,password));
    }

    public void LogoutButton()
    {
        isLogout = true;
        SaveAccount(accountname,password);
        isLogout = false;
    }

}

[System.Serializable]
public class AccountManager
{
    public string accountname;
    public string password;
}

