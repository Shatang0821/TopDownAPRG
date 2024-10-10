using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerMovementState
{
    private MovementComponent _movementComponent;
    private PlayerStatasComponent _playerStatasComponent;
    
    public PlayerMoveState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
        _movementComponent = player.GetComponent<MovementComponent>();
        if (_movementComponent == null) Debug.LogError("MovementComponent��������܂���");
        _playerStatasComponent = player.GetComponent<PlayerStatasComponent>();
        if (_playerStatasComponent == null) Debug.LogError("PlayerStatasComponent��������܂���");
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
        _movementComponent.Move(playerInputComponent.Axis,_playerStatasComponent.CurrentStats.Speed,0.2f);
    }
    
}