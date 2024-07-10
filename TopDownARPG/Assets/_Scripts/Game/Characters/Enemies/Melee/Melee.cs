using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine = FrameWork.FSM.StateMachine;

public enum MeleeStateEnum
{
    Idle,
    Move,
    Attack,
    Damaged,
    Die
}

public class Melee : Enemy
{
    private MovementComponent _movementComponent;
    public AttackComponent AttackComponent;

    protected override void Awake()
    {
        base.Awake();
        _movementComponent = new MovementComponent(Rigidbody, transform);
        AttackComponent = GetComponent<AttackComponent>();

        speed = new Observer<float>(2);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Debug.Log(DistanceToPlayer);
    }

    protected override StateMachine CreateStateMachine()
    {
        var stateMachine = new EnemyStateMachine(this);
        //状態の登録 
        stateMachine.RegisterState(MeleeStateEnum.Idle, new MeleeIdleState("Idle", this, stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Move, new MeleeMoveState("Move", this, stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Damaged, new MeleeDamagedState("Damaged", this, stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Attack, new MeleeAttackState("Attack", this, stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Die, new MeleeDieState("Die", this, stateMachine));
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
        return MeleeStateEnum.Idle;
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
        enemyStateMachine.ChangeState(MeleeStateEnum.Damaged);
    }
}