using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.FSM;
using UnityEngine;

public enum MeleeStateEnum{
    Idle,
    Move,
    Attack,
    Damaged,
    Die
}

public class Melee : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override StateMachine CreateStateMachine()
    {
        var stateMachine = new EnemyStateMachine(this);
        //èÛë‘ÇÃìoò^ 
        stateMachine.RegisterState(MeleeStateEnum.Idle,new MeleeIdleState("Idle",this,stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Move,new MeleeMoveState("Move",this,stateMachine));
        
        return stateMachine;
    }

    protected override Enum GetInitialState()
    {
        return MeleeStateEnum.Idle;
    }
}