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
/// ���C�L���X�g�̃p�����[�^
/// </summary>
[Serializable]
public struct AttackParam
{
    public float Radius;                // ��^�̔��a
    public float Angle;                 // ��^�̊p�x
    public int RayCount;                // Ray�̐�
    public float RollAngle;             // ��^�̃��[���p�x
    public float RaycastTriggerTime;    //���C���΂��^�C�~���O
}