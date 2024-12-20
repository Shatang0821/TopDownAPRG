﻿
public class FireDemonMovementState : FireDemonBaseState
{
    public FireDemonMovementState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(
        animBoolName, enemy, enemyStateMachine)
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.IsTakenDamaged)
        {
            ChangeState(FDStateEnum.Damaged);
            return;
        }

        if (enemy.InAttackRange)
        {
            //攻撃
            ChangeState(FDStateEnum.Attack);
            return;
        }
    }
}