using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力処理
/// </summary>
public class PlayerInput
{
    #region 変数定義

    private PlayerInputAcion _inputActions;
    private InputDevice _currentDevice; //現在デバイス
    
    //移動
    public Vector2 Axis => _inputActions.GamePlay.Axis.ReadValue<Vector2>();
    //ダッシュ
    public bool Dash => _inputActions.GamePlay.Dash.WasPerformedThisFrame();

    public bool Attack => _inputActions.GamePlay.Attack.IsPressed();
    #endregion

    #region クラスライフサイクル

    public PlayerInput()
    {
        Init();
    }
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        _inputActions = new PlayerInputAcion();
        _currentDevice = Keyboard.current;
    }

    public void OnEnable()
    {
        _inputActions.Enable();
        InputSystem.onActionChange += OnActionChange;
    }

    public void OnDisable()
    {
        _inputActions.Disable();
        InputSystem.onActionChange -= OnActionChange;
    }

    #endregion

    #region デバイス

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
                    if (_currentDevice == Keyboard.current)
                        return;
                    _currentDevice = Keyboard.current;
                    Debug.Log(_currentDevice);
                    break;
                case Gamepad:
                    if (_currentDevice == Gamepad.current)
                        return;
                    _currentDevice = Gamepad.current;
                    Debug.Log(_currentDevice);
                    break;
            }
        }
    }

    #endregion
}