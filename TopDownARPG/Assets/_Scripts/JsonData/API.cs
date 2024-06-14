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
    InputField accountname;//�A�J�E���g�̖��O���͗�
    [SerializeField]
    InputField password;//�p�X���[�h���͗�

    private bool isLogout;

    IEnumerator CreateAccount()
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

    IEnumerator Login()
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
            SaveAccount();
            StartCoroutine(Get_Game_Info());
        }
    }

    IEnumerator Get_Game_Info()
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
            Debug.Log("�f�[�^�擾����: " + request.downloadHandler.text);

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
    void SaveAccount()
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
                Debug.Log("a");
            }
            else
            {
                accountmanager.accountname = accountname.text;
                accountmanager.password = password.text;
                Debug.Log("b");
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


    public void CreateAccountButton()
    {
        StartCoroutine(CreateAccount());
    }

    public void LoginButton()
    {
        isLogout = false;
        StartCoroutine(Login());
    }

    public void LogoutButton()
    {
        isLogout = true;
        SaveAccount();
        isLogout = false;
    }

}

[System.Serializable]
public class AccountManager
{
    public string accountname;
    public string password;
}

