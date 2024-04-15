using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Resource
{
    public class ResManager : UnitySingleton<ResManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public T GetAssetCache<T>(string path) where T : UnityEngine.Object
        {
            // string path = "Assets/AssetsPackage/" + name;
            // UnityEngine.Object target = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            // return target as T;
            UnityEngine.Object target = Resources.Load<T>(path);
            return (T)target;
        }
    }
}
