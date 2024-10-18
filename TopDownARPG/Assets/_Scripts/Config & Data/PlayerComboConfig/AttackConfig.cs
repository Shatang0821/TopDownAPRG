using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAttackConfig",menuName = "ComboSystem/CreateNewAttackConfig")]
public class AttackConfig : ScriptableObject
{
    public float Radius = 5.0f; // ξ^ΜΌa
    public float Angle = 45.0f; // ξ^Μpx
    public int RayCount = 10;   // RayΜ
    public float RollAngle = 30.0f; // ξ^Μ[px

    public float StartMoveTime = 0.0f;
    public float StopMoveTime = 0.0f;
    public float Speed = 2.0f;

    public float RaycastTriggerTime = 0.2f;  //CπςΞ·^C~O
}