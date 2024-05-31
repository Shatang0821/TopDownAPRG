using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class API : MonoBehaviour
{

    [SerializeField]
    InputField accountname;//アカウントの名前入力欄
    [SerializeField]
    InputField password;//パスワード入力欄


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
        }
    }

    public void CreateAccountButton()
    {
        StartCoroutine(CreateAccount());
    }

    public void LoginButton()
    {
        StartCoroutine(Login());
    }



}

