/****************
 *@class name:		RedDotBasicItem
 *@description:		红点表现层抽象基类
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    public abstract class RedDotBasicItem
    {
        protected bool m_isForceShow = true;
        public bool IsForceShow
        {
            get { return m_isForceShow && m_redDotData.IsShow; }
            set
            {
                m_isForceShow = value;
                RefreshRedState();
            }
        }
        protected RedDotData m_redDotData;
        public RedDotBasicItem(params object[] keys)
        {
            m_redDotData = RedDotModule.Instance.GetRedDotData(keys);
            m_redDotData.OnRedStateChanged += RefreshRedState;
        }

        protected virtual void Destroy()
        {
            m_redDotData.OnRedStateChanged -= RefreshRedState;
            m_redDotData = null;
        }

        protected abstract void RefreshRedState();
    }
}