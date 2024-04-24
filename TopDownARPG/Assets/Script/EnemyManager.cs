using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject _player;
    [SerializeField]private Enemy _enemy;
    private List<Enemy> _enemies;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.Log("Player is Null");
        }

        _enemies = new List<Enemy>()
        {
           GameObject.Instantiate(_enemy,new Vector3(0,1,0),Quaternion.identity),
           GameObject.Instantiate(_enemy,new Vector3(0,1,0),Quaternion.identity),
        };
    }

    private void Update()
    {
        foreach (var enemy in _enemies)
        {
            enemy.AllowPlayer(_player);
        }
    }
}