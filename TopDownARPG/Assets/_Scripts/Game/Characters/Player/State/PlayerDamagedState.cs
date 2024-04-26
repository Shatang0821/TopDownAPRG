using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class PlayerDamagedState : PlayerBaseState
    {
        public PlayerDamagedState(string animBoolName, PlayerStateMachine playerStateMachine) : base(animBoolName, playerStateMachine)
        {
        }
    }
}
