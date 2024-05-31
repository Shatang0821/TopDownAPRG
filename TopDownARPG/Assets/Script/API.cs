using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class API : MonoBehaviour
{

    [SerializeField]
    InputField accountname;//�A�J�E���g�̖��O���͗�
    [SerializeField]
    InputField password;//�p�X���[�h���͗�


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

