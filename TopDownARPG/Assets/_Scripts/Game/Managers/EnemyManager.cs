using System.Collections.Generic;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : UnitySingleton<EnemyManager>
{
    private List<Enemy> _currentWaveEnemies;
    private GameObject _enemy;
    private EnemiesConfig _enemiesConfig;
    
    public void Initialize()
    {
        _enemiesConfig = ResManager.Instance.GetAssetCache<EnemiesConfig>("Enemies Config/Enemies_Config");
        if (!_enemiesConfig)
        {
            DebugLogger.Log("Enemy配置ファイルが取得できません");
            return;
        }
        _currentWaveEnemies = new List<Enemy>();
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        foreach (var enemy in _enemiesConfig.Enemies)
        {
           var eObject = GameObject.Instantiate(enemy, new Vector3(5,0,25), Quaternion.identity);
            _currentWaveEnemies.Add(eObject.GetComponent<Enemy>());
        }
    }
    public void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        
        if (_currentWaveEnemies.Contains(enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}