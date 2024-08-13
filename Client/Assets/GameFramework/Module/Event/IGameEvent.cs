/****************
 *@class name:		IGameEvent
 *@description:		游戏事件接口
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    public interface IGameEvent
    {
        public void Broadcast<T>(T data) where T : GameEventData;
    }
}