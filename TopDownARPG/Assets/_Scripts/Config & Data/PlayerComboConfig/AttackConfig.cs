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
/// CLXgΜp[^
/// </summary>
[Serializable]
public struct AttackParam
{
    public float Radius;                // ξ^ΜΌa
    public float Angle;                 // ξ^Μpx
    public int RayCount;                // RayΜ
    public float RollAngle;             // ξ^Μ[px
    public float RaycastTriggerTime;    //CπςΞ·^C~O
}