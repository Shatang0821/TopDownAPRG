using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    private PlayerStateMachine _playerStateMachine;
    public Animator Animator { get; private set; }

    private void Awake()
    {
        PlayerController = new PlayerController(this.transform);
        Animator = GetComponentInChildren<Animator>();
        _playerStateMachine = new PlayerStateMachine(this);
        _playerStateMachine.Initialize(PlayerStateEnum.Idle);
    }

    private void OnEnable()
    {
        PlayerController.OnEnable();
    }

    private void OnDisable()
    {
        PlayerController.OnDisable();
    }


    // Update is called once per frame
    void Update()
    {
        _playerStateMachine.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _playerStateMachine.PhysicsUpdate();
    }

    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="animHash"></param>
    /// <param name="value"></param>
    public void SetAnimation(int animHash, bool value)
    {
        Animator.SetBool(animHash, value);
    }
}