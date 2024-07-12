using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class PlayerMoveState : PlayerMovementState
    {
        public PlayerMoveState(string animBoolName, Player player,PlayerStateMachine stateMachine) : base(animBoolName, player,stateMachine)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(!playerStateMachine.CheckState(this))
                return;
            
            if (player.Axis == Vector2.zero)
            {
                playerStateMachine.ChangeState(PlayerStateEnum.Idle);
                return;
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            player.Move();
        }
    }
}
