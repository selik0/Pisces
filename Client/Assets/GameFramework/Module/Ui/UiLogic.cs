/****************
 *@class name:		UiLogic
 *@description:		ui逻辑代码基类
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace PiscesGame
{
    public abstract class UiLogic
    {
        
        #region  Unity生命周期
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void OnDestroy();
        #endregion

        #region  Ui生命周期
        protected abstract void OnShow();
        protected abstract void OnHide();
        #endregion
    }
}