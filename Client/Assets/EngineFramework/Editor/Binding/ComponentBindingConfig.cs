/****************
 *@class name:		ComponentBindingConfig
 *@description:		组件绑定的配置
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using PiscesEngine;
using UnityEngine;
namespace PiscesEditor
{
    public class ComponentBindingConfig : ScriptableObject
    {
        public class FieldInfo
        {
            public string fieldName;
            public string fieldType;
            public string codeTemplate;
        }
        public class EventInfo
        {
            public string eventName;
            public string eventType;
            public string registerCodeTemplate;
            public string unRegisterCodeTemplate;
        }
        public string prefix;
        public string componetFullName;
        public bool autoCollect;
        public List<FieldInfo> fieldInfos;
        public List<EventInfo> eventInfos;

#if UNITY_EDITOR
        public bool TryGetUpdateFieldCode(string fieldName, string name, out string code)
        {
            code = string.Empty;
            if (fieldInfos?.Count == 0)
                return false;
            foreach (var item in fieldInfos)
            {
                if (item.fieldName == fieldName)
                {
                    code = string.Format(item.codeTemplate, name);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetRegisterEventCode(string eventName, string bindName, out string code)
        {
            code = string.Empty;
            if (eventInfos?.Count == 0)
                return false;
            foreach (var item in eventInfos)
            {
                if (item.eventName == eventName)
                {
                    code = string.Format(item.registerCodeTemplate, bindName, bindName.FirstUpper());
                    return true;
                }
            }
            return false;
        }

        public bool TryGetUnRegisterEventCode(string eventName, string bindName, out string code)
        {
            code = string.Empty;
            if (eventInfos?.Count == 0)
                return false;
            foreach (var item in eventInfos)
            {
                if (item.eventName == eventName)
                {
                    code = string.Format(item.unRegisterCodeTemplate, bindName, bindName.FirstUpper());
                    return true;
                }
            }
            return false;
        }
#endif
    }
}