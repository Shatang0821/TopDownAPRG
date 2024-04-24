using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.SetAnimation(Animator.StringToHash("Run"),true);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimation(Animator.StringToHash("Run"),false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(player.PlayerController.Axis == Vector2.zero)
            playerStateMachine.ChangeState(PlayerStateEnum.Idle);
        Debug.Log(player.PlayerController.Attack);
        if(player.PlayerController.Attack)
            playerStateMachine.ChangeState(PlayerStateEnum.Attack);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.PlayerController.Move();
    }
}