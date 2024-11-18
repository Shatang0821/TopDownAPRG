
public class RHMovementState : RHBaseState
{
    public RHMovementState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.IsTakenDamaged)
        {
            ChangeState(RHStateEnum.Damaged);
            return;
        }

    }
}
