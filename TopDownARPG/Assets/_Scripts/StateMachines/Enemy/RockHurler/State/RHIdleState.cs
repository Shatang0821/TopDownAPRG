using Unity.VisualScripting;

public class RHIdleState : RHMovementState
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
        if (!enemyStateMachine.CheckState(this)) return;

        if (enemy.InAttackRange)
        {
            enemyStateMachine.ChangeState(RHStateEnum.Attack); 
            if (enemy.TargetFound)
            {
                //ˆÚ“®
                enemyStateMachine.ChangeState(RHStateEnum.Move);
                return;
            }
            return;
        }

    }
}
