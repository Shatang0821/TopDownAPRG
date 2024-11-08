using UnityEngine;

public class FireDemonDamagedState : EnemyBaseState
{
    private MovementComponent _movementComponent;
    private float _moveDuration = 0.5f;
    private float _moveSpeed = 10.0f;
    public FireDemonDamagedState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        _movementComponent = enemy.GetComponent<MovementComponent>();
        if(!_movementComponent)Debug.LogWarning("Move Component is null");
    }

    public override void Enter()
    {
        base.Enter();
        enemy.IsTakenDamaged = false;
        _movementComponent.Move(new Vector3(0,0,0),15);
        
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