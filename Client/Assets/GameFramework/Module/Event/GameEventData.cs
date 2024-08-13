/****************
 *@class name:		GameEventData
 *@description:		游戏事件数据基类
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    public abstract class GameEventData
    {
        public virtual void Broadcast()
        {
            GameEventModule.Instance.Broadcast(this);
        }
    }
}