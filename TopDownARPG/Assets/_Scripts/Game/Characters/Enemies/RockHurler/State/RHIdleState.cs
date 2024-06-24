using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHIdleState : MeleeMovementState
{
    public RHIdleState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.TargetFound)
        {
            if (enemy.InAttackRange)
            {
                //UŒ‚
                enemyStateMachine.ChangeState(RHStateEnum.Attack);
                Debug.Log("UŒ‚");
            }
            
            if(!enemy.TargetFound)
            {
                //ˆÚ“®
                enemyStateMachine.ChangeState(RHStateEnum.Move);
                Debug.Log("ˆÚ“®");
            }
        }
    }
}
