using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDamagedState : PlayerBaseState
{
    public PlayerDamagedState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.GetCurrentHealth <= 0)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Die);
            return;
        }
        
        //if (_canOtherState)
        {
            if (playerInputComponent.Axis != Vector2.zero)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
                return;
            }
            else
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Move);
                return;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.Damaged = false;
    }
    
}