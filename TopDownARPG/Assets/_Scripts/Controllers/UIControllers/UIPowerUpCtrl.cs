using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.UI;
using FrameWork.Audio;
using UnityEngine.EventSystems;
using System;

public class UIPowerUpCtrl : UICtrl
{
    private Image _blackImage; // Black Image を格納するための変数

    [SerializeField]
    private PowerUpData _powerUpData; // パワーアップデータベース

    public override void Awake()
    {
        base.Awake();
        _blackImage = View["Black"].GetComponent<Image>(); // Black Image を取得

        // 1秒後にBlack Imageを非表示にするコルーチンを開始
        StartCoroutine(HideBlackImageAfterDelay());

        Debug.Log("Power-Up Name: " + _powerUpData.powerUpName);
        Debug.Log("Description: " + _powerUpData.description);
    }

    // 1秒後にBlack Imageを非表示にするコルーチン
    private IEnumerator HideBlackImageAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _blackImage.gameObject.SetActive(false);
    }
}

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData : ScriptableObject
{
    public string powerUpName; // パワーアップの名前
    public string description; // パワーアップの詳細
}
