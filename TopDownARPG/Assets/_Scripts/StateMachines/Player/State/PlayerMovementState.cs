public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(string animName, Player player, PlayerStateMachine stateMachine) : base(animName, player, stateMachine)
    {
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // if (playerInputComponent.Dash)
        // {
        //     playerStateMachine.ChangeState(PlayerStateEnum.Dash);
        //     return;
        // }
        // if (player.Damaged)
        // {
        //     playerStateMachine.ChangeState(PlayerStateEnum.Damaged);
        //     return;
        // }
        //
        if (playerInputComponent.Attack)
        {
            ChangeState(PlayerStateEnum.Attack);
        }
    }
}