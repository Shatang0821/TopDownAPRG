using System;
using System.Collections.Generic;

namespace FrameWork.EventCenter
{
    public static class EventCenter
    {
        private static Dictionary<Enum, Delegate> m_EventDictionary = new Dictionary<Enum, Delegate>();

        private static void OnListenerAdding(Enum eventType, Delegate callBack)
        {
            if (!m_EventDictionary.ContainsKey(eventType))
            {
                m_EventDictionary.Add(eventType, null);
            }

            Delegate d = m_EventDictionary[eventType];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception($"イベント {eventType} に異なるデリケートを追加しようとする。現在のイベントに対応するデリケートは {d.GetType()}、登録したいデリケートは {callBack.GetType()}。");
            }
        }

        public static void AddListener(Enum eventType, Action callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventDictionary[eventType] = Delegate.Combine(m_EventDictionary[eventType], callBack);
        }

        public static void AddListener<T>(Enum eventType, Action<T> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventDictionary[eventType] = Delegate.Combine(m_EventDictionary[eventType], callBack);
        }

        public static void AddListener<T1, T2>(Enum eventType, Action<T1, T2> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventDictionary[eventType] = Delegate.Combine(m_EventDictionary[eventType], callBack);
        }

        public static void RemoveListener(Enum eventType, Action callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventDictionary[eventType] = (Action)m_EventDictionary[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T>(Enum eventType, Action<T> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventDictionary[eventType] = (Action<T>)m_EventDictionary[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T1, T2>(Enum eventType, Action<T1, T2> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventDictionary[eventType] = (Action<T1, T2>)m_EventDictionary[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        public static void TriggerEvent(Enum eventType)
        {
            if (m_EventDictionary.TryGetValue(eventType, out Delegate d))
            {
                (d as Action)?.Invoke();
            }
        }

        public static void TriggerEvent<T>(Enum eventType, T arg)
        {
            if (m_EventDictionary.TryGetValue(eventType, out Delegate d))
            {
                (d as Action<T>)?.Invoke(arg);
            }
        }

        public static void TriggerEvent<T1, T2>(Enum eventType, T1 arg1, T2 arg2)
        {
            if (m_EventDictionary.TryGetValue(eventType, out Delegate d))
            {
                (d as Action<T1, T2>)?.Invoke(arg1, arg2);
            }
        }

        private static void OnListenerRemoving(Enum eventType, Delegate callBack)
        {
            if (m_EventDictionary.TryGetValue(eventType, out Delegate d))
            {
                if (d == null)
                {
                    throw new Exception($"イベント {eventType} に対応するデリケートが存在しません。");
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception($"イベント {eventType} のデリケートの型が一致しません。");
                }
            }
            else
            {
                throw new Exception($"イベントキー {eventType} は存在していません。");
            }
        }

        private static void OnListenerRemoved(Enum eventType)
        {
            if (m_EventDictionary[eventType] == null)
            {
                m_EventDictionary.Remove(eventType);
            }
        }
    }
}
