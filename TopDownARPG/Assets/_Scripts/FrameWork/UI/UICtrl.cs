using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrameWork.UI
{
    public class UICtrl : MonoBehaviour
    {
        private Dictionary<string, GameObject> _view = new Dictionary<string, GameObject>();

        protected Dictionary<string, GameObject> View => _view;

        public virtual void Awake()
        {
            LoadAllObjectsToView(this.gameObject, "");
        }

        /// <summary>
        /// UIの子オブジェクトをすべてを取得する
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        private void LoadAllObjectsToView(GameObject root, string path)
        {
        
            foreach (Transform transform in root.transform)
            {
                var gameObject = transform.gameObject;
                if(this._view.ContainsKey(path + gameObject.name))
                {
                    continue;
                }
                this._view.Add(path + gameObject.name,gameObject);
                this.LoadAllObjectsToView(gameObject,path + gameObject.name + "/");
            }
        }

        protected void AddButtonListener(string viewName, UnityAction onClick)
        {
            Button bt = this._view[viewName].GetComponent<Button>();
            if (bt == null)
            {
                Debug.LogWarning(this.name + "Try add button listener but failed");
                return;
            }
        
            bt.onClick.AddListener(onClick);
        }
    }
}

