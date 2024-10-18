using System;
using System.Collections.Generic;
using UnityEngine;

public abstract@class BaseStateConfig : ScriptableObject
{
    [Header("General Settings")]
    public float fullLockTime;                      // Š®‘Só‘ÔƒƒbƒNŠÔ
    public float partialLockTime;                   // •”•ªó‘ÔƒƒbƒNŠÔ

    [Header("State Transitions")]
    public List<StateTransition> stateTransitions;  // ó‘ÔŠÔ‚Ì‘JˆÚŠÔ
    
    /// <summary>
    /// ó‘Ô‘JˆÚŠÔæ“¾
    /// </summary>
    /// <param name="targetState">‘JˆÚ‚µ‚½‚¢ó‘Ô–¼</param>
    /// <returns>‘JˆÚŠÔ</returns>
    public float GetTransitionDuration(string targetState)
    {
        // ó‘Ô‘JˆÚŠÔæ“¾
        foreach (var transition in stateTransitions)
        {
            if (transition.targetState == targetState)
            {
                return transition.transitionDuration;
            }
        }
        // ‘¶İ‚µ‚È‚¢‚Æ‚«‚É0‚ğ•Ô‚·
        Debug.Log("‘JˆÚŠÔ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        return 0;
    }
}

[Serializable]
public class StateTransition
{
    public string targetState;          //ó‘Ô–¼
    public float transitionDuration;    //‘JˆÚŠÔ
}
