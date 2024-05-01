using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    private PlayerInput _playerInput;
    private PlayerStateMachine _stateMachine;
    
    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _stateMachine = new PlayerStateMachine(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerInput.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerInput.OnDisable();
    }

    private void Update()
    {
        _stateMachine.LogicUpdate();;
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
}