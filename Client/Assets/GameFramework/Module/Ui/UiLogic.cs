/****************
 *@class name:		UiLogic
 *@description:		ui逻辑代码基类
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace PiscesGame
{
    public enum UiLogicState
    {
        Opening,
        Opened,
        Closeing,
        Closed,
    }

    public abstract class UiLogic
    {
        public virtual bool IsValid => gameObject != null;

        public virtual string UiPath => string.Empty;

        protected GameObject gameObject;
        protected Transform transform;

        public void InitGo(GameObject go)
        {
            if (!go)
            {
                Debug.LogError("");
                return;
            }
            gameObject = go;
            transform = go.transform;
        }

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }

        protected virtual void OnVisible()
        {

        }

        protected virtual void OnInvisible()
        {
            
        }

    }
}