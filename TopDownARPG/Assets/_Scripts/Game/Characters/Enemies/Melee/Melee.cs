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
        //ó‘Ô‚Ì“o˜^ 
        stateMachine.RegisterState(MeleeStateEnum.Idle,new MeleeIdleState("Idle",this,stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Move,new MeleeMoveState("Move",this,stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Damaged,new MeleeDamagedState("Damaged",this,stateMachine));
        stateMachine.RegisterState(MeleeStateEnum.Attack,new MeleeAttackState("Attack",this,stateMachine));
        
        return stateMachine;
    }

    protected override Enum GetInitialState()
    {
        return MeleeStateEnum.Idle;
    }

    public override void TakenDamageState()
    {
        
        enemyStateMachine.ChangeState(MeleeStateEnum.Damaged);
    }

    //ƒ`ƒ“ƒyƒ“’Ç‰Á
    protected override void OnCurrentHealthChanged(float newCurrentHealth)
    {
        base.OnCurrentHealthChanged(newCurrentHealth);

    }
}