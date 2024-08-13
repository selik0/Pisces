/****************
 *@class name:		GameTimer
 *@description:		单个游戏定时器
 *@author:			selik0
*************************************************************************/
using System;

namespace PiscesGame
{
    /// <summary>
    /// 会在满足delay时间时执行第一次，然后每间隔interval时间执行一次直到大于等于time后停止
    /// </summary>
    public class GameTimer
    {
        private enum TimerState
        {
            Running,
            Stop
        }
        private uint m_instanceId;
        public uint InstanceId => m_instanceId;
        /// <summary>
        /// 延迟多少秒后开始执行
        /// </summary>
        private float m_delay;
        /// <summary>
        /// 需要执行的总时长
        /// </summary>
        private float m_time;
        /// <summary>
        /// 定时器开始了多久
        /// </summary>
        private float m_now;
        /// <summary>
        /// 执行间隔时间
        /// </summary>
        private float m_interval;
        /// <summary>
        /// 是否无限执行， 为true时m_time无效
        /// </summary>
        private bool m_isLoop;
        /// <summary>
        /// 是否受缩放时间影响
        /// </summary>
        private bool m_isScaleTime;
        /// <summary>
        /// 定时器状态
        /// </summary>
        private TimerState m_state;
        /// <summary>
        /// 定时器执行的内容
        /// </summary>
        private Action m_callback;

        private bool m_isFirstInvoke;
        private float m_nowInterval;
        private float m_deltaTime => m_isScaleTime ? UnityEngine.Time.deltaTime : UnityEngine.Time.unscaledDeltaTime;

        private GameTimer() { }

        public GameTimer(uint instaceId, float delay, float time, float interval, bool isLoop, bool isScaleTime, Action callback)
        {
            Reset(instaceId, delay, time, interval, isLoop, isScaleTime, callback);
        }

        public void Reset(uint instaceId, float delay, float time, float interval, bool isLoop, bool isScaleTime, Action callback)
        {
            m_delay = delay;
            m_time = time;
            m_interval = interval;
            m_isLoop = isLoop;
            m_isScaleTime = isScaleTime;
            m_callback = callback;
            m_state = TimerState.Stop;
        }

        public void Start()
        {
            m_state = TimerState.Running;
            m_isFirstInvoke = true;
            m_now = 0;
            m_interval = 0;
        }

        public void Stop()
        {
            m_state = TimerState.Stop;
        }

        public void Invoke()
        {
            if (m_state != TimerState.Running || m_callback == null)
            {
                return;
            }
            //时间到了
            if (m_now >= m_delay + m_time)
            {
                m_state = TimerState.Stop;
                return;
            }
            m_nowInterval += m_deltaTime;
            //还没到延迟时间
            if (m_isFirstInvoke && m_nowInterval < m_delay)
            {
                return;
            }
            // 满足间隔
            if (m_nowInterval >= m_interval || m_isFirstInvoke)
            {
                m_isFirstInvoke = false;
                m_now += m_nowInterval;
                m_nowInterval = 0;
                m_callback.Invoke();
            }
        }
    }
}