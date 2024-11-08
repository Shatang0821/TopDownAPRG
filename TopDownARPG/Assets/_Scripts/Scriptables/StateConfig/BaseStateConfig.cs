using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract　class BaseStateConfig : ScriptableObject
{
    [Header("General Settings")]
    public float FullLockTime;                      // 完全状態ロック時間
    public float PartialLockTime;                   // 部分状態ロック時間

    [Header("State Transitions")]
    public List<StateTransition> stateTransitions;  // 状態間の遷移時間
    
    /// <summary>
    /// 状態遷移時間取得
    /// </summary>
    /// <param name="targetState">遷移したい状態名</param>
    /// <returns>遷移時間</returns>
    public float GetTransitionDuration(string targetState)
    {
        // 状態遷移時間取得
        foreach (var transition in stateTransitions)
        {
            if (transition.targetState == targetState)
            {
                return transition.transitionDuration;
            }
        }
        // 存在しないときに0を返す
        return 0;
    }
}

[Serializable]
public class StateTransition
{
    public string targetState;          //状態名
    public float transitionDuration;    //遷移時間
}
