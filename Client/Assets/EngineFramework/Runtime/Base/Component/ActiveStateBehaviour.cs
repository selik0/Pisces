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
        public class ActiveStateEvent : BaseEngineEvent { }
        public ActiveStateEvent DestroyEvent { get; protected set; } = new ActiveStateEvent();
        public ActiveStateEvent EnableEvent { get; protected set; } = new ActiveStateEvent();
        public ActiveStateEvent DisableEvent { get; protected set; } = new ActiveStateEvent();

        protected void OnEnable()
        {
            EnableEvent.Invoke();
        }
        protected void OnDisable()
        {
            DisableEvent.Invoke();
        }
        protected void OnDestroy()
        {
            DestroyEvent.Invoke();
        }
    }
}