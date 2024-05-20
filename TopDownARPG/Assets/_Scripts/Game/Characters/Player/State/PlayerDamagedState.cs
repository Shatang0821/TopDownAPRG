using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDamagedState : PlayerBaseState
{
    public PlayerDamagedState(string animBoolName, Player player, PlayerStateMachine stateMachine) : base(animBoolName,
        player, stateMachine)
    {
    }
}