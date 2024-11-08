using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAttackConfig",menuName = "ComboSystem/CreateNewAttackConfig")]
public class AttackConfig : ScriptableObject
{
    public AttackParam AttackParam;
    
    public float StartMoveTime = 0.0f;
    public float StopMoveTime = 0.0f;
    public float Speed = 2.0f;
    public string AnimationName;
}
/// <summary>
/// レイキャストのパラメータ
/// </summary>
[Serializable]
public struct AttackParam
{
    public float Radius;                // 扇型の半径
    public float Angle;                 // 扇型の角度
    public int RayCount;                // Rayの数
    public float RollAngle;             // 扇型のロール角度
    public float RaycastTriggerTime;    //レイを飛ばすタイミング
}