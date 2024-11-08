using System;
using System.Collections;
using FrameWork.EventCenter;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力処理
/// </summary>
public class PlayerInputComponent : MonoBehaviour
{
    #region 変数定義

    private PlayerInputAcion _inputActions;
    
    //private InputDevice _currentDevice; //現在デバイス
    public Observer<InputDevice> CurrentDevice;
    //移動
    public Vector2 Axis => _inputActions.GamePlay.Axis.ReadValue<Vector2>();
    //ダッシュ
    public bool Dash => _inputActions.GamePlay.Dash.WasPerformedThisFrame();

    public bool Attack => _inputActions.GamePlay.Attack.IsPressed();
    
    // マウスの位置
    public Vector2 MousePosition => Mouse.current.position.ReadValue();
    
    //攻撃バッファの継続時間
    private float _attackInputBufferTime = 0.1f;            
    //攻撃入力バッファ
    private WaitForSeconds _waitAttackInputBufferTime;
    //バッファ持ちチェック
    [HideInInspector]
    public bool HasAttackInputBuffer;
    
    #endregion

    #region クラスライフサイクル

    private void Awake()
    {
        Init();
    }
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        _inputActions = new PlayerInputAcion();
       CurrentDevice = new Observer<InputDevice>(Keyboard.current);
       _waitAttackInputBufferTime = new WaitForSeconds(_attackInputBufferTime);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        CurrentDevice.Value = Gamepad.current;
        InputSystem.onActionChange += OnActionChange;
        CurrentDevice.Register(new Action<InputDevice>(OnDeviceChanged));
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        InputSystem.onActionChange -= OnActionChange;
        CurrentDevice.UnRegister(new Action<InputDevice>(OnDeviceChanged));
    }

    #endregion

    #region 入力バッファ

    /// <summary>
    /// 攻撃入力バッファの設定
    /// </summary>
    public void SetAttackInputBufferTimer()
    {
        StopCoroutine(nameof(AttackInputBufferCoroutine));
        StartCoroutine(nameof(AttackInputBufferCoroutine));
    }

    IEnumerator AttackInputBufferCoroutine()
    {
        HasAttackInputBuffer = true;
       
        yield return _waitAttackInputBufferTime;
        HasAttackInputBuffer = false;
    }

    #endregion

    #region デバイス
    
    protected void OnDeviceChanged(InputDevice device)
    {
        Debug.Log($"Maximum Health Changed to: {device}");
    }
    
    /// <summary>
    /// デバイス切り替え処理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    if (CurrentDevice.Value == Keyboard.current)
                        return;
                    CurrentDevice.Value = Keyboard.current;
                    //Debug.Log(_currentDevice);
                    break;
                case Gamepad:
                    if (CurrentDevice.Value == Gamepad.current)
                        return;
                    CurrentDevice.Value = Gamepad.current;
                    //Debug.Log(_currentDevice);
                    break;
            }
        }
    }

    #endregion
}