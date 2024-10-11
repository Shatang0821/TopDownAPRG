using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerMovementState
{
    private MovementComponent _movementComponent;
    private PlayerStatusComponent _playerStatusComponent;
    
    public PlayerMoveState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _movementComponent = player.GetComponent<MovementComponent>();
        if (_movementComponent == null) Debug.LogError("MovementComponent‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        _playerStatusComponent = player.GetComponent<PlayerStatusComponent>();
        if (_playerStatusComponent == null) Debug.LogError("PlayerStatasComponent‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!playerStateMachine.CheckState(this))
            return;

        if (playerInputComponent.Axis == Vector2.zero)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Idle);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _movementComponent.Move(playerInputComponent.Axis,_playerStatusComponent.CurrentStatus.Speed,0.2f);
    }
    
}