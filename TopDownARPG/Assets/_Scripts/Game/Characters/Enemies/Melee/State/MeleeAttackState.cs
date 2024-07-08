public class MeleeAttackState : EnemyBaseState
{
    public MeleeAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }
    
    

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stateTimer > 1.2f)
        {
            enemyStateMachine.ChangeState(MeleeStateEnum.Idle);
        }
    }
}