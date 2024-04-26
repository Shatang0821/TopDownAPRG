using UnityEngine;

public class Entity : MonoBehaviour
{
    private float _maxHealth;

    public float MaxHealth
    {
        get => Health;
        set
        {
            if (value >= 0)
            {
                MaxHealth = value;
            }
        }
    }
    private float _health;
    
    public float Health
    {
        get => _health;
        set
        {
            Mathf.Clamp(_health + value, 0, _maxHealth);
            //イベントのトリガ
        }
    }
    
    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="animHash"></param>
    /// <param name="value"></param>
    public virtual void SetAnimation(int animHash, bool value)
    {
        //Animator.SetBool(animHash, value);
    }
}