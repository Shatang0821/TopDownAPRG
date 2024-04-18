using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class PlayerDieState : PlayerBaseState
    {
        public PlayerDieState(string animBoolName, PlayerStateMachine playerStateMachine) : base(animBoolName, playerStateMachine)
        {
        }
    }
}
