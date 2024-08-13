/****************
 *@class name:		UIBaseCtrl
 *@description:		ui mvp中的Presenter基类
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
namespace PiscesGame
{
    public class UIBaseCtrl
    {
        public event Action OnInit;
        public event Action OnBeforeOpen;
        public event Action OnOpen;
        public event Action OnBeforeClose;
        public event Action OnClose;
        public event Action OnShow;
        public event Action OnHide;

        public bool IsInitGo => uiGo;
        public bool IsShowing { get; private set; }
        GameObject uiGo;

        public void InitGo(GameObject go)
        {
            uiGo = go;
            OnInit?.Invoke();
        }

        public void Open()
        {
            OnBeforeOpen?.Invoke();

            OnOpen?.Invoke();
        }

        public void Close()
        {
            OnBeforeClose?.Invoke();

            OnClose?.Invoke();
        }

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }
}