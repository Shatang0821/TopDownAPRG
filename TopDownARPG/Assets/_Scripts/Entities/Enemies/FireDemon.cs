using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine = FrameWork.FSM.StateMachine;

public enum FDStateEnum
{
    Idle,
    Move,
    Attack,
    Damaged,
    Die
}

public class FireDemon : Enemy
{
    private MovementComponent _movementComponent;
    public AttackComponent AttackComponent;

    protected override void Awake()
    {
        base.Awake();
        _movementComponent = GetComponent<MovementComponent>();
        AttackComponent = GetComponent<AttackComponent>();

        
    }
    protected override StateMachine CreateStateMachine()
    {
        var stateMachine = new EnemyStateMachine(this);
        //状態の登録 
        stateMachine.RegisterState(FDStateEnum.Idle, new MeleeIdleState("Idle", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Move, new MeleeMoveState("Move", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Damaged, new MeleeDamagedState("Damaged", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Attack, new MeleeAttackState("Attack", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Die, new MeleeDieState("Die", this, stateMachine));
        return stateMachine;
    }
    
    
    /// <summary>
    /// アニメーションイベント
    /// </summary>
    private void AnimationEventCalled()
    {
        enemyStateMachine.AnimationEventCalled();
    }

    private void AnimationEndCalled()
    {
        enemyStateMachine.AnimationEndCalled();
    }

    protected override Enum GetInitialState()
    {
        return FDStateEnum.Idle;
    }
    

    public override void Move(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            _movementComponent.Move(dir, speed.Value, 0.2f);
        }
    }
    
    public override void StopMove()
    {
        Rigidbody.velocity = Vector3.zero;
    }
    
    /// <summary>
    /// ターゲット方向に回転
    /// </summary>
    /// <param name="targetDirection">ターゲット方向</param>
    /// <param name="rotationSpeed">回転速度</param>
    public void Rotation(Vector3 targetDirection, float rotationSpeed)
    {
        targetDirection.y = 0;
        if (targetDirection.magnitude > 0.1f)
        {
            _movementComponent.RotateTowards(transform, targetDirection, rotationSpeed);
        }
    }

    public override void TakenDamageState()
    {
        enemyStateMachine.ChangeState(FDStateEnum.Damaged);
    }
}