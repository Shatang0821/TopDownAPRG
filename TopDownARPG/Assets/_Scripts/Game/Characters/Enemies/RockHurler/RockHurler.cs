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
        //èÛë‘ÇÃìoò^ 
        stateMachine.RegisterState(RHStateEnum.Idle, new MeleeIdleState("Idle", this, stateMachine));
        stateMachine.RegisterState(RHStateEnum.Move, new MeleeMoveState("Move", this, stateMachine));

        return stateMachine;

    }

    protected override Enum GetInitialState()
    {
        return RHStateEnum.Idle;
    }
}
