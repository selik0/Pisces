/****************
 *@class name:		UiLayerTag
 *@description:		ui层级标签
 *@author:			selik0
*************************************************************************/
namespace PiscesEngine.UI
{
    public enum UiLayerTag
    {
        /// <summary>
        /// 预留的
        /// </summary>
        Basic = 0,

        /// <summary>
        /// 主界面（特殊的一级功能ui）
        /// </summary>
        Main = 10,

        /// <summary>
        /// 一级功能ui（可以理解为全屏界面）
        /// </summary>
        First,

        /// <summary>
        /// 二级功能ui（一级系统里的子界面，一般为弹窗界面）
        /// </summary>
        Second,

        /// <summary>
        /// 三级功能ui（二级系统里的子界面，一般为弹窗界面）
        /// </summary>
        Three,

        /// <summary>
        /// 确认框ui
        /// </summary>
        Dialog,

        /// <summary>
        /// 飘字ui
        /// </summary>
        Toast,

        /// <summary>
        /// 网络等待ui
        /// </summary>
        NetWait,

        /// <summary>
        /// 场景加载ui
        /// </summary>
        Loading,

        /// <summary>
        /// 调试ui
        /// </summary>
        Debug
    }
}