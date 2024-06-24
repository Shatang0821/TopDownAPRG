using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHMoveState : MeleeMovementState
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
        if (enemy.TargetFound)
        {
            if (enemy.InAttackRange)
            {
                //�U��
                enemyStateMachine.ChangeState(RHStateEnum.Attack);
            }
            else
            {
                //�ړ�
                enemyStateMachine.ChangeState(RHStateEnum.Idle);
            }
        }
    }
}
