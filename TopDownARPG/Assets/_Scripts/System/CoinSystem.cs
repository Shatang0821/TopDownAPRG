using FrameWork.Utils;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    private int _coin = 500; // コイン数
    public int Coin => _coin;
    
    public void AddCoin(int value)
    {
        _coin += value;
    }
    
    public void UseCoin(int value)
    {
        _coin -= value;
    }
}