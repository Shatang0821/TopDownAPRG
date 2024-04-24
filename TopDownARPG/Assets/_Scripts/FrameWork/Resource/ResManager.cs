using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.Resource
{
    public class ResManager : Singleton<ResManager>
    {
        /// <summary>
        /// リソースからオブジェクトを取得する
        /// </summary>
        /// <param name="path">パス</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns></returns>
        public T GetAssetCache<T>(string path) where T : Object
        {
            //string path = "Assets/Resources/" + name;
            Object target = Resources.Load<T>(path);
            return (T)target;
        }
    }
}
