using FrameWork.Audio;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _duration;
    private Vector3 _direction;
    public PlayerDashState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName, player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _direction = player.Axis;
        _duration = 0.1f;

        //チンペン音
        AudioManager.Instance.PlayDash();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(stateTimer < _duration) return;
        if (player.Axis != Vector2.zero)
        {
            player.ComboConfig.ComboCount = 0;
            playerStateMachine.ChangeState(PlayerStateEnum.Move);
            return;
        }
        if (player.Axis == Vector2.zero)
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
                player.Move(_direction,25,0);
            }
            else
            {
                var forward = player.transform.forward;
                player.Move(new Vector3(forward.x,forward.z,0),25,0,false);

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}