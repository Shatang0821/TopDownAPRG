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
        RegisterState(PlayerStateEnum.Idle,new PlayerIdleState("Idle",player,this));
        RegisterState(PlayerStateEnum.Move,new PlayerMoveState("Move",player,this));
        RegisterState(PlayerStateEnum.Attack,new PlayerAttackState("Attack",player,this));
    }
    
    
}