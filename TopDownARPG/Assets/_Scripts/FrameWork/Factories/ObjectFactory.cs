using FrameWork.Resource;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Factories
{
    /// <summary>
    /// GameObjectを作るクラス
    /// </summary>
    public class ObjectFactory : Singleton<ObjectFactory>
    {
        
        public GameObject CreateGameObject(string path, Transform parent = null)
        {
            // パスからプレハブを取得
            GameObject prefab = ResManager.Instance.GetAssetCache<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found for path: {path}");
                return null;
            }

            // 親が指定されている場合はその下でプレハブをインスタンス化します。それ以外の場合はルートでプレハブをインスタンス化します。
            GameObject go = GameObject.Instantiate(prefab, parent);

            // 非アクティブ化にする
            go.SetActive(false);

            return go;
        }
    }
}