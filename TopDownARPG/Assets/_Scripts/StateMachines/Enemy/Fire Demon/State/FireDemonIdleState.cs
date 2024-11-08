using FrameWork.Resource;

public class FireDemonIdleState : FireDemonMovementState
{
    public FireDemonIdleState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<FireDemonStateConfig>(stateConfigPath + "FireDemon/FireDemonIdle_Config");
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
               ChangeState(FDStateEnum.Move);
                return;
            }
            
        }
    }
}