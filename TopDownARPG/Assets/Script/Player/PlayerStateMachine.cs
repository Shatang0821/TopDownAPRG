using FrameWork.FSM;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Attack,
}

public class PlayerStateMachine : StateMachine<PlayerStateEnum>
{
    public PlayerStateMachine(Player player)
    {
        RegisterState(PlayerStateEnum.Idle,new PlayerIdleState(player,this));
        RegisterState(PlayerStateEnum.Run,new PlayerRunState(player,this));
        RegisterState(PlayerStateEnum.Attack,new PlayerAttackState(player,this));
    }
}