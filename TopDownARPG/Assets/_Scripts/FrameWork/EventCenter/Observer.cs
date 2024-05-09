using System;
using System.Collections.Generic;

namespace FrameWork.EventCenter
{
    /// <summary>
    /// 数値変換監視クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T>
    {
        private T _value;
        private Delegate _delegate;

        public Observer(T value)
        {
            this._value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    _delegate?.DynamicInvoke(_value);
                }
            }
        }

        /// <summary>
        /// アクションを登録する
        /// </summary>
        /// <param name="callback">登録するアクション</param>
        public void Register(Delegate callback)
        {
            _delegate = Delegate.Combine(_delegate, callback);
        }

        /// <summary>
        /// アクションを解除する
        /// </summary>
        /// <param name="callback">解除するアクション</param>
        public void UnRegister(Delegate callback)
        {
            _delegate = Delegate.Remove(_delegate, callback);
        }
    }
}