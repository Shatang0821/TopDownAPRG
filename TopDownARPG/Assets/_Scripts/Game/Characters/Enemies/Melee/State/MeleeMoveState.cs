using UnityEngine;

public class MeleeMoveState : MeleeMovementState
{
    public MeleeMoveState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
}