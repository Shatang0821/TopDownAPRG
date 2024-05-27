using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerBaseState
{
    private bool _animetionEnd;
    private bool _canOtherState;
    private ComboConfig _comboConfig;
    private AttackConfig _attackConfig;
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.SetAttackComboCount();
        base.Enter();
        
        _comboConfig = player.ComboConfig;
        _attackConfig = _comboConfig.AttackConfigs[_comboConfig.ComboCount - 1];
        StableRolledFanRayCast(_attackConfig.Angle, _attackConfig.RayCount,_attackConfig.RollAngle,_attackConfig.Radius);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        var comboConfig = player.ComboConfig;
        var attackConfig = comboConfig.AttackConfigs[comboConfig.ComboCount-1];
        StableRolledFanRayCast(attackConfig.Angle, attackConfig.RayCount,attackConfig.RollAngle,attackConfig.Radius);
        if (player.Damaged)
        {
            player.ComboConfig.ComboCount = 0;
            playerStateMachine.ChangeState(PlayerStateEnum.Damaged);
            return;
        }
        if (_canOtherState)
        {
            if (player.Attack && comboConfig.ComboCount < comboConfig.AttackConfigs.Count)
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateTimer > _attackConfig.StartMoveTime && stateTimer < _attackConfig.StopMoveTime)
        {
            MovePlayer();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _canOtherState = false;
        _animetionEnd = false;
    }

    private void StableRolledFanRayCast(float angle, int rayCount, float rollAngle,float radius)
    {
        Vector3 forward = player.RayStartPoint.forward;
        Vector3 origin = player.RayStartPoint.position;

        // 扇型の各ポイントを計算
        List<Vector3> fanPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -angle / 2 + (angle / rayCount) * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
            fanPoints.Add(origin + direction * radius);
        }

        // ワールド空間でのロール角度の適用
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, player.RayStartPoint.forward);
        for (int i = 0; i < fanPoints.Count; i++)
        {
            Vector3 localPoint = fanPoints[i] - origin;
            fanPoints[i] = origin + rollRotation * localPoint;
        }

        // Rayを飛ばして衝突判定
        foreach (var point in fanPoints)
        {
            Vector3 direction = (point - origin).normalized;
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, radius))
            {
                Debug.Log("Hit: " + hit.collider.name);
                // 衝突処理を追加
            }

            // デバッグ用のRayを描画
            Debug.DrawRay(origin, direction * radius, Color.red);
        }
    }

    private void MovePlayer()
    {
        player.Rigidbody.AddForce(player.transform.forward * _attackConfig.Speed,
            ForceMode.VelocityChange);
    }
    
    

}