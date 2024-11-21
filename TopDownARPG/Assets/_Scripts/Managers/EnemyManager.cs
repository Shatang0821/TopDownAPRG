using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.EventCenter;
using FrameWork.Interface;
using FrameWork.Pool;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IInitializable,IUpdatable
{
    private List<WaveConfig> _waveConfigs;
    private int _currentWaveIndex = 0;
    
    public List<Enemy> _currentWaveEnemies;
    public List<Enemy> _enemiesToRemove;
    
    private GameObject _enemy;
    private WaveConfig _waveConfig;

    private Transform _playerTransform; //プレイヤーの変換情報

    private Transform _level;

    /// <summary>
    /// EnemyManagerの初期化
    /// </summary>
    public void Init()
    {
        _waveConfig = ResManager.Instance.GetAssetCache<WaveConfig>("Enemies Config/Enemies_Config");
        if (!_waveConfig)
        {
            DebugLogger.Log("Enemy配置ファイルが取得できません");
            return;
        }
        _currentWaveEnemies = new List<Enemy>();
        _enemiesToRemove = new List<Enemy>();
    }

    public void SetLevel(Transform level)
    {
        _level = level;
    }

    private void OnDestroy()
    {
        if(_currentWaveEnemies != null)
        {
            foreach (var enemy in _currentWaveEnemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        
        if(_enemiesToRemove != null)
        {
            foreach (var enemy in _enemiesToRemove)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        
        Reset();
    }

    /// <summary>
    /// データ構造のリセット
    /// </summary>
    public void Reset()
    {
        _currentWaveEnemies.Clear();
        _enemiesToRemove.Clear();
    }
    
    /// <summary>
    /// ウェーブの設定
    /// </summary>
    /// <param name="waveConfigs"></param>
    public void SetWaveConfig(List<WaveConfig> waveConfigs)
    {
        Reset();
        this._waveConfigs = waveConfigs;
        _currentWaveIndex = 0;
    }


    /// <summary>
    /// プレイヤーの変換情報を設定する
    /// </summary>
    /// <param name="playerTransform"></param>
    public void SetPlayerTransform(Transform playerTransform) {_playerTransform = playerTransform;}
    
    
    /// <summary>
    /// ウェーブの自動進行
    /// </summary>
    /// <returns>true ウェーブがまだある false ウェーブが終わり</returns>
    public void StartWave()
    {
        if (_currentWaveIndex < _waveConfigs.Count)
        {
            StartCoroutine(SpawnWave(_waveConfigs[_currentWaveIndex]));
            _currentWaveIndex++;
        }
        else
        {
            Debug.Log("クリア");
            //クリア時のイベント
            EventCenter.TriggerEvent(LevelEvent.Clear);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="waveConfig"></param>
    /// <returns></returns>
    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        foreach (var spawnInfo in waveConfig.Enemies)
        {
            yield return new WaitForSeconds(spawnInfo.SpawnTime);
            
            var worldSpawnPos = GameManager.Instance.LevelManager.CurrentStageInstance.transform.TransformPoint(spawnInfo.SpawnLocalPosition);
//            Debug.Log(worldSpawnPos);
            var enemy = PoolManager.Release(spawnInfo.EnemyPrefab,worldSpawnPos,Quaternion.identity).GetComponent<Enemy>();
            
            if (enemy != null)
            {
                enemy.SetPlayerTransform(_playerTransform);
                RegisterWaveEnemy(enemy);
            }
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
        
    }

    public void LogicUpdate()
    {
        foreach (var enemy in _currentWaveEnemies)
        {
            if (enemy && enemy.gameObject.activeSelf)
            {
                enemy.LogicUpdate(); 
            }
        }
        
        // 遅延削除処理
        if (_enemiesToRemove.Count == _currentWaveEnemies.Count && _enemiesToRemove.Count != 0)
        {
            foreach (var deleteEnemy in _enemiesToRemove)
            {
                //非アクティブになってから消す
                _currentWaveEnemies.Remove(deleteEnemy);
                // その他のクリーンアップ処理
            }
            _enemiesToRemove.Clear();
            _currentWaveEnemies.Clear();
            
            StartWave();
        }
    }

    
}