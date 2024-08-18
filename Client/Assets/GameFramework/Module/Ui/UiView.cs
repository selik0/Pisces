/****************
 *@class name:		UiView
 *@description:		ui view的基类
 *@author:			selik0
*************************************************************************/
using PiscesEngine.UI;
namespace PiscesGame
{
    public abstract class UiView
    {
        public void BindUi(BindBehaviour behaviour)
        {
            InternalBindView();
        }

        public virtual void InternalBindView() { }
    }
}