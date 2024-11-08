using UnityEngine;

namespace FrameWork.FSM
{
    public abstract class BaseState : IState
    {
        protected int StateHash; // アニメーターのハッシュ値
        protected float stateTimer;  // ステート持続時間
        protected BaseState(string animBoolName)
        {
            StateHash = Animator.StringToHash(animBoolName); // アニメーターハッシュの初期化
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
    }
}