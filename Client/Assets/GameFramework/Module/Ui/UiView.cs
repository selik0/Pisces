/****************
 *@class name:		UiView
 *@description:		ui view的基类
 *@author:			selik0
*************************************************************************/
using PiscesEngine;
using PiscesEngine.UI;
using UnityEngine;
namespace PiscesGame
{
    public abstract class UiView
    {
        public abstract string PrefabPath { get; }
        public GameObject GameObject { get; protected set; }
        public RectTransform RectTransform { get; protected set; }
        public BindBehaviour BindBehaviour { get; protected set; }

        public virtual void BindGO(GameObject go)
        {
            Log.Assert(GameObject != null, "bind GameObject is null");
            
            GameObject = go;
            RectTransform = go.transform as RectTransform;
            BindBehaviour = GameObject.GetComponent<BindBehaviour>();

            Log.Assert(BindBehaviour != null, "bind GameObejct not have BindBehaviour");

            InternalBindComponent();
        }

        protected abstract void InternalBindComponent();
    }
}