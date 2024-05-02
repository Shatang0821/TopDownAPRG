using UnityEngine;


public class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
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
        if (!playerStateMachine.CheckState(this))
            return;

        if (player.PlayerController.Axis != Vector2.zero)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Move);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}