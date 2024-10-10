using System;
using UnityEngine;

public class PlayerStatasComponent : MonoBehaviour
{
    [SerializeField] 
    private PlayerBaseData playerBaseData;                      // Playerの基本数値
    [SerializeField]
    private Stats _maxStats;                                    // Playerの基本最大数値
    [SerializeField]　
    private Stats _currentStats;                                // Playerの基本現在数値
    [SerializeField]　
    private PlayerSpecificStats _maxSpecificStats;              // Player固有ステータス

    [SerializeField]　
    private PlayerSpecificStats _currentSpecificStats;          // Player固有現在ステータス
    
    //ゲッター
    public Stats CurrentStats => _currentStats;
    public PlayerSpecificStats CurrentSpecificStats => _currentSpecificStats;
    private void Start()
    {
        // プレイヤの基本数値を取得
        _maxStats = playerBaseData.BaseStats;
        _maxSpecificStats = playerBaseData.PlayerSpecificStats;
        // パワーアップシステムから数値を更新
        PowerUpSystem.Instance.ApplyValues(ref _maxStats, ref _maxSpecificStats);
        
        // 現在数値を最大数値に設定
        _currentStats = _maxStats;
        _currentSpecificStats = _maxSpecificStats;
    }
}
