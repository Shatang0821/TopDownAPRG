using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackConfig",menuName = "ComboSystem/CreateNewAttackConfig")]
public class AttackConfig : ScriptableObject
{
    public float Radius = 5.0f; // îŒ^‚Ì”¼Œa
    public float Angle = 45.0f; // îŒ^‚ÌŠp“x
    public int RayCount = 10;   // Ray‚Ì”
    public float RollAngle = 30.0f; // îŒ^‚Ìƒ[ƒ‹Šp“x

    //public float AnimationTime;
}