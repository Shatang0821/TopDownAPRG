﻿public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName, player, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.Attack)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Attack);
            return;
        }
    }
}