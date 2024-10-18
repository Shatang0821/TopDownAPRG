using System;
using System.Collections.Generic;

namespace FrameWork.FSM
{
    public abstract class StateMachine
    {
        protected IState currentState { get; private set; }     //現在状態クラス
        public string CurrentStateName { get; private set; }    //現在状態列挙型
        
        private Dictionary<string, IState> _stateTable = new();
        
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
            var stateName = newState.ToString();
            if (_stateTable.TryGetValue(stateName, out IState state))
            {
                currentState?.Exit();
                CurrentStateName = stateName;
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

        public void RegisterState(Enum stateEnum, IState state)
        {
            _stateTable[stateEnum.ToString()] = state;
        }
    
        /// <summary>
        /// 現在ステータスチェック
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool CheckState(IState state) => state == currentState;
    }
}