/****************
 *@class name:		ActiveStateBehaviour
 *@description:		GameObject的活动状态组件
 *@author:			selik0
*************************************************************************/
using System;
using UnityEngine;
namespace PiscesEngine
{
    [DisallowMultipleComponent]
    public class ActiveStateBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Function definition for a state event.
        /// </summary>
        public class ActiveStateEvent : BaseEngineEvent<GameObject> { }
        public ActiveStateEvent onDestroy => m_destroyEvent;
        public ActiveStateEvent onEnable => m_enableEvent;
        public ActiveStateEvent onDisable => m_disableEvent;
        private ActiveStateEvent m_destroyEvent = new ActiveStateEvent();
        private ActiveStateEvent m_enableEvent = new ActiveStateEvent();
        private ActiveStateEvent m_disableEvent = new ActiveStateEvent();

        protected void OnEnable()
        {
            m_enableEvent.Invoke(gameObject);
        }
        protected void OnDisable()
        {
            m_disableEvent.Invoke(gameObject);
        }
        protected void OnDestroy()
        {
            m_destroyEvent.Invoke(gameObject);
        }
    }
}