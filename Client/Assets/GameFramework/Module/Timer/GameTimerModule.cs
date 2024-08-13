/****************
 *@class name:		GameTimerModule
 *@description:		
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;

namespace PiscesGame
{
    internal sealed class GameTimerModule : GameModule<GameTimerModule>
    {
        private static uint m_hashCode = 0;
        private Queue<GameTimer> m_recycleQueue = new Queue<GameTimer>();
        private Dictionary<uint, GameTimer> m_timerMap = new Dictionary<uint, GameTimer>();

        public override void ReLogin()
        {
            m_hashCode = 0;
            m_recycleQueue.Clear();
            m_timerMap.Clear();
            
        }

        public GameTimer Create(float delay, float time, float interval, bool isLoop, bool isScaleTime, Action callback)
        {
            var instaceId = m_hashCode++;
            GameTimer timer;
            if (m_recycleQueue.Count > 0)
            {
                timer = m_recycleQueue.Dequeue();
            }
            else
            {
                timer = new GameTimer(instaceId, delay, time, interval, isLoop, isScaleTime, callback);
            }
            m_timerMap[instaceId] = timer;

            timer.Start();
            return timer;
        }

        public void Recycle(GameTimer timer)
        {
            if (timer == null)
            {
                return;
            }
            if (m_timerMap.ContainsKey(timer.InstanceId))
            {
                timer.Stop();
                m_timerMap.Remove(timer.InstanceId);
                m_recycleQueue.Enqueue(timer);
            }
        }
        #region  静态方法
        /// <summary>
        /// 只执行一次的定时器
        /// </summary>
        /// <param name="delay">n秒后执行</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static GameTimer CreateOnceTimer(float delay, Action callback)
        {
            return Instance.Create(delay, 0, 0, false, false, callback);
        }

        public static GameTimer CreateOnceTimer(float delay, bool isScaleTime, Action callback)
        {
            return Instance.Create(delay, 0, 0, false, isScaleTime, callback);
        }

        /// <summary>
        /// 无延迟的固定时间定时器，会在创建时就执行一次，然后每间隔interval后执行
        /// </summary>
        /// <param name="time">持续的时间</param>
        /// <param name="interval">间隔执行时间</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static GameTimer CreateTimer(float time, float interval, Action callback)
        {
            return Instance.Create(delay: 0, time, interval, isLoop: false, isScaleTime: false, callback);
        }
        /// <summary>
        /// 固定时间定时器，会在创建时就执行一次，然后每间隔interval后执行
        /// </summary>
        /// <param name="delay">延迟n秒后执行第一次</param>
        /// <param name="time">持续的时间</param>
        /// <param name="interval">间隔执行时间</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static GameTimer CreateTimer(float delay, float time, float interval, Action callback)
        {
            return Instance.Create(delay, time, interval, isLoop: false, isScaleTime: false, callback);
        }

        public static GameTimer CreateTimer(float delay, float time, float interval, bool isScaleTime, Action callback)
        {
            return Instance.Create(delay, time, interval, isLoop: false, isScaleTime, callback);
        }
        /// <summary>
        /// 无延迟不受时间缩放影响的循环定时器
        /// </summary>
        /// <param name="interval">间隔n秒后执行</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static GameTimer CreateLoopTimer(float interval, Action callback)
        {
            return Instance.Create(delay: 0, time: 0, interval, isLoop: true, isScaleTime: false, callback);
        }

        /// <summary>
        /// 不受时间缩放影响的循环定时器
        /// </summary>
        /// <param name="delay">延迟n秒后执行</param>
        /// <param name="interval">间隔n秒后执行</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static GameTimer CreateLoopTimer(float delay, float interval, Action callback)
        {
            return Instance.Create(delay, time: 0, interval, isLoop: true, isScaleTime: false, callback);
        }

        public static GameTimer CreateLoopTimer(float delay, float interval, bool isScaleTime, Action callback)
        {
            return Instance.Create(delay, time: 0, interval, isLoop: true, isScaleTime, callback);
        }

        public static void RecycleTimer(GameTimer timer)
        {
            Instance.Recycle(timer);
        }
        #endregion
    }
}