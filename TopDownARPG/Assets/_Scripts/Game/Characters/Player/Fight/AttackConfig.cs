using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackConfig",menuName = "ComboSystem/CreateNewAttackConfig")]
public class AttackConfig : ScriptableObject
{
    public float Radius = 5.0f; // ��^�̔��a
    public float Angle = 45.0f; // ��^�̊p�x
    public int RayCount = 10;   // Ray�̐�
    public float RollAngle = 30.0f; // ��^�̃��[���p�x

    //public float AnimationTime;
}