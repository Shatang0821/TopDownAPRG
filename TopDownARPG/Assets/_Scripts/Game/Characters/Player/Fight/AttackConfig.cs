using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackConfig",menuName = "ComboSystem/CreateNewAttackConfig")]
public class AttackConfig : ScriptableObject
{
    public float Radius = 5.0f; // ξ^ΜΌa
    public float Angle = 45.0f; // ξ^Μpx
    public int RayCount = 10;   // RayΜ
    public float RollAngle = 30.0f; // ξ^Μ[px

    //public float AnimationTime;
}