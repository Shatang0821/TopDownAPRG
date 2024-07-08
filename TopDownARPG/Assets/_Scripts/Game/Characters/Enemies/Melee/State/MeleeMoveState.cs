using UnityEngine;

public class MeleeMoveState : MeleeMovementState
{
    private float _pathFindInterval = 2.0f;
    public MeleeMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.FindPath();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.InAttackRange)
        {
            enemyStateMachine.ChangeState(MeleeStateEnum.Attack);
            return;
        }

        if (!enemy.TargetFound)
        {
            enemyStateMachine.ChangeState(MeleeStateEnum.Idle);
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
            //movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
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
            enemy.FindPath();
        }
    }
}