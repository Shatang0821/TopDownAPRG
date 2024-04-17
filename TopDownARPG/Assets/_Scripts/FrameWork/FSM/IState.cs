namespace FrameWork.FSM
{
    public interface IState
    {
        public void Enter();            //初期処理
        
        public void Exit();             //終了処理
        
        public void HandleInput();      //入力更新
        
        public void LogicUpdate();      //ロジック更新
        
        public void PhysicsUpdate();    //物理演算更新
    }
}
