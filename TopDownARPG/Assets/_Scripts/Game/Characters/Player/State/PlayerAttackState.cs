using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(string animBoolName, PlayerStateMachine playerStateMachine) : base(animBoolName, playerStateMachine)
        {
        }
    }
}
