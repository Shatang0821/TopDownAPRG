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
                //�U��
                enemyStateMachine.ChangeState(RHStateEnum.Attack);
                Debug.Log("�U��");
            }
            
            if(!enemy.TargetFound)
            {
                //�ړ�
                enemyStateMachine.ChangeState(RHStateEnum.Move);
                Debug.Log("�ړ�");
            }
        }
    }
}
