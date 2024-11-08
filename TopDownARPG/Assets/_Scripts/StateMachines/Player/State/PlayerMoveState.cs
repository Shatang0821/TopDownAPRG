using FrameWork.Resource;
using UnityEngine;

public class PlayerMoveState : PlayerMovementState
{
    private MovementComponent _movementComponent;
    private PlayerStatusComponent _playerStatusComponent;
    
    public PlayerMoveState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _movementComponent = player.GetComponent<MovementComponent>();
        if (_movementComponent == null) Debug.LogError("MovementComponentが見つかりません");
        _playerStatusComponent = player.GetComponent<PlayerStatusComponent>();
        if (_playerStatusComponent == null) Debug.LogError("PlayerStatasComponentが見つかりません");
        playerStateConfig = ResManager.Instance.GetAssetCache<PlayerStateConfig>(stateConfigPath + "PlayerMove_Config");
        if(playerStateConfig == null) Debug.LogError("PlayerMove_Configが見つかりません");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerInputComponent.Axis == Vector2.zero)
        {
            ChangeState(PlayerStateEnum.Idle);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _movementComponent.Move(playerInputComponent.Axis,_playerStatusComponent.CurrentStatus.Speed,true,0.6f);
    }
    
}