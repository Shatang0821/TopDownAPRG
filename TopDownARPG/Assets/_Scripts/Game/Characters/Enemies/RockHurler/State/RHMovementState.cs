using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHMovementState : EnemyBaseState
{
    public RHMovementState(string animBoolName, Enemy enemy, EnemyStateMachine enemyStateMachine) : base(animBoolName, enemy, enemyStateMachine)
    {
    }
}
