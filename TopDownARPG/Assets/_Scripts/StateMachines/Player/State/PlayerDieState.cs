using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDieState : PlayerBaseState
{
    public PlayerDieState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }
    
    public override void AnimationEndCalled()
    {
        base.AnimationEndCalled();
        player.Die();
    }
    
}