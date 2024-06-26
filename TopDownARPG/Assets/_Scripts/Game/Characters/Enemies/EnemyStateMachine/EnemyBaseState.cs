using FrameWork.FSM;
using UnityEngine;

public class EnemyBaseState : BaseState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyBaseState(string animBoolName,Enemy enemy,EnemyStateMachine enemyStateMachine) : base(animBoolName)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().ToString());
    }

    public override void Exit()
    {
        
    }

    public override void LogicUpdate()
    {
       
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void AnimationEventCalled()
    {
        
    }

    public override void AnimationEndCalled()
    {
        
    }
}