using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStatusComponent : MonoBehaviour
{
    [SerializeField] 
    private PlayerBaseData playerBaseData;                      // Playerの基本数値
    [FormerlySerializedAs("_maxStats")] [SerializeField]
    private Status maxStatus;                                    // Playerの基本最大数値
    [FormerlySerializedAs("_currentStats")] [SerializeField]　
    private Status currentStatus;                                // Playerの基本現在数値
    [FormerlySerializedAs("_maxSpecificStats")] [SerializeField]　
    private PlayerSpecificStatus maxSpecificStatus;              // Player固有ステータス
    [FormerlySerializedAs("_currentSpecificStats")] [SerializeField]　
    private PlayerSpecificStatus currentSpecificStatus;          // Player固有現在ステータス
    
    //ゲッター
    public Status CurrentStatus => currentStatus;
    public PlayerSpecificStatus CurrentSpecificStatus => currentSpecificStatus;
    private void Start()
    {
        // プレイヤの基本数値を取得
        maxStatus = playerBaseData.BaseStatus;
        maxSpecificStatus = playerBaseData.playerSpecificStatus;
        // パワーアップシステムから数値を更新
        PowerUpSystem.Instance.ApplyValues(ref maxStatus, ref maxSpecificStatus);
        
        // 現在数値を最大数値に設定
        currentStatus = maxStatus;
        currentSpecificStatus = maxSpecificStatus;
    }
}
