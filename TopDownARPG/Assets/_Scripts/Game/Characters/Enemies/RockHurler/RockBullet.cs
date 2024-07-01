using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBullet : MonoBehaviour
{
    GameObject _player;     // プレイヤーオブジェクト
    Rigidbody _rb;          // 岩の Rigidbody コンポーネント

    public float moveSpeed = 10f; // 岩の移動速度
    Vector3 directionToPlayer;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        // プレイヤーの位置を目標とする方向ベクトルを計算
        directionToPlayer = (_player.transform.position - transform.position).normalized;
    }

    void Update()
    {
        if (_player == null)
        {
            return;
        }


        // 岩に速度を与えてプレイヤーの方向に飛ばす
        _rb.velocity = directionToPlayer * moveSpeed;
    }
}
