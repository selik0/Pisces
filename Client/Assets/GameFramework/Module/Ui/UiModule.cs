/****************
 *@class name:		UiModule
 *@description:		ui界面管理类
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using PiscesEngine.UI;
using UnityEngine;
namespace PiscesGame
{
    internal sealed class UiModule : GameModule<UiModule>
    {
        private Dictionary<UiLayerTag, List<UiProxy>> m_panelDict = new Dictionary<UiLayerTag, List<UiProxy>>();
        private List<UiProxy> m_nowShowUiList = new List<UiProxy>();
        public override void ReLogin()
        {

        }

        public void ShowWindow<T>()
        {

        }

        public void ShowPanel<T>()
        {

        }
    }
}