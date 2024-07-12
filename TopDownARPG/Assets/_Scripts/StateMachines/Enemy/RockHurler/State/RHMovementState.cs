
public class RHMovementState : EnemyBaseState
{
    public RHMovementState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.IsTakenDamaged)
        {
            enemy.TakenDamageState();
            return;
        }

    }
}
