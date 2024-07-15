using Unity.VisualScripting;

public class MeleeIdleState : MeleeMovementState
{
    public MeleeIdleState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!enemyStateMachine.CheckState(this)) return;
        
        if (enemy.TargetFound)
        {
            if(!enemy.InAttackRange)
            {
                //移動
                enemyStateMachine.ChangeState(FDStateEnum.Move);
                return;
            }
            
        }
    }
}