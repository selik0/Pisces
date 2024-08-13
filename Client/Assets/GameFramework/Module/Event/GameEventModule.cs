/****************
 *@class name:		GameEventModule
 *@description:		游戏事件模块
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PiscesEngine;
using UnityEngine;

namespace PiscesGame
{
    internal sealed class GameEventModule : GameModule<GameEventModule>
    {
        private readonly Dictionary<Type, IGameEvent> m_delegateMap = new Dictionary<Type, IGameEvent>();

        public override void ReLogin()
        {
            m_delegateMap.Clear();
        }

        public void AddListener<T>(GameEventHandler<T> listener) where T : GameEventData
        {
            var dataType = typeof(T);
            if (!m_delegateMap.TryGetValue(dataType, out var value))
            {
                value = new GameEvent<T>();
                m_delegateMap.Add(dataType, value);
            }
            var gameEvent = value as GameEvent<T>;
            gameEvent.AddListener(listener);
        }

        public void RemoveListener<T>(GameEventHandler<T> listener) where T : GameEventData
        {
            if (!m_delegateMap.TryGetValue(typeof(T), out var value))
            {
                Log.Warning("-->GameEvent<--remove no listener gameEvent ");
                return;
            }
            var gameEvent = value as GameEvent<T>;
            gameEvent.RemoveListener(listener);
        }

        public void Broadcast<T>(T args = null) where T : GameEventData
        {
            if (!m_delegateMap.TryGetValue(typeof(T), out var value))
            {
                Log.Warning("-->GameEvent<--broadcast no listener gameEvent ");
                return;
            }
            value.Broadcast(args);
        }
    }
}