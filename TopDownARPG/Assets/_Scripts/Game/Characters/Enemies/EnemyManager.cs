using System.Collections.Generic;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<Enemy> _currentWaveEnemies;
    private GameObject _enemy;
    private EnemiesConfig _enemiesConfig;

    public void Initialize()
    {
        _enemiesConfig = ResManager.Instance.GetAssetCache<EnemiesConfig>("Enemies Config/Enemies_Config");
        if (_enemiesConfig)
        {
            Debug.Log(_enemiesConfig.Melee.name);
        }
        _currentWaveEnemies = new List<Enemy>();
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        GameObject enemyObject = GameObject.Instantiate(_enemiesConfig.Melee, new Vector3(5,1,-5), Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();   
        _currentWaveEnemies.Add(enemy);
    }
    public void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        
        if (_currentWaveEnemies.Contains(enemy))
        {
            Debug.Log("Yes");
            enemy.TakeDamage(damage);
        }
    }
}