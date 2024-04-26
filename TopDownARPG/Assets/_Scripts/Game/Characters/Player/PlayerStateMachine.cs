using FrameWork.FSM;
using SK;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Attack,
}

public class PlayerStateMachine : StateMachine<PlayerStateEnum>
{
    public PlayerStateMachine(Player player)
    {
        RegisterState(PlayerStateEnum.Idle,new PlayerIdleState(PlayerStateEnum.Idle.ToString(),this));
        RegisterState(PlayerStateEnum.Move,new PlayerMoveState(PlayerStateEnum.Move.ToString(),this));
        RegisterState(PlayerStateEnum.Attack,new PlayerAttackState(PlayerStateEnum.Attack.ToString(),this));
    }
}