/****************
 *@class name:		BindBehaviour
 *@description:		ui 组件数据绑定
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PiscesEngine.UI
{
    public class BindBehaviour : ActiveStateBehaviour
    {
        [SerializeField] protected string codePath;
        [SerializeField] protected UiLayerTag uiLayerTag;
        [SerializeField] protected ViewFieldInfo[] viewFieldInfos;

        public string CodePath { get => codePath; }
        public UiLayerTag UiLayerTag { get => uiLayerTag; }
#if UNITY_EDITOR
        [SerializeField] protected ModelFieldInfo[] modelFieldInfos;

        public Dictionary<int, ModelFieldInfo> GetModelFieldDict()
        {
            var len = modelFieldInfos?.Length ?? 0;
            var dict = new Dictionary<int, ModelFieldInfo>(len);
            if (len == 0)
            {
                return dict;
            }
            foreach (var item in modelFieldInfos)
            {
                dict.Add(item.DataId, item);
            }
            return dict;
        }

        public Dictionary<string, ViewFieldInfo> GetViewFieldDict()
        {
            var len = viewFieldInfos?.Length ?? 0;
            var dict = new Dictionary<string, ViewFieldInfo>(len);
            if (len == 0)
                return dict;
            foreach (var item in viewFieldInfos)
            {
                dict.Add(item.name, item);
            }
            return dict;
        }
#endif
        // public T TryGet<T>(string name) where T : Component
        // {
        //     if (viewFieldDict == null)
        //     {
        //         Debug.LogError("viewFieldDict为null");
        //         return null;
        //     }
        //     if (!viewFieldDict.TryGetValue(name, out ViewFieldInfo info))
        //     {
        //         Debug.LogError($"viewFieldDict里未找到key{name}");
        //         return null;
        //     }
        //     if (!info.component)
        //     {
        //         Debug.LogError($"{name}的component为null");
        //         return null;
        //     }
        //     return info.component as T;
        // }

        public string GetUIClassName()
        {
            return codePath;
        }

        public void UpdateViewFieldInfos(ViewFieldInfo[] infos)
        {
            viewFieldInfos = infos;
        }
    }
}