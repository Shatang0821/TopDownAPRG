using FrameWork.Utils;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    private int _coin = 500; // コイン数
    public int Coin => _coin;
    
    /// <summary>
    /// コインを追加する
    /// </summary>
    /// <param name="value">追加分</param>
    public void AddCoin(int value)
    {
        _coin += value;
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