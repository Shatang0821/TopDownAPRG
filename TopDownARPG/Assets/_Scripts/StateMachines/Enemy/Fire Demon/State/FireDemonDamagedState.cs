using FrameWork.Resource;
using UnityEngine;

public class FireDemonDamagedState : FireDemonBaseState
{
    private MovementComponent _movementComponent;
    private float _moveDuration = 0.1f;
    private float _moveSpeed = 100.0f;
    public FireDemonDamagedState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<FireDemonStateConfig>(stateConfigPath + "FireDemon/FireDemonDamaged_Config");
        _movementComponent = enemy.GetComponent<MovementComponent>();
        if(!_movementComponent)Debug.LogWarning("Move Component is null");
    }

    public override void Enter()
    {
        base.Enter();
        enemy.IsTakenDamaged = false;
        enemy.ApplyKnockback();;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(enemy.GetCurrentHealth <= 0)
        {
            
            ChangeState(FDStateEnum.Die);
            return;
        }
        
        if (enemy.IsTakenDamaged)
        {
            ChangeState(FDStateEnum.Damaged);
            return;
        }
        
        if (stateTimer > 0.5f)
        {
            
            if (enemy.TargetFound)
            {
                if (enemy.InAttackRange)
                {
                    ChangeState(FDStateEnum.Attack);
                    return;
                }
                else
                {
                    ChangeState(FDStateEnum.Move);
                    return;
                }
            }
            else
            {
                ChangeState(FDStateEnum.Idle);
                return;
            }
            
            
           
            
        }
        
    }
}