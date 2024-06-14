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
    InputField accountname;//アカウントの名前入力欄
    [SerializeField]
    InputField password;//パスワード入力欄

    private bool isLogout;

    IEnumerator CreateAccount()
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/create_accout", form);
        UnityWebRequest getrequest = UnityWebRequest.Get("http://192.168.56.102:8000/api/create_accout");
        // リクエストを送信して応答を待つ
        yield return request.SendWebRequest();
        yield return getrequest.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("エラー: " + request.error);
        }
        else
        {
            Debug.Log("アカウント作成成功: " + request.downloadHandler.text);
            Debug.Log(getrequest.downloadHandler.text);
        }


    }

    IEnumerator Login()
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/login", form);
        // リクエストを送信して応答を待つ
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ログイン失敗: " + request.error);

        }
        else
        {
            Debug.Log("ログイン成功: " + request.downloadHandler.text);
            SaveAccount();
            StartCoroutine(Get_Game_Info());
        }
    }

    IEnumerator Get_Game_Info()
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.102:8000/api/get_game_info", form);
        // リクエストを送信して応答を待つ
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("データ取得失敗: " + request.error);

        }
        else
        {
            Debug.Log("データ取得完了: " + request.downloadHandler.text);

            // JSON文字列を直接取得
            string json = request.downloadHandler.text;

            // JSON文字列をクラスにデシリアライズする
            JsonGameData playerData = JsonUtility.FromJson<JsonGameData>(json);

            // GameDataにデータをセット
            GameData gameData = new GameData();
            gameData.SetBasicData(playerData);

            Debug.Log(gameData.ToString());

        }
    }


    #region アカウントの保存
    void SaveAccount()
    {
        string filePath = Application.persistentDataPath + "/LoginAccount.json";

        AccountManager accountmanager;
        if (File.Exists(filePath))
        {
            // ファイルが存在する場合はロードして追加
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
            // ファイルが存在しない場合は新規作成
            accountmanager = new AccountManager();
            accountmanager.accountname = accountname.text;
            accountmanager.password = password.text;
            Debug.Log(filePath);
        }

        // JSONファイルに保存
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

