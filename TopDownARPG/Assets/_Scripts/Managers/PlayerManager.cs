using FrameWork.Interface;
using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

public class PlayerManager : MonoBehaviour,IInitializable,IUpdatable
{
    private GameObject _playerPrefab;   //プレイヤーのプレハブ
    private Player _player;            //プレイヤー
    
    public void Init()
    {
        _playerPrefab = ResManager.Instance.GetAssetCache<GameObject>("Prefabs/Player");
        _player = Instantiate(_playerPrefab, GameManager.Instance.LevelManager.GetPlayerSpawnPos(), Quaternion.identity).GetComponent<Player>();
        _player.Initialize();
    }
    public void LogicUpdate()
    {
        if(_player != null)
            _player.LogicUpdate();
    }
    
    public void PhysicsUpdate()
    {
        if(_player != null)
            _player.PhysicsUpdate();
    }

    /// <summary>
    /// プレイヤーの位置更新
    /// </summary>
    /// <param name="newPosition"></param>
    public void UpdatePlayerPosition(Vector3 newPosition)
    {
        if (_player != null)
        {
            DebugLogger.Log("プレイヤーの位置更新" + newPosition);
            _player.transform.position = newPosition;
        }
        
    }
    
    /// <summary>
    /// プレイヤーのオブジェクトを取得する
    /// </summary>
    /// <returns>プレイヤーのオブジェクト</returns>
    public GameObject GetPlayerInstance() => _player.gameObject;
}