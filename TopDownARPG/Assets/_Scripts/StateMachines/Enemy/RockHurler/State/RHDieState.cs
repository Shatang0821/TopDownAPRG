using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHDieState : RHBaseState
{
    public RHDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (stateTimer >= 2.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        enemy.RaiseOnDeathEvent();
        enemy.gameObject.SetActive(false);
    }
}
