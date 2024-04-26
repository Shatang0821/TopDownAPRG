using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.Utils;
using UnityEngine;

namespace FrameWork.EventCenter
{
    /// <summary>
    /// イベント処理中心
    /// </summary>
    public class EventCenter
    {
        //デフォルトイベント
        private static Dictionary<string, Delegate> m_EventDictionary = new Dictionary<string, Delegate>();
#region Add

        /// <summary>
        /// イベント登録チェック
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="callBack"></param>
        /// <exception cref="Exception"></exception>
        private static void OnListenerAdding(string eventKey, Delegate callBack)
        {
            //イベント登録処理
            if (!m_EventDictionary.ContainsKey(eventKey))
            {
                m_EventDictionary.Add(eventKey, null);
            }

            Delegate d = m_EventDictionary[eventKey];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("イベント{0}に異なるデリケートを追加しようとする,現在のイベントに対応するデリケートは{1},登録したいデリケートは{2}",
                    eventKey, d.GetType(), callBack.GetType()));
            }
        }

        /// <summary>
        /// 引数なしデリケートを登録
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        public static void AddListener(string eventKey, CallBack callBack)
        {
            OnListenerAdding(eventKey, callBack);

            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }

        /// <summary>
        /// 引数一つ持ち
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T">引数</typeparam>
        public static void AddListener<T>(string eventKey, CallBack<T> callBack)
        {
            OnListenerAdding(eventKey, callBack);

            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }
        
        
        /// <summary>
        /// 引数二つ持ち
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T1">引数</typeparam>
        /// <typeparam name="T2">引数</typeparam>
        public static void AddListener<T1,T2>(string eventKey, CallBack<T1,T2> callBack)
        {
            OnListenerAdding(eventKey, callBack);

            m_EventDictionary[eventKey] = Delegate.Combine(m_EventDictionary[eventKey], callBack);
        }

#endregion

#region Remove
        /// <summary>
        /// イベントの解除チェック
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">イベント</param>
        /// <exception cref="Exception"></exception>
        private static void OnListenerRemoving(string eventKey, Delegate callBack)
        {
            //イベント登録処理
            if (m_EventDictionary.ContainsKey(eventKey))
            {
                Delegate d = m_EventDictionary[eventKey];
                if (d == null)
                {
                    throw new Exception(string.Format("デリケートを解除失敗,イベント{0}には対応するデリケート{1}は存在していない", eventKey,
                        callBack.GetType()));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format(
                        "デリケートを解除失敗,イベント{0}に違う型のデリケートを解除しようとする,現在のイベントに対応するデリケートは{1},解除したいデリケートは{2}", eventKey,
                        d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("デリケートを解除失敗,イベントキー{0}は存在していない", eventKey));
            }
        }

        /// <summary>
        /// イベントの解除処理
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        private static void OnListenerRemoved(string eventKey)
        {
            if (m_EventDictionary[eventKey] == null)
            {
                m_EventDictionary.Remove(eventKey);
            }
        }

        /// <summary>
        /// デリケートを解除
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        public static void RemoveListener(string eventKey, CallBack callBack)
        {
            OnListenerRemoving(eventKey, callBack);

            m_EventDictionary[eventKey] = (CallBack)m_EventDictionary[eventKey] - callBack;

            OnListenerRemoved(eventKey);
        }

        /// <summary>
        /// 引数１つ持ち
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T">引数</typeparam>
        public static void RemoveListener<T>(string eventKey, CallBack<T> callBack)
        {
            OnListenerRemoving(eventKey, callBack);

            m_EventDictionary[eventKey] = (CallBack<T>)m_EventDictionary[eventKey] - callBack;

            OnListenerRemoved(eventKey);
        }
        
        /// <summary>
        /// 引数２つ持ち
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="callBack">デリケート</param>
        /// <typeparam name="T1">引数</typeparam>
        /// <typeparam name="T2">引数</typeparam>
        public static void RemoveListener<T1,T2>(string eventKey, CallBack<T1,T2> callBack)
        {
            OnListenerRemoving(eventKey, callBack);

            m_EventDictionary[eventKey] = (CallBack<T1,T2>)m_EventDictionary[eventKey] - callBack;

            OnListenerRemoved(eventKey);
        }
#endregion

#region Trigger

        /// <summary>
        /// イベントを実行する
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        public static void TriggerEvent(string eventKey)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is CallBack callBack)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("イベントを実行失敗:イベント{0}は違う型のデリケートを持っている", eventKey));
                }
            }
        }

        /// <summary>
        /// 引数一つのイベントを実行
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <param name="arg">引数</param>
        /// <typeparam name="T">型</typeparam>
        /// <exception cref="Exception"></exception>
        public static void TriggerEvent<T>(string eventKey, T arg)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is CallBack<T> callBack)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("イベントを実行失敗:イベント{0}は違う型のデリケートを持っている", eventKey));
                }
            }
        }
        
        /// <summary>
        /// 引数二つのトリガー
        /// </summary>
        /// <param name="eventKey">イベントキー</param>
        /// <typeparam name="T1">引数型1</typeparam>
        /// <typeparam name="T2">引数型2</typeparam>
        public static void TriggerEvent<T1,T2>(string eventKey, T1 arg1,T2 arg2)
        {
            if (m_EventDictionary.TryGetValue(eventKey, out Delegate d))
            {
                if (d is CallBack<T1,T2> callBack)
                {
                    callBack(arg1,arg2);
                }
                else
                {
                    throw new Exception(string.Format("イベントを実行失敗:イベント{0}は違う型のデリケートを持っている", eventKey));
                }
            }
        }
#endregion
    }
}