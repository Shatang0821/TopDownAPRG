using FrameWork.FSM;
using SK;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Attack,
    Dash,
    Damaged,
}

public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(Player player)
    {
        RegisterState(PlayerStateEnum.Idle,new PlayerIdleState("Idle",player,this));
        RegisterState(PlayerStateEnum.Move,new PlayerMoveState("Move",player,this));
        RegisterState(PlayerStateEnum.Attack,new PlayerAttackState("Attack",player,this));
        RegisterState(PlayerStateEnum.Dash,new PlayerDashState("Move",player,this));
        RegisterState(PlayerStateEnum.Damaged,new PlayerDamagedState("Damaged",player,this));
    }
    
    
}