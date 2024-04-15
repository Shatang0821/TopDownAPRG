using UnityEngine;

namespace FrameWork.FSM
{
    public abstract class BaseState : IState
    {
        protected int StateBoolHash; // アニメーターのハッシュ値

        protected BaseState(string animBoolName)
        {
            StateBoolHash = Animator.StringToHash(animBoolName); // アニメーターハッシュの初期化
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void HandleInput();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
    }
}