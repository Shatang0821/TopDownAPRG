using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork.Interface;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IInitializable,IUpdatable
{
    private List<WaveConfig> _waveConfigs;
    private int _currentWaveIndex = 0;
    
    private List<Enemy> _currentWaveEnemies;
    private List<Enemy> _enemiesToRemove;
    
    private GameObject _enemy;
    private WaveConfig _waveConfig;

    private Transform _playerTransform; //プレイヤーの変換情報
    
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
    public bool StartWave()
    {
        if (_currentWaveIndex < _waveConfigs.Count)
        {
            StartCoroutine(SpawnWave(_waveConfigs[_currentWaveIndex]));
            _currentWaveIndex++;
            return true;
        }

        //ウェーブが終わったら
        return false;
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
            
            //!!!! ローカル座標を世界座標に変換していない
            var enemy = Instantiate(spawnInfo.EnemyPrefab, spawnInfo.SpawnLocalPosition, Quaternion.identity).GetComponent<Enemy>();

            if (enemy != null)
            {
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

    
}