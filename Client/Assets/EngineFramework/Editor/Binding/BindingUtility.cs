/****************
 *@class name:		BindingUtility
 *@description:		数据绑定的工具类
 *@author:			selik0
*************************************************************************/
using UnityEngine;
using PiscesEngine;
using System.Collections.Generic;
using System.Linq;
using PiscesEngine.UI;
using System.Text;
namespace PiscesEditor
{
    public static class BindingUtility
    {
        public static void CollectComponent(Transform root)
        {
            BindBehaviour bindBehaviour = root.GetComponent<BindBehaviour>();
            if (!bindBehaviour)
                return;

            Dictionary<Component, ViewFieldInfo> infoDict = new Dictionary<Component, ViewFieldInfo>();
            var viewFieldDict = bindBehaviour.GetViewFieldDict();
            foreach (var item in viewFieldDict.Values)
            {
                if (!item.component)
                    return;
                infoDict.Add(item.component, item);
            }

            CollectSingle(root, infoDict);

            if (root.childCount == 0)
                return;

            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.TryGetComponent<BindBehaviour>(out var bind))
                {
                    CollectComponent(child);
                }
                else
                {
                    CollectSingle(child, infoDict);
                }
            }

            var arr = infoDict.Values.ToArray();
            RemoveDuplicateName(arr);
            bindBehaviour.UpdateViewFieldInfos(arr);
        }

        static void CollectSingle(Transform node, Dictionary<Component, ViewFieldInfo> infoDict)
        {
            List<Component> components = new List<Component>();
            node.GetComponents(components);
            string[] names = node.name.Split(BindingConfig.splitChar);
            foreach (var item in components)
            {
                if (item == null)
                    continue;
                var typeFullName = item.GetType().FullName;
                if (!BindingConfig.TryComponentConfig(typeFullName, out var config))
                    continue;
                // 自动收集，或者命名包含前缀
                if (config.autoCollect || names.Contains(config.prefix))
                {
                    var nameBuilder = new StringBuilder(names[names.Length - 1]);
                    nameBuilder[0] = char.ToUpper(nameBuilder[0]);
                    nameBuilder = nameBuilder.Insert(0, config.prefix);
                    if (infoDict.TryGetValue(item, out var element))
                        element.name = nameBuilder.ToString();
                    else
                        element = new ViewFieldInfo(nameBuilder.ToString(), config.componetFullName, item);
                    infoDict.Add(item, element);
                }
            }
        }

        static void RemoveDuplicateName(ViewFieldInfo[] infos)
        {
            Dictionary<string, List<int>> nameDict = new Dictionary<string, List<int>>();
            for (int i = 0; i < infos.Length; i++)
            {
                var name = infos[i].name;
                if (nameDict.ContainsKey(name))
                {
                    nameDict[name].Add(i);
                }
                else
                {
                    nameDict.Add(name, new List<int>() { i });
                }
            }
            foreach (var item in nameDict)
            {
                if (item.Value.Count == 1)
                {
                    continue;
                }
                for (int i = 0; i < item.Value.Count; i++)
                {
                    infos[item.Value[i]].name += i;
                }
            }
        }
    }
}