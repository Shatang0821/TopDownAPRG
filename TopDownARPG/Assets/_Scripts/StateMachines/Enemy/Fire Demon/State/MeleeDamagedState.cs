using UnityEngine;

public class MeleeDamagedState : EnemyBaseState
{
    public MeleeDamagedState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
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
        if(enemy.GetCurrentHealth <= 0)
        {
            
            enemyStateMachine.ChangeState(FDStateEnum.Die);
            return;
        }
        
        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }
        
        if (stateTimer > 0.5f)
        {
            
            if (enemy.TargetFound)
            {
                if (enemy.InAttackRange)
                {
                    enemyStateMachine.ChangeState(FDStateEnum.Attack);
                    return;
                }
                else
                {
                    enemyStateMachine.ChangeState(FDStateEnum.Move);
                    return;
                }
            }
            else
            {
                enemyStateMachine.ChangeState(FDStateEnum.Idle);
                return;
            }
            
            
           
            
        }
        
    }
}