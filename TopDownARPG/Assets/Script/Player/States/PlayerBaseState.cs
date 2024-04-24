using System.Collections;
using System.Collections.Generic;
using FrameWork.FSM;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    protected static int currentFrame;
    public PlayerBaseState(Player player,PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void Enter()
    {
        Debug.Log(this.GetType().ToString());
        currentFrame = 0;
        
    }

    public virtual void Exit()
    {
       
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        currentFrame++;
        //Debug.Log(currentFrame);
    }
    

}
