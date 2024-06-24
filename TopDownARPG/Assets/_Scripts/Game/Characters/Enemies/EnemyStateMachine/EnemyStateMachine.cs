using FrameWork.FSM;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy;

    public EnemyStateMachine(Enemy enemy)
    {
        this.Enemy = enemy;
    }
}