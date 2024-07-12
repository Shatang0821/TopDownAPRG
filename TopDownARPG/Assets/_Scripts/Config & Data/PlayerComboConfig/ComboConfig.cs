using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComboConfig",menuName = "ComboSystem/CreateNewComboConfig")]
public class ComboConfig : ScriptableObject
{
    public List<AttackConfig> AttackConfigs;
    public int ComboCount =  0;

    private void OnEnable()
    {
        ComboCount = 0;
    }
}