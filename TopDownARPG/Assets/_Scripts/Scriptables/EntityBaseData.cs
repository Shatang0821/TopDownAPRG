using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EntityBaseData : ScriptableObject
{
    [SerializeField] private Status status;  // Entityの基本数値
    public Status BaseStatus => status;
}

/// <summary>
/// Entity基本数値
/// </summary>
[Serializable]
public struct Status
{
    public float Health;
    public float MagicPoint;
    public float AttackPower;
    public float DefensePower;
    public float Speed;
}


