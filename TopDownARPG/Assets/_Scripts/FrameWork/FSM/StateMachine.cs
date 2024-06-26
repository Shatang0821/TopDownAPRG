using System;
using System.Collections.Generic;

namespace FrameWork.FSM
{
    public abstract class StateMachine
    {
        protected IState currentState { get; private set; }     //現在状態クラス
        public Enum CurrentState { get; private set; }    //現在状態列挙型
        
        private Dictionary<Enum, IState> _stateTable = new();
        
        /// <summary>
        /// 状態の初期化
        /// </summary>
        /// <param name="startState"></param>
        public void Initialize(Enum startState)
        {
            ChangeState(startState);
        }
        public void ChangeState(Enum newState)
        {
            if (_stateTable.TryGetValue(newState, out IState state))
            {
                currentState?.Exit();
                CurrentState = newState;
                currentState = state;

                currentState.Enter();
            }
            
        }

        public void LogicUpdate()
        {
            currentState?.LogicUpdate();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }

        public void AnimationEventCalled()
        {
            currentState?.AnimationEventCalled();
        }

        public void AnimationEndCalled()
        {
            currentState?.AnimationEndCalled();
        }

        public void RegisterState(Enum stateEnum, IState state)
        {
            _stateTable[stateEnum] = state;
        }
    
        /// <summary>
        /// 現在ステータスチェック
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool CheckState(IState state) => state == currentState;
    }
}