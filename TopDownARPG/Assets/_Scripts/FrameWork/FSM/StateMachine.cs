using System.Collections.Generic;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.FSM
{
    public abstract class StateMachine<TStateEnum>
    {
        protected IState currentState { get; private set; }     //現在状態クラス
        public TStateEnum CurrentState { get; private set; }    //現在状態列挙型
        
        private Dictionary<TStateEnum, IState> _stateTable = new();
        
        /// <summary>
        /// 状態の初期化
        /// </summary>
        /// <param name="startState"></param>
        public void Initialize(TStateEnum startState)
        {
            ChangeState(startState);
        }
        public void ChangeState(TStateEnum newState)
        {
            if (_stateTable.TryGetValue(newState, out IState state))
            {
                if (currentState == null)
                {
                    Debug.Log("currentState is null");
                }
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

        protected void RegisterState(TStateEnum stateEnum, IState state)
        {
            _stateTable[stateEnum] = state;
        }
    }
}