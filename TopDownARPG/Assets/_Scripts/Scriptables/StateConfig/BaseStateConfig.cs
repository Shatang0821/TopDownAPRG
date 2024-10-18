using System;
using System.Collections.Generic;
using UnityEngine;

public abstract�@class BaseStateConfig : ScriptableObject
{
    [Header("General Settings")]
    public float fullLockTime;                      // ���S��ԃ��b�N����
    public float partialLockTime;                   // ������ԃ��b�N����

    [Header("State Transitions")]
    public List<StateTransition> stateTransitions;  // ��ԊԂ̑J�ڎ���
    
    /// <summary>
    /// ��ԑJ�ڎ��Ԏ擾
    /// </summary>
    /// <param name="targetState">�J�ڂ�������Ԗ�</param>
    /// <returns>�J�ڎ���</returns>
    public float GetTransitionDuration(string targetState)
    {
        // ��ԑJ�ڎ��Ԏ擾
        foreach (var transition in stateTransitions)
        {
            if (transition.targetState == targetState)
            {
                return transition.transitionDuration;
            }
        }
        // ���݂��Ȃ��Ƃ���0��Ԃ�
        Debug.Log("�J�ڎ��Ԃ�������܂���");
        return 0;
    }
}

[Serializable]
public class StateTransition
{
    public string targetState;          //��Ԗ�
    public float transitionDuration;    //�J�ڎ���
}
