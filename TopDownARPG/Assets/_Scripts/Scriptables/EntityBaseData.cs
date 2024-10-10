using System;
using UnityEngine;

public abstract class EntityBaseData : ScriptableObject
{
    [SerializeField] private Stats _stats;  // Entityの基本数値
    public Stats BaseStats => _stats;
}

/// <summary>
/// Entity基本数値
/// </summary>
[Serializable]
public struct Stats
{
    public float Health;
    public float MagicPoint;
    public float AttackPower;
    public float DefensePower;
    public float Speed;
}


