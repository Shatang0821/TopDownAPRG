using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(string animBoolName, PlayerStateMachine playerStateMachine) : base(animBoolName, playerStateMachine)
        {
        }
    }
}
