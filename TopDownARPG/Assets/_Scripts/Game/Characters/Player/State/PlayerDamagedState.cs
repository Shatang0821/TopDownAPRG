using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDamagedState : PlayerBaseState
{
    private bool _canOtherState;
    public PlayerDamagedState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _canOtherState = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (_canOtherState)
        {
            if (player.Axis != Vector2.zero)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
            }
            else
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.Damaged = false;
    }

    public override void AnimationEventCalled()
    {
        base.AnimationEventCalled();
        _canOtherState = true;
    }
}