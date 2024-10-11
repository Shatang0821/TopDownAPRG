
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerBaseData", menuName = "PlayerBaseData")]
public class PlayerBaseData : EntityBaseData
{  
    public PlayerSpecificStatus playerSpecificStatus; // プレイヤ固有ステータス
}

/// <summary>
/// プレイヤ固有ステータス
/// </summary>
[Serializable]
public struct PlayerSpecificStatus
{
    public float MpRecoverySpeed; // MP回復速度
    public float DashCoolTime;    // ダッシュクールタイム
}
