
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseData", menuName = "PlayerBaseData")]
public class PlayerBaseData : EntityBaseData
{  
    public PlayerSpecificStats PlayerSpecificStats; // プレイヤ固有ステータス
}

/// <summary>
/// プレイヤ固有ステータス
/// </summary>
[Serializable]
public struct PlayerSpecificStats
{
    public float MpRecoverySpeed; // MP回復速度
    public float DashCoolTime;    // ダッシュクールタイム
}
