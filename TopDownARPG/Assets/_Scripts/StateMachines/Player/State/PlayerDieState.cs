using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDieState : PlayerBaseState
{
    public PlayerDieState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (stateTimer >= 3.0f)
        {
            player.Die();
        }
    }
    
}