using System.Collections;
using System.Collections.Generic;
using FrameWork.Resource;
using UnityEngine;


public class PlayerDamagedState : PlayerBaseState
{
    
    public PlayerDamagedState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerDamaged_Config");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.GetCurrentHealth <= 0)
        {
            ChangeState(PlayerStateEnum.Die);
            return;
        }
        
        if (playerStateConfig.FullLockTime < stateTimer)
        {
            ChangeState(PlayerStateEnum.Idle);
            return;
        }
        
        if(playerStateConfig.PartialLockTime < stateTimer)
        {
            if(playerInputComponent.Attack)
            {
                ChangeState(PlayerStateEnum.Attack);
                return;
            }
            if (playerInputComponent.Axis != Vector2.zero)
            {
                ChangeState(PlayerStateEnum.Move);
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