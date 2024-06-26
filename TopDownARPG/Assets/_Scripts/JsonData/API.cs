using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.UI;

public class API : MonoBehaviour
{

    [SerializeField]
    InputField accountname;

    [SerializeField]
    InputField password;
    private bool isLogout;

    IEnumerator CreateAccount(InputField accountname,InputField password)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POST���N�G�X�g���쐬
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/create_accout", form);
        UnityWebRequest getrequest = UnityWebRequest.Get("http://192.168.56.102:8000/api/create_accout");
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();
        yield return getrequest.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�G���[: " + request.error);
        }
        else
        {
            Debug.Log("�A�J�E���g�쐬����: " + request.downloadHandler.text);
            Debug.Log(getrequest.downloadHandler.text);
        }


    }

    IEnumerator Login(InputField accountname, InputField password)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POST���N�G�X�g���쐬
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/login", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���O�C�����s: " + request.error);

        }
        else
        {
            Debug.Log("���O�C������: " + request.downloadHandler.text);
            SaveAccount(accountname,password);
            StartCoroutine(Get_Game_Info(accountname));
        }
    }

    IEnumerator Get_Game_Info(InputField accountname)
    {
        // �t�H�[���f�[�^���쐬
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        // POST���N�G�X�g���쐬
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/get_game_info", form);
        // ���N�G�X�g�𑗐M���ĉ�����҂�
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�f�[�^�擾���s: " + request.error);

        }
        else
        {
            Debug.Log("�f�[�^�擾���� ");

            // JSON������𒼐ڎ擾
            string json = request.downloadHandler.text;

            // JSON��������N���X�Ƀf�V���A���C�Y����
            JsonGameData playerData = JsonUtility.FromJson<JsonGameData>(json);

            // GameData�Ƀf�[�^���Z�b�g
            GameData gameData = new GameData();
            gameData.SetBasicData(playerData);

            Debug.Log(gameData.ToString());

        }
    }


    #region �A�J�E���g�̕ۑ�
    void SaveAccount(InputField accountname, InputField password)
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

