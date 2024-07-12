using UnityEngine;

public class MeleeMoveState : MeleeMovementState
{
    //10m以上2秒ごと,10m以下5m以上1秒ごと,5m以下0.5秒ごと
    private float _pathFindInterval = 2.0f;
    private float _pathFindTimer = 0.0f;
    public MeleeMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _pathFindInterval = 2.0f;
        enemy.FindPath();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!enemyStateMachine.CheckState(this)) return;

        if (!enemy.TargetFound)
        {
            enemyStateMachine.ChangeState(FDStateEnum.Idle);
        }
        
        UpdatePathFindInterval();
        _pathFindTimer += Time.deltaTime;
        if (_pathFindTimer >= _pathFindInterval)
        {
            enemy.FindPath();
            _pathFindTimer = 0.0f;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        MoveAlongPath();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.StopMove();
    }

    private void MoveAlongPath()
    {
        
        if (enemy.Path != null && enemy.Path.Count != 0 && enemy.CurrentPathIndex < enemy.Path.Count)
        {
            Vector3 targetPosition = StageManager.Instance.GridToWorldPosition(enemy.Path[enemy.CurrentPathIndex].Pos);
            
            var targetDirection = (targetPosition - enemy.transform.position).normalized;
            targetDirection.y = 0; 
            
            enemy.Move(new Vector2(targetDirection.x,targetDirection.z));
            if (Vector3.Distance(enemy.transform.position, targetPosition) < 0.1f)
            {
                enemy.CurrentPathIndex++;
            }
        }
        else
        {
            enemy.StopMove();
        }
    }
    
    /// <summary>
    /// 経路探索頻度の調整
    /// </summary>
    private void UpdatePathFindInterval()
    {
        float distanceToPlayer = enemy.DistanceToPlayer;
        if (distanceToPlayer > 10.0f)
        {
            _pathFindInterval = 2.0f;
        }
        else if (distanceToPlayer > 5.0f)
        {
            _pathFindInterval = 1.0f;
        }
        else
        {
            _pathFindInterval = 0.5f;
        }
    }
}