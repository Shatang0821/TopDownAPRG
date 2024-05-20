namespace FrameWork.FSM
{
    public interface IState
    {
        public void Enter();
        
        public void Exit();
        
        public void LogicUpdate();
        
        public void PhysicsUpdate();
    
        public void AnimationEventCalled();

        public void AnimationEndCalled();
    }
}
