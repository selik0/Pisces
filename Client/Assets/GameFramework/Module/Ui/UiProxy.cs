/****************
 *@class name:		UiProxy
 *@description:		控制ui显示隐藏
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;

namespace PiscesGame
{
    public class UiProxy
    {
        public UiProxy ParentCtrl { get; private set; }
        public IReadOnlyList<UiProxy> ChildCtrls => m_childCtrls;

        private List<UiProxy> m_childCtrls = new List<UiProxy>();

        public void Show()
        {

        }

        public void Hide()
        {

        }
    }
}