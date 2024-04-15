using FrameWork.Utils;

namespace FrameWork.FSM
{
    public abstract class StateMachine
    {
        protected IState CurrentState { get; private set; }

        /// <summary>
        /// 状態の初期化
        /// </summary>
        /// <param name="startState"></param>
        public void Initialize(IState startState)
        {
            ChangeState(startState);
        }
        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            
            CurrentState = newState;

            CurrentState.Enter();
        }

        public void HandleInput()
        {
            CurrentState?.HandleInput();
        }

        public void LogicUpdate()
        {
            CurrentState?.LogicUpdate();
        }

        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }
    }
}