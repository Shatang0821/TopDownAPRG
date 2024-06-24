using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHAttackState : MeleeMovementState
{
    public RHAttackState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }
}
