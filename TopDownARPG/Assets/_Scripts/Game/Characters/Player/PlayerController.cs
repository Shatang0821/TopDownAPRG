using UnityEngine;

public class PlayerController
{
    private PlayerInput _playerInput;
    private MoveComponent _moveComponent;
    
    //移動
    public Vector2 Axis => _playerInput.Axis;
    //ダッシュ
    public bool Dash => _playerInput.Dash;

    public bool Attack => _playerInput.Attack;
    public PlayerController(Transform transform)
    {
        _playerInput = new PlayerInput();
        _moveComponent = new MoveComponent(transform);
        
    }

    public void OnEnable()
    {
        _playerInput.OnEnable();
    }

    public void OnDisable()
    {
        _playerInput.OnDisable();
    }

    public void Move()
    {
        if (Axis != Vector2.zero)
        {
            _moveComponent.Move(Axis);
        }
    }
}