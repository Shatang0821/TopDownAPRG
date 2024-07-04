using System.Collections.Generic;
using UnityEngine;
using System;
using FrameWork.Utils;
using FrameWork.EventCenter;
using FrameWork.Resource;

namespace FrameWork.UI
{
    /// <summary>
    /// UIControllerの初期化とオブジェクト登録
    /// </summary>
    public class UIManager : PersistentUnitySingleton<UIManager>
    {
        public GameObject Canvas { get;private set; }

        private Dictionary<string, GameObject> _uiPrefabs; //Ctrlプレハブ

        private GameObject _currentUIPrefab; //現在UIPanel

        /// <summary>
        /// UIプレハブルート
        /// </summary>
        private const string UIPREFABROOT = "GUI/UIPrefabs/";

        protected override void Awake()
        {
            base.Awake();
            //Canvasを検索して
            this.Canvas = GameObject.Find("MainCanvas");
            //存在しない場合はエラー表示
            if (this.Canvas == null)
            {
                Debug.LogError("UI manager load Canvas failed!!!!");
            }

            _uiPrefabs = new Dictionary<string, GameObject>();
        }


        /// <summary>
        /// UIオブジェクトの切り替え操作
        /// </summary>
        /// <param name="uiName">stringをキーとして使用する</param>
        public void ChangeUIPrefab(string uiName)
        {
            if (_currentUIPrefab != null)
            {
                _currentUIPrefab.SetActive(false);
            }

            if (_uiPrefabs.ContainsKey(uiName))
            {
                _currentUIPrefab = _uiPrefabs[uiName];
                _currentUIPrefab.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"UI with name {uiName} does not exist in ChangeUIPrefab.");
            }
        }

        /// <summary>
        /// 指定UIを生成する
        /// </summary>
        /// <param name="uiName">UI名前</param>
        /// <param name="parent">オブジェクト親</param>
        /// <returns></returns>
        public UICtrl ShowUI(string uiName, Transform parent = null)
        {
            if (parent == null)
            {
                parent = this.Canvas.transform;
            }

            // Check if the UI prefab already exists
            if (_uiPrefabs.ContainsKey(uiName))
            {
                Debug.LogWarning($"UI with name {uiName} already exists, updating...");
                GameObject existingUI = _uiPrefabs[uiName];
                GameObject.Destroy(existingUI);
                _uiPrefabs.Remove(uiName);
            }

            // UIプレハブを取得する
            GameObject uiPrefab = ResManager.Instance.GetAssetCache<GameObject>(UIPREFABROOT + uiName);

            // UIプレハブを生成する
            GameObject uiView = GameObject.Instantiate(uiPrefab, parent, false);

            uiView.name = uiName;
            _uiPrefabs.Add(uiName, uiView);

            Type type = Type.GetType(uiName + "Ctrl");
            UICtrl ctrl = (UICtrl)uiView.AddComponent(type);

            return ctrl;
        }

        /// <summary>
        /// 指定UIを削除
        /// </summary>
        /// <param name="uiName"></param>
        public void RemoveUI(string uiName)
        {
            if (_uiPrefabs.ContainsKey(uiName))
            {
                GameObject uiView = _uiPrefabs[uiName];
                Debug.Log($"Removing UI: {uiName}");
                GameObject.Destroy(uiView);
                _uiPrefabs.Remove(uiName);
                Debug.Log($"UI {uiName} removed.");
            }
            else
            {
                Debug.LogWarning($"UI with name {uiName} does not exist.");
            }
        }

        /// <summary>
        /// すべてのUIを削除
        /// </summary>
        public void RemoveAll()
        {
            foreach (var uiView in _uiPrefabs.Values)
            {
                GameObject.Destroy(uiView);
            }
            _uiPrefabs.Clear();
        }
    }
}
