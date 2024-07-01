using FrameWork.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RHStateEnum
{
    Idle,
    Move,
    Attack,
    Damaged,
    Die
}

public class RockHurler : Enemy
{

 

    protected override StateMachine CreateStateMachine()
    {
        var stateMachine = new EnemyStateMachine(this);
        //��Ԃ̓o�^ 
        stateMachine.RegisterState(RHStateEnum.Idle, new RHIdleState("Idle", this, stateMachine));
        stateMachine.RegisterState(RHStateEnum.Move, new RHMoveState("Move", this, stateMachine));
        stateMachine.RegisterState(RHStateEnum.Attack, new RHAttackState("Attack", this, stateMachine));
        //stateMachine.RegisterState(RHStateEnum.Damaged, new RHDamagedState("Damaged", this, stateMachine));

        return stateMachine;

    }

    protected override Enum GetInitialState()
    {
        return RHStateEnum.Idle;
    } 
    
    public override void TakenDamageState()
    {
        enemyStateMachine.ChangeState(RHStateEnum.Damaged);
    }

    private void AnimationEventCalled()
    {
        enemyStateMachine.AnimationEventCalled();
    }


}
