/****************
 *@class name:		UIBindingConfig
 *@description:		ui绑定的配置
 *@author:			selik0
*************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace PiscesEditor
{
    public class BindingConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [MenuItem("Assets/Create/ScriptableObject/Create BindingConfig")]
        static void Create()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BindingConfig)}");
            if (guids?.Length > 0)
            {
                Debug.LogError("已存在UIBindingConfig");
                return;
            }
            var data = CreateInstance<BindingConfig>();
            AssetDatabase.CreateAsset(data, "Assets/ZFramework/Editor/ScriptableObject/BindingConfig.asset");
            AssetDatabase.Refresh();
        }
        static BindingConfig _instance;
        public static BindingConfig Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(BindingConfig)}");
                if (guids?.Length > 0)
                {
                    _instance = AssetDatabase.LoadAssetAtPath<BindingConfig>(AssetDatabase.GUIDToAssetPath(guids[0]));
                    if (guids.Length > 1)
                    {
                        Debug.LogError("存在多个Component绑定配置");
                    }
                }
                return _instance;
            }
        }

        public const char splitChar = '_';
        [SerializeField] protected TextAsset[] codeTemplates;
        [SerializeField] protected List<ComponentBindingConfig> staticComponentConfigs;
        [SerializeField] protected List<ComponentBindingConfig> dynamicComponentConfigs;

        Dictionary<string, ComponentBindingConfig> componentDict;
        // Dictionary<string, ComponentBindingConfig.FieldInfo> fieldDict;

        public string[] VariableTypeArr { get; private set; }

        public void OnAfterDeserialize()
        {
            componentDict = new Dictionary<string, ComponentBindingConfig>();
            // fieldDict = new Dictionary<string, ComponentBindingConfig.FieldInfo>();
            HashSet<string> typeSet = new HashSet<string>();

            DeserializeComponentConfig(staticComponentConfigs, typeSet);
            DeserializeComponentConfig(dynamicComponentConfigs, typeSet);

            VariableTypeArr = typeSet.ToArray();
            Array.Sort(VariableTypeArr);
        }

        public void OnBeforeSerialize()
        {

        }

        void DeserializeComponentConfig(List<ComponentBindingConfig> configs, HashSet<string> typeSet)
        {
            if (configs == null || configs.Count == 0)
            {
                return;
            }
            foreach (var item in configs)
            {
                if (componentDict.ContainsKey(item.componetFullName))
                {
                    Debug.LogError($"存在重复的FullName{item.componetFullName}");
                    continue;
                }
                componentDict.Add(item.componetFullName, item);

                // if (item.fieldInfos?.Count > 0)
                // {
                //     foreach (var propertyConfig in item.fieldInfos)
                //     {
                //         typeSet.Add(propertyConfig.fieldName);
                //         fieldDict.Add($"{item.componetName}{splitChar}{propertyConfig.fieldName}", propertyConfig);
                //     }
                // }
            }
        }

        public static bool TryComponentConfig(string componentFullName, out ComponentBindingConfig config)
        {
            return Instance.componentDict.TryGetValue(componentFullName, out config);
        }

        public static bool TryGetCodeTemplate(string name, out string code)
        {
            code = "";
            if (Instance.codeTemplates?.Length == 0)
            {
                Debug.LogError("");
                return false;
            }
            foreach (var item in Instance.codeTemplates)
            {
                if (item.name == name)
                {
                    code = item.text;
                    return true;
                }
            }
            return false;
        }
    }
}