using System;
using System.Collections.Generic;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : UnitySingleton<EnemyManager>
{
    private List<Enemy> _currentWaveEnemies;
    private List<Enemy> _enemiesToRemove;
    
    private GameObject _enemy;
    private EnemiesConfig _enemiesConfig;

    private Transform _playerTransform; //プレイヤーの変換情報
    /// <summary>
    /// EnemyManagerの初期化
    /// </summary>
    public void Initialize()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        
        _enemiesConfig = ResManager.Instance.GetAssetCache<EnemiesConfig>("Enemies Config/Enemies_Config");
        if (!_enemiesConfig)
        {
            DebugLogger.Log("Enemy配置ファイルが取得できません");
            return;
        }
        _currentWaveEnemies = new List<Enemy>();
        _enemiesToRemove = new List<Enemy>();
        SpawnEnemy();
    }
    
    /// <summary>
    /// 敵の生成
    /// </summary>
    public void SpawnEnemy()
    {
        
        foreach (var enemy in _enemiesConfig.Enemies)
        {
            var eObject = GameObject.Instantiate(enemy, new Vector3(5,0,25), Quaternion.identity);
            var enemyComponent = eObject.GetComponent<Enemy>();
            enemyComponent.SetPlayerTransform(_playerTransform);
            _currentWaveEnemies.Add(enemyComponent);
        }
    }

    /// <summary>
    /// ウェーブに敵を登録する
    /// </summary>
    /// <param name="enemy">敵</param>
    public void RegisterWaveEnemy(Enemy enemy)
    {
        _currentWaveEnemies.Add(enemy);
        enemy.OnDeath += HandleEnemyDeath;
    }
    
    private void HandleEnemyDeath(Enemy enemy)
    {
        _enemiesToRemove.Add(enemy);
        enemy.OnDeath -= HandleEnemyDeath;
        // Wave管理などの追加処理
    }

    private void Update()
    {
        foreach (var enemy in _currentWaveEnemies)
        {
            if (enemy)
            {
                enemy.LogicUpdate(); 
            }
        }
        
        // 遅延削除処理
        if (_enemiesToRemove.Count > 0)
        {
            foreach (var enemy in _enemiesToRemove)
            {
                _currentWaveEnemies.Remove(enemy);
                // その他のクリーンアップ処理
            }
            _enemiesToRemove.Clear();
        }
    }
}