using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHDieState : RHMovementState
{
    private bool _isDead;
    public RHDieState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isDead = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = false;
        enemy.RaiseOnDeathEvent();
        enemy.gameObject.SetActive(false);
    }
}
