/****************
 *@class name:		AbstractFsm
 *@description:		有限状态机基类
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;

namespace PiscesGame
{
    public enum FsmRunningStatus
    {
        Ready,
        Running,
        Destroyed,
    }

    public abstract class AbstractFsm
    {
        public string Name => m_name;
        public int Count => m_runningStateDict.Count;

        private readonly string m_name;
        private readonly Dictionary<Type, List<IFsmState>> m_typeDict = new Dictionary<Type, List<IFsmState>>();
        private readonly Dictionary<int, IFsmState> m_runningStateDict = new Dictionary<int, IFsmState>();
        private readonly Dictionary<string, object> m_dataDict = new Dictionary<string, object>();

        public bool Contains<T>() where T : IFsmState
        {
            return Contains(typeof(T));
        }

        public bool Contains(Type type)
        {
            return m_typeDict.ContainsKey(type) && m_typeDict[type].Count > 0;
        }

        public bool ContainsKey(int key)
        {
            return m_runningStateDict.ContainsKey(key);
        }

        public virtual List<IFsmState> GetFsmStates<T>()
        {
            return GetFsmStates(typeof(T));
        }

        public virtual List<IFsmState> GetFsmStates(Type stateType)
        {
            if (m_typeDict.TryGetValue(stateType, out var list))
            {
                return list;
            }
            return null;
        }

        public IFsmState GetFsmState(int key)
        {
            if (m_runningStateDict.TryGetValue(key, out var state))
            {
                return state;
            }
            return null;
        }

        public virtual void StartState<T>() where T : IFsmState, new()
        {
            var state = new T();
            StartState(state);
        }

        public virtual void StartState(IFsmState state)
        {
            if (state == null)
            {
                throw new Exception("不能开始空状态");
            }

            if (!m_runningStateDict.TryGetValue(state.Key, out var oldState))
            {
                state.OnInit(this);
                state.OnEnter(this);
                AddState(state);
            }
        }

        public virtual void StopStates<T>() where T : IFsmState
        {
            StopStates(typeof(T));
        }

        public virtual void StopStates(Type stateType)
        {
            if (m_typeDict.TryGetValue(stateType, out var list))
            {
                foreach (var item in list)
                {
                    m_runningStateDict.Remove(item.Key);
                }
            }
            m_typeDict.Remove(stateType);
        }

        public void StopState(int key)
        {
            if (m_runningStateDict.TryGetValue(key, out var state))
            {
                StopState(state);
            }
        }

        public void StopState(IFsmState state)
        {
            state.OnExit(this);
            RemoveState(state);
        }

        public void SetData(string name, object value)
        {
            m_dataDict[name] = value;
        }

        public T GetData<T>(string name)
        {
            if (m_dataDict.TryGetValue(name, out object data))
            {
                return (T)data;
            }
            return default;
        }

        public virtual void Shutdown()
        {
            foreach (var item in m_runningStateDict.Values)
            {
                item.OnExit(this);
            }
            m_runningStateDict.Clear();
            m_typeDict.Clear();
            m_dataDict.Clear();
        }

        private void AddState(IFsmState state)
        {
            m_runningStateDict.Add(state.Key, state);
            var stateType = state.GetType();
            if (!m_typeDict.TryGetValue(stateType, out var list))
            {
                list = new List<IFsmState>();
                m_typeDict.Add(stateType, list);
            }
            list.Add(state);
        }

        private void RemoveState(IFsmState state)
        {
            m_runningStateDict.Remove(state.Key);
            var stateType = state.GetType();
            if (m_typeDict.TryGetValue(stateType, out var list))
            {
                list.Remove(state);
            }
        }
    }
}