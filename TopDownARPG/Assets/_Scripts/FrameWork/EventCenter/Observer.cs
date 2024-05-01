using UnityEngine;

namespace FrameWork.EventCenter
{
    /// <summary>
    /// 数値変換監視クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T>
    {
        private T _value;
        private readonly string _actionKey;//トリガーするイベント

        public Observer(T value,string actionKey)
        {
            this._value = value;
            this._actionKey = actionKey;
            Debug.Log(_actionKey);
        }
        
        public T Value
        {
            get => _value;
            set
            {
                this._value = value;
                EventCenter.TriggerEvent(_actionKey,_value);
            }
        }
    }
}