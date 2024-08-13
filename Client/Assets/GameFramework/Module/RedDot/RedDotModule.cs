/****************
 *@class name:		RedDotModule
 *@description:		红点控制层
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    internal sealed class RedDotModule : GameModule<RedDotModule>
    {
        private RedDotData m_root = new RedDotData(null);

        public override void ReLogin()
        {
            m_root = new RedDotData(null);
        }

        public RedDotData GetRedDotData(object firstKey, params object[] keys)
        {
            RedDotData redDotData = m_root.GetChildData(firstKey);
            if (keys == null || keys.Length == 0)
            {
                return redDotData;
            }
            for (int i = 0; i < keys.Length; i++)
            {
                redDotData = redDotData.GetChildData(keys[i]);
            }
            return redDotData;
        }
    }
}