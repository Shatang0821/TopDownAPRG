using FrameWork.FSM;
using UnityEditor;
using UnityEngine;

public class EnemyBaseState : BaseState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected EnemyStateConfig enemyStateConfig;
    protected Animator animator;
    protected string stateConfigPath = "Config & Data/StateConfig/";
    public EnemyBaseState(string animBoolName,Enemy enemy,EnemyStateMachine enemyStateMachine) : base(animBoolName)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        animator = enemy.GetComponent<Animator>();
    }

    public override void Enter()
    {
        stateTimer = 0;
    }

    public override void Exit()
    {   
        
    }

    public override void LogicUpdate()
    {
        stateTimer += Time.deltaTime;
        
    }

    public override void PhysicsUpdate()
    {
        
    }
    

}