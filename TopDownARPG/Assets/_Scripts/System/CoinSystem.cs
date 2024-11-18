using FrameWork.Utils;
using System.IO;
using TMPro;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    private int _coin = 0; // コイン数
    public int Coin => _coin;
    API _api;

    public void InitCoin(int _datacoin)
    {
        _coin = _datacoin;
    }

    /// <summary>
    /// コインを追加する
    /// </summary>
    /// <param name="value">追加分</param>
    public void AddCoin(int value)
    {
        _coin += value;
        _api = _api = FindObjectOfType<API>();
        string filePath = Application.persistentDataPath + "/LoginAccount.json";
        AccountManager accountmanager = new AccountManager(); // accountmanager を初期化
        if (File.Exists(filePath))
        {
            // ファイルが存在する場合はロードして追加
            string json = File.ReadAllText(filePath);
            accountmanager = JsonUtility.FromJson<AccountManager>(json);

        }
        StartCoroutine(_api.UpDate_Status_String(accountmanager.accountname));
    }
    
    /// <summary>
    /// コインを使用する
    /// </summary>
    /// <param name="value">使用分</param>
    /// <returns>コインが十分か</returns>
    public bool UseCoin(int value)
    {
        if (_coin < value) return false;
        _coin -= value;
        return true;
    }
}