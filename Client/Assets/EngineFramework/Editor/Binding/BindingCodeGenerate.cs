/****************
 *@class name:		UICodeGenerate
 *@description:		ui 代码生成
 *@author:			selik0
*************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Text;
using PiscesEngine;
using PiscesEngine.UI;
using UnityEditor;
using UnityEngine;
namespace PiscesEditor
{
    public static class BindingCodeGenerate
    {
        [MenuItem("Pisces/Gererate Code/Test")]
        static void TestGenerate()
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(@"using PiscesEngine;
            using PiscesEngine.UI;
            using Unity.Collections;");

            File.WriteAllText(@"C:\PersonUnityProject\Pisces\Client\Assets\ATest\Code.cs", code.ToString());
        }
        static string SAVE_PATH => @$"{Application.dataPath}\ATest\Code.cs";
        const string CODE_TEMPLATE_NAME = "UICodeTemplate";
        const string VIEW_FIELDS = "//#ViewFields#";
        const string GET_VIEW_FIELDS = "//#GetViewFields#";
        const string UPDATE_VIEW_FUNC = "//#UpdateViewFunc#";
        const string MODEL_FIELDS = "//#ModelFields#";
        const string REGISTER_CODE = "//#RegisterCode#";
        const string UNREGISTER_CODE = "//#UnRegisterCode#";

        public static void GenerateUIPanelCode(GameObject go)
        {
            if (!BindingConfig.TryGetCodeTemplate(CODE_TEMPLATE_NAME, out var code))
            {
                Debug.LogError($"未找到ui代码模板文件{CODE_TEMPLATE_NAME}");
                return;
            }
            BindBehaviour bindBehaviour = go.GetComponent<BindBehaviour>();
            var viewFieldDict = bindBehaviour.GetViewFieldDict();
            var modelFieldDict = bindBehaviour.GetModelFieldDict();
            if (!bindBehaviour || viewFieldDict.Count == 0 || modelFieldDict.Count == 0)
            {
                Debug.LogError(@$"UI prefab {go.name} 数据配置保持, bindBehaviour is {bindBehaviour} 
                view field count = {viewFieldDict.Count}, model field count = {modelFieldDict.Count}");
                return;
            }

            StringBuilder viewFields = new StringBuilder();
            StringBuilder getViewFields = new StringBuilder();
            Dictionary<int, StringBuilder> updateViewFuncBodyDict = new Dictionary<int, StringBuilder>();
            StringBuilder updateViewFunc = new StringBuilder();
            StringBuilder modelFields = new StringBuilder();
            StringBuilder registerCode = new StringBuilder();
            StringBuilder unRegisterCode = new StringBuilder();

            HashSet<int> errorDataIds = new HashSet<int>();
            foreach (var item in viewFieldDict.Values)
            {
                if (!BindingConfig.TryComponentConfig(item.componentFullName, out var componentBindingConfig))
                {
                    Debug.LogError($"未找到{item.componentFullName}的绑定配置");
                    continue;
                }
                getViewFields.AppendLine($"{item.name} = bindBehaviour.TryGet<{item.componentFullName}>({item.name})");
                // field
                if (item.fieldBindingInfos?.Length > 0)
                {
                    foreach (var info in item.fieldBindingInfos)
                    {
                        if (!modelFieldDict.TryGetValue(info.dataId, out var modelField))
                        {
                            errorDataIds.Add(info.dataId);
                            continue;
                        }
                        if (!updateViewFuncBodyDict.TryGetValue(info.dataId, out var funcCode))
                        {
                            funcCode = new StringBuilder();
                            updateViewFuncBodyDict.Add(info.dataId, funcCode);
                        }
                        if (componentBindingConfig.TryGetUpdateFieldCode(info.componentFieldName, item.name, out var tempCode))
                        {
                            funcCode.AppendLine(tempCode);
                        }
                    }
                }

                // event
                if (item.bindingEvents?.Length > 0)
                {
                    foreach (var info in item.bindingEvents)
                    {
                        if (componentBindingConfig.TryGetRegisterEventCode(info, item.name, out var code1))
                        {
                            registerCode.AppendLine(code1);
                        }
                        if (componentBindingConfig.TryGetUnRegisterEventCode(info, item.name, out var code2))
                        {
                            unRegisterCode.AppendLine(code2);
                        }
                    }
                }
            }
            // 更新ui的方法的代码
            foreach (var kv in updateViewFuncBodyDict)
            {
                if (!modelFieldDict.TryGetValue(kv.Key, out var modelField))
                {
                    continue;
                }
                updateViewFunc.AppendLine(modelField.GetFuncCode(kv.Value.ToString()));
            }

            foreach (var item in modelFieldDict.Values)
            {
                modelFields.AppendLine(item.GetDataDeclarationCode());
            }

            code.Replace(VIEW_FIELDS, viewFields.ToString());
            code.Replace(MODEL_FIELDS, modelFields.ToString());
            code.Replace(GET_VIEW_FIELDS, getViewFields.ToString());
            code.Replace(UPDATE_VIEW_FUNC, updateViewFunc.ToString());
            code.Replace(REGISTER_CODE, registerCode.ToString());
            code.Replace(UNREGISTER_CODE, unRegisterCode.ToString());

            File.WriteAllText(SAVE_PATH, code.ToString());
            AssetDatabase.Refresh();
        }
    }
}