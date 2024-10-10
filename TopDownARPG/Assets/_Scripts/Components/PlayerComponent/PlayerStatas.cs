using System;
using UnityEngine;

public class PlayerStatas : MonoBehaviour
{
    [SerializeField] private PlayerBaseData playerBaseData;     // Playerの基本数値
    private Stats _maxStats;                                    // Playerの基本最大数値
    private Stats _currentStats;                                // Playerの基本現在数値
    private PlayerSpecificStats _maxSpecificStats;              // Player固有ステータス
    private PlayerSpecificStats _currentSpecificStats;          // Player固有現在ステータス

    private void Start()
    {
        // プレイヤの基本数値を取得
        _maxStats = playerBaseData.BaseStats;
        _maxSpecificStats = playerBaseData.PlayerSpecificStats;
        Debug.Log(_maxStats.Health);
        // パワーアップシステムから数値を更新
        PowerUpSystem.Instance.ApplyValues(ref _maxStats, ref _maxSpecificStats);
        Debug.Log(_maxStats.Health);
        
    }
}
