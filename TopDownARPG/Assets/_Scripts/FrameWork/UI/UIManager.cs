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

            _currentUIPrefab = _uiPrefabs[uiName];
            _currentUIPrefab.SetActive(true);
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
            Transform view = this.Canvas.transform.Find(uiName);
            if (view)
            {
                GameObject.Destroy(view.gameObject);
            }
        }

        /// <summary>
        /// すべてのUIを削除
        /// </summary>
        public void RemoveAll()
        {
            //すべてのUIをリストに入れる
            List<Transform> children = new List<Transform>();
            //すべてのUIをリストに入れる
            foreach (Transform variable in this.Canvas.transform)
            {
                children.Add(variable);
            }

            //リスト内のUIを削除
            foreach (var t in children)
            {
                GameObject.Destroy(t.gameObject);
            }
        }
    }
}