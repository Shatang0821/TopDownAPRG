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

    public AttackComponent AttackComponent;

    protected override void Awake()
    {
        base.Awake();
        AttackComponent = GetComponent<AttackComponent>();

        
    }
    protected override StateMachine CreateStateMachine()
    {
        var stateMachine = new EnemyStateMachine(this);
        //��Ԃ̓o�^ 
        stateMachine.RegisterState(FDStateEnum.Idle, new FireDemonIdleState("Idle", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Move, new FireDemonMoveState("Move", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Damaged, new FireDemonDamagedState("Damaged", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Attack, new FireDemonAttackState("Attack", this, stateMachine));
        stateMachine.RegisterState(FDStateEnum.Die, new FireDemonDieState("Die", this, stateMachine));
        return stateMachine;
    }
    
    protected override Enum GetInitialState()
    {
        return FDStateEnum.Idle;
    }
    

    public override void Move(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            movementComponent.Move(dir, speed.Value,true, 0.2f);
        }
    }
    
    public override void StopMove()
    {
        Rigidbody.velocity = Vector3.zero;
    }
    
    /// <summary>
    /// �^�[�Q�b�g�����ɉ�]
    /// </summary>
    /// <param name="targetDirection">�^�[�Q�b�g����</param>
    /// <param name="rotationSpeed">��]���x</param>
    public void Rotation(Vector3 targetDirection, float rotationSpeed)
    {
        targetDirection.y = 0;
        if (targetDirection.magnitude > 0.1f)
        {
            movementComponent.RotateTowards(transform, targetDirection, rotationSpeed);
        }
    }
}