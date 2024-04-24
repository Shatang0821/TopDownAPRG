using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.PlayerController.Axis != Vector2.zero)
            playerStateMachine.ChangeState(PlayerStateEnum.Run);
        if(player.PlayerController.Attack)
            playerStateMachine.ChangeState(PlayerStateEnum.Attack);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}