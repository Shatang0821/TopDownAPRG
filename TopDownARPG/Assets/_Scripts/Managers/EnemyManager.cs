using System;
using System.Collections.Generic;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : UnitySingleton<EnemyManager> , IUpdatable
{
    private List<Enemy> _currentWaveEnemies;
    private List<Enemy> _enemiesToRemove;
    
    private GameObject _enemy;
    private WaveConfig _waveConfig;

    private Transform _playerTransform; //プレイヤーの変換情報
    /// <summary>
    /// EnemyManagerの初期化
    /// </summary>
    public void Initialize()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        
        _waveConfig = ResManager.Instance.GetAssetCache<WaveConfig>("Enemies Config/Enemies_Config");
        if (!_waveConfig)
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
        int i = 1;
        foreach (var enemy in _waveConfig.Enemies)
        {
            var eObject = GameObject.Instantiate(enemy, new Vector3(5,0,i * 10), Quaternion.identity);
            var enemyComponent = eObject.GetComponent<Enemy>();
            enemyComponent.SetPlayerTransform(_playerTransform);
            RegisterWaveEnemy(enemyComponent);
            i++;
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
            if (enemy && enemy.gameObject.activeSelf)
            {
                enemy.LogicUpdate(); 
            }
        }
        
        // 遅延削除処理
        if (_enemiesToRemove.Count > 0)
        {
            foreach (var enemy in _enemiesToRemove)
            {
                //非アクティブになってから消す
                _currentWaveEnemies.Remove(enemy);
                // その他のクリーンアップ処理
            }
            _enemiesToRemove.Clear();
        }
    }

    public void LogicUpdate()
    {
        
    }
}