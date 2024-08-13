/****************
 *@class name:		ViewFieldInfo
 *@description:		ui view的字段信息
 *@author:			selik0
*************************************************************************/
using System.Text;
using UnityEditor;
using UnityEngine;
namespace PiscesEngine
{
    public class ViewFieldInfo
    {
        public string name;
        public Component component;
#if UNITY_EDITOR
        public string componentFullName;
        private ViewFieldInfo() { }
        public ViewFieldInfo(string name, string componentFullName, Component component)
        {
            this.name = name;
            this.component = component;
            this.componentFullName = componentFullName;
        }
        public string[] bindingEvents;
        public class FieldBindingInfo
        {
            public int dataId;
            public bool reverse;
            public string componentFieldName;
        }
        public FieldBindingInfo[] fieldBindingInfos;

        const string COMPONENT_DECLARATION = "protected {0} {1};";
        const string COMPONENT_BIND = "    {0} = bindBehaviour.TryGet<{1}>({2});";

        public string GetDeclarationCode()
        {
            return string.Format(COMPONENT_DECLARATION, componentFullName, name);
        }

        public string GetBindCode()
        {
            return string.Format(COMPONENT_BIND, name, componentFullName, name);
        }
#endif
    }
}