using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHMoveState : RHMovementState
{
    public RHMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!enemy.TargetFound)
        {
            enemyStateMachine.ChangeState(RHStateEnum.Idle);
        }
    }
}
