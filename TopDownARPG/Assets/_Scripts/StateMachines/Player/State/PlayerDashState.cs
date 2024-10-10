using FrameWork.Audio;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _duration;
    private Vector3 _direction;                     //プレイヤの入力方向
    private MovementComponent _movementComponent;
    public PlayerDashState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName, player, stateMachine)
    {
        _movementComponent = player.GetComponent<MovementComponent>();
        if(_movementComponent == null) Debug.LogError("MovementComponentが見つかりません");
    }

    public override void Enter()
    {
        base.Enter();
        _direction = playerInputComponent.Axis;
        _duration = 0.1f;

        //チンペン音
        AudioManager.Instance.PlayDash();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(stateTimer < _duration) return;
        if (playerInputComponent.Axis != Vector2.zero)
        {
            player.ComboConfig.ComboCount = 0;
            playerStateMachine.ChangeState(PlayerStateEnum.Move);
            return;
        }
        if (playerInputComponent.Axis == Vector2.zero)
        {
            playerStateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateTimer < _duration)
        {
            if (_direction != Vector3.zero)
            {
                _movementComponent.Move(_direction,25,0);
            }
            else
            {
                var forward = player.transform.forward;
                _movementComponent.Move(new Vector3(forward.x,forward.z,0),25,0,false);

            }
        }
    }
}