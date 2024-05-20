using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerBaseState
{
    public float radius = 5.0f; // ��^�̔��a
    public float angle = 45.0f; // ��^�̊p�x
    public int rayCount = 10;   // Ray�̐�
    public float rollAngle = 30.0f; // ��^�̃��[���p�x

    private bool _animetionEnd;
    private bool _canOtherState;
    
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.SetAttackComboCount();
        base.Enter();
        StableRolledFanRayCast(180, 10,45);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_canOtherState)
        {
            if (player.Attack)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Attack);
                return;
            }
            if (player.Axis != Vector2.zero)
            {
                player.ComboConfig.ComboCount = 0;
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
                return;
            }
        }

        if (_animetionEnd)
        {
            player.ComboConfig.ComboCount = 0;
            if (player.Axis != Vector2.zero)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
            }
            else
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
            }
        }
    }

    public override void AnimationEventCalled()
    {
        base.AnimationEventCalled();
        //_canOtherState
        //playerStateMachine.ChangeState(PlayerStateEnum.Idle);
        _canOtherState = true;
    }

    public override void AnimationEndCalled()
    {
        base.AnimationEndCalled();
        _animetionEnd = true;
    }

    public override void Exit()
    {
        base.Exit();
        _canOtherState = false;
        _animetionEnd = false;
    }

    private void StableRolledFanRayCast(float angle, int rayCount, float rollAngle)
    {
        Vector3 forward = player.RayStartPoint.forward;
        Vector3 origin = player.RayStartPoint.position;

        // ��^�̊e�|�C���g���v�Z
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ���[���h��Ԃł̃��[���p�x�̓K�p
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, player.RayStartPoint.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // Ray���΂��ďՓ˔���
        foreach (var point in fanPoints)
        {
            Vector3 direction = (point - origin).normalized;
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, radius))
            {
                Debug.Log("Hit: " + hit.collider.name);
                // �Փˏ�����ǉ�
            }

            // �f�o�b�O�p��Ray��`��
            Debug.DrawRay(origin, direction * radius, Color.red);
        }
    }



}