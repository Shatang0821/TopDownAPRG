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

    public bool isLogout;
    public bool isLogin = false;

    DataManager _dataManager; //データマネージャーのインスタンス

    public IEnumerator CreateAccount(TMP_InputField accountname,TMP_InputField password)
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/create_accout", form);
        UnityWebRequest getrequest = UnityWebRequest.Get("http://192.168.56.104:8000/api/create_accout");
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
            //作ったアカウントでログイン処理を行う
            StartCoroutine(Login(accountname,password));
        }


    }

    public IEnumerator Login(TMP_InputField accountname, TMP_InputField password)
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        form.AddField("password", password.text);

        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/login", form);
        // リクエストを送信して応答を待つ
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ログイン失敗: " + request.error);
            isLogin = false;
        }
        else
        {
            Debug.Log("ログイン成功: " + request.downloadHandler.text);
            //アカウントの保存
            SaveAccount(accountname,password);
            //一緒にゲームの情報を持ってくる
            StartCoroutine(Get_Game_Info(accountname));
            isLogin = true;
        }
    }

    IEnumerator Get_Game_Info(TMP_InputField accountname)
    {
        // フォームデータを作成
        WWWForm form = new WWWForm();
        form.AddField("user_name", accountname.text);
        // POSTリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.56.104:8000/api/get_game_info", form);
        // リクエストを送信して応答を待つ
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("データ取得失敗: " + request.error);

        }
        else
        {
            Debug.Log("データ取得完了 ");

            _dataManager = FindObjectOfType<DataManager>();
            // JSON文字列を直接取得
            string json = request.downloadHandler.text;

            // JSON文字列をクラスにデシリアライズする
            JsonGameData playerData = JsonUtility.FromJson<JsonGameData>(json);

            _dataManager.GameDataSet(playerData);
        }
    }


    #region アカウントの保存
    public void SaveAccount(TMP_InputField accountname, TMP_InputField password)
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
                Debug.Log("ログアウト");
            }
            else
            {
                accountmanager.accountname = accountname.text;
                accountmanager.password = password.text;
                Debug.Log("保存成功");
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

    //保存したJsonデータからログアウト出来るように引数がstring型のを作った
    public void StringSaveAccount(string accountname, string password)
    {
        string filePath = Application.persistentDataPath + "/LoginAccount.json";

        AccountManager accountmanager = new AccountManager(); // accountmanager を初期化

        if (File.Exists(filePath))
        {
            // ファイルが存在する場合はロードして追加
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);
            if (isLogout)
            {
                accountmanager.accountname = null;
                accountmanager.password = null;
                Debug.Log("ログアウト");
            }
            else
            {
                Debug.Log("保存成功");
            }

            Debug.Log(filePath);
        }
        else
        {
            Debug.Log(filePath);
        }

        // JSONファイルに保存
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
            // レスポンスの処理
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Server response: " + jsonResponse);

            // ログアウトのシグナルを受信した場合
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

