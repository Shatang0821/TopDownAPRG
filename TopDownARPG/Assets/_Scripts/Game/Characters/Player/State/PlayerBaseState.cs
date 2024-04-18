using System.Collections;
using System.Collections.Generic;
using FrameWork.FSM;
using JetBrains.Annotations;
using UnityEngine;

namespace SK
{
    public class PlayerBaseState : BaseState
    {
        protected PlayerStateMachine playerStateMachine;
        
        public PlayerBaseState(string animBoolName,PlayerStateMachine playerStateMachine) : base(animBoolName)
        {
            this.playerStateMachine = playerStateMachine;
        }

        /// <summary>
        /// 状態に入る処理
        /// </summary>
        public override void Enter()
        {
            
        }
        
        /// <summary>
        /// 状態から退出処理
        /// </summary>
        public override void Exit()
        {
            
        }

        public override void HandleInput()
        {
            
        }

        public override void LogicUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }
    }
}
