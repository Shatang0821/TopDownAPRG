using System.Collections;
using System.Collections.Generic;
using FrameWork.Resource;
using UnityEngine;

public class RHDamagedState : RHMovementState
{
    public RHDamagedState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<RockHurlerStateConfig>(stateConfigPath + "RockHurler/RockHurlerDamaged_Config");
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
            ChangeState(RHStateEnum.Die);
            return;
        }

        if (enemy.IsTakenDamaged)
        {
            ChangeState(RHStateEnum.Damaged);
            return;
        }

        if (stateTimer > 0.5f)
        {

            if (enemy.InAttackRange)
            {
                if (enemy.TargetFound)
                {
                    ChangeState(RHStateEnum.Move);
                }
                else
                {
                    ChangeState(RHStateEnum.Attack);
                }
            }
            else
            {
                ChangeState(RHStateEnum.Idle);
            }


        }
    }
}
