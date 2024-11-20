using FrameWork.Resource;
using Unity.VisualScripting;

public class RHIdleState : RHMovementState
{
    public RHIdleState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
        enemyStateConfig = ResManager.Instance.GetAssetCache<RockHurlerStateConfig>(stateConfigPath + "RockHurler/RockHurlerIdle_Config");
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
            ChangeState(RHStateEnum.Attack); 
            if (enemy.TargetFound)
            {
                //ˆÚ“®
                ChangeState(RHStateEnum.Move);
                return;
            }
            return;
        }

    }
}
