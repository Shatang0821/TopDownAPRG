using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHDamageState : RHMovementState
{
    public RHDamageState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.IsTakenDamaged = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(enemy.GetCurrentHealth <= 0 )
        {
            enemyStateMachine.ChangeState(RHStateEnum.Die);
            return;
        }

        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }

        if (stateTimer > 0.5f)
        {

            if (enemy.InAttackRange)
            {
                if (enemy.TargetFound)
                {
                    enemyStateMachine.ChangeState(RHStateEnum.Move);
                }
                else
                {
                    enemyStateMachine.ChangeState(RHStateEnum.Attack);
                }
            }
            else
            {
                enemyStateMachine.ChangeState(RHStateEnum.Idle);
            }


        }
    }
}
