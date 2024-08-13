/****************
 *@class name:		IFsmState
 *@description:		
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    public interface IFsmState
    {
        int Key { get; }

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public void OnInit(AbstractFsm fsm);
        
        /// <summary>
        /// 有限状态机进入状态时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public void OnEnter(AbstractFsm fsm);
        
        /// <summary>
        /// 有限状态机离开状态时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public void OnExit(AbstractFsm fsm);

        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(AbstractFsm fsm, float elapseSeconds, float realElapseSeconds);
    }
}