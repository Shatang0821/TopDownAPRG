using System;
using System.Collections.Generic;
using UnityEngine;

public abstract　class BaseStateConfig : ScriptableObject
{
    [Header("General Settings")]
    public float fullLockTime;                      // 完全状態ロック時間
    public float partialLockTime;                   // 部分状態ロック時間

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
        Debug.Log("遷移時間が見つかりません");
        return 0;
    }
}

[Serializable]
public class StateTransition
{
    public string targetState;          //状態名
    public float transitionDuration;    //遷移時間
}
