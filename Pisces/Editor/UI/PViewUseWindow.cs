/* ----------------------------------------------
    * @Author          :    cb
    * @DateTime        :    2020年12月29日 17:30:33 星期二
    * @Description     :    PViewUseWindow
-----------------------------------------------*/

using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
namespace Pisces
{
    public class PViewUseWindow : EditorWindow
    {
        [MenuItem("Pisces/UIPanel/属性赋值", false, 21)]
        static void Create()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("提示", "请在编译结束后调用", "OK");
            }
            else
            {
                PViewUseWindow win = EditorWindow.GetWindow(typeof(PViewUseWindow), false, "挂载VIEW", true) as PViewUseWindow;
                win.position = new Rect((Screen.currentResolution.width - 800f) / 2f, (Screen.currentResolution.height - 600f) / 2f, 800f, 600f);
                win.Show();
            }
        }

        List<string> _taskList = new List<string>();
        int _taskCount;
        bool _doing;
        Assembly _csharpAss;
        void OnEnable()
        {
            if (Directory.Exists(PDefine.PBundlesFolder))
            {
                _taskList.AddRange(Directory.GetFiles(PDefine.PBundlesFolder, "*.prefab", SearchOption.AllDirectories));
                _taskCount = _taskList.Count;
                _doing = true;
                _csharpAss = Assembly.Load("Assembly-CSharp");
            }
        }

        void Update()
        {
            if (_doing)
            {
                if (_taskList.Count != 0)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_taskList[0]);
                    if (prefab != null)
                    {
                        PViewMarker uiPrefabInfo = prefab.GetComponent<PViewMarker>();
                        if (uiPrefabInfo != null && !string.IsNullOrEmpty(uiPrefabInfo.className))
                        {
                            string csFileName = uiPrefabInfo.className.Substring(uiPrefabInfo.className.LastIndexOf('/') + 1);
                            Type viewType = _csharpAss.GetType(csFileName);
                            if (viewType != null)
                            {
                                Component com = prefab.GetComponent(viewType);
                                if (com == null)
                                    com = prefab.AddComponent(viewType);
                                FieldInfo field = null;
                                bool isDirty = false;
                                for (int i = 0, lenI = uiPrefabInfo.fieldNameList.Count; i < lenI; i++)
                                {
                                    field = viewType.GetField(uiPrefabInfo.fieldNameList[i]);
                                    if (field == null)
                                    {
                                        Debug.LogError(viewType + "类型,并没有定义," + uiPrefabInfo.fieldNameList[i] + ",字段?");
                                    }
                                    else
                                    {
                                        if (uiPrefabInfo.fieldValueList[i] == null)
                                        {
                                            Debug.LogWarning(uiPrefabInfo.fieldNameList[i] + " 没有有效的赋值……");
                                        }
                                        else
                                        {
                                            if ((UnityEngine.Object)field.GetValue(com) != uiPrefabInfo.fieldValueList[i])
                                            {
                                                field.SetValue(com, uiPrefabInfo.fieldValueList[i]);
                                                isDirty = true;
                                            }
                                        }
                                    }
                                }
                                if (isDirty)
                                    EditorUtility.SetDirty(prefab);
                            }
                            else
                            {
                                Debug.LogError(_taskList[0] + ",找不到类 " + csFileName + " 的类型定义,确定执行了上一步吗?");
                            }
                        }
                    }
                    _taskList.RemoveAt(0);
                }
                if (_taskList.Count != 0)
                    EditorUtility.DisplayProgressBar("正在处理", _taskList[0], (_taskCount - _taskList.Count) / (float)_taskCount);
                else
                {
                    EditorUtility.ClearProgressBar();
                    _doing = false;
                    EditorUtility.DisplayDialog("Complete", "相信科學……", "確定");
                    this.Close();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}

