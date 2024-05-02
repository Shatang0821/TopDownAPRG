using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void AnimationEventCalled()
    {
        base.AnimationEventCalled();
        playerStateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}