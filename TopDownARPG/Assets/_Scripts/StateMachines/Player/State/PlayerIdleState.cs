using FrameWork.Resource;
using UnityEngine;


public class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerIdle_Config");
        if(playerStateConfig == null) Debug.LogError("PlayerIdle_Configが見つかりません");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerInputComponent.Axis != Vector2.zero)
        {
            ChangeState(PlayerStateEnum.Move);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}