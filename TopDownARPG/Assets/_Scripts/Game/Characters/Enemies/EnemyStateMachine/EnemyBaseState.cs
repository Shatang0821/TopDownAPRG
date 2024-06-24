using FrameWork.FSM;
using UnityEditor;
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
        stateTimer = 0;
        Debug.Log(this.GetType().ToString());
        enemy.SetAnimation(StateBoolHash,true);
    }

    public override void Exit()
    {
        enemy.SetAnimation(StateBoolHash, false);
    }

    public override void LogicUpdate()
    {
        stateTimer += Time.deltaTime;
        if (enemy.Damaged)
        {
            enemy.TakenDamageState();
        }
        
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