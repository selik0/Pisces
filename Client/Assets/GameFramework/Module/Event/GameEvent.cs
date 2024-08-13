/****************
 *@class name:		GameEvent
 *@description:		对事件进行封装
 *@author:			selik0
*************************************************************************/
using UnityEngine;

namespace PiscesGame
{
    /// <summary>
    /// 游戏事件的委托类型
    /// </summary>
    /// <param name="args">事件参数</param>
    public delegate void GameEventHandler<T>(T args) where T : GameEventData;

    /// <summary>
    /// 游戏事件
    /// </summary>
    public class GameEvent<T> : IGameEvent where T : GameEventData
    {
        private event GameEventHandler<T> m_event;

        public void AddListener(GameEventHandler<T> listener)
        {
            m_event += listener;
        }


        public void RemoveListener(GameEventHandler<T> listener)
        {
            m_event -= listener;
        }

        public void RemvoeAllListener()
        {
            m_event = null;
        }

        public void Broadcast<V>(V args = null) where V : GameEventData
        {
            Debug.Assert(args is T, "gameEvent data type is difficult");
            m_event?.Invoke(args as T);
        }
    }
}