/* ----------------------------------------------
    * @Author          :    cb
    * @DateTime        :    2020年12月29日 16:10:07 星期二
    * @Description     :    PViewDefineWindow
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
    public class PViewDefineWindow : EditorWindow
    {
        const string view_calss_templet = @"using Pisces;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CLASS_NAME : VIEWTYPE
{
FIELD_DEFINE
}";
        const string class_tag = "CLASS_NAME";
        const string field_define_tag = "FIELD_DEFINE";
        const string class_type_tag = "VIEWTYPE";

        static Dictionary<string, string> typeStringDic = new Dictionary<string, string>();

        [MenuItem("Pisces/UIPanel/生成脚本", false, 20)]
        static void Create()
        {
            InitDic();
            PViewDefineWindow win = EditorWindow.GetWindow(typeof(PViewDefineWindow), false, "导出VIEW", true) as PViewDefineWindow;
            win.position = new Rect((Screen.currentResolution.width - 800f) / 2f, (Screen.currentResolution.height - 600f) / 2f, 800f, 600f);
            win.Show();
        }

        static void InitDic()
        {
            if (typeStringDic.Count != 0)
            {
                return;
            }
            typeStringDic.Add("Text", "txt");
            typeStringDic.Add("Image", "img");
            typeStringDic.Add("Button", "btn");
            typeStringDic.Add("ScrollRect", "scroll");
            typeStringDic.Add("TextMeshProUGUI", "tmp2d");
            typeStringDic.Add("TextMeshPro", "tmp3d");
            typeStringDic.Add("RawImage", "rawImg");
            typeStringDic.Add("InputField", "input");
            typeStringDic.Add("Toggle", "tog");
            typeStringDic.Add("RectTransform", "rtTrans");
            typeStringDic.Add("Transform", "trans");
            typeStringDic.Add("GameObject", "go");
        }

        static List<Assembly> _allAssemblies;
        List<string> _taskList = new List<string>();
        int _taskCount;
        bool _doing;
        Dictionary<string, string> _className2PrefabPath = new Dictionary<string, string>();
        void OnEnable()
        {
            if (Directory.Exists(PDefine.PBundlesFolder))
            {
                //生成本工程的代码目录//
                PTools.CreateDirectory(PDefine.ViewCsFolder);
                _taskList.AddRange(Directory.GetFiles(PDefine.PBundlesFolder, "*.prefab", SearchOption.AllDirectories));
                _taskCount = _taskList.Count;
                _allAssemblies = PAssemblyManager.LoadAssembly();
                _doing = true;
            }
            else
            {
                Debug.LogError("没有找到对应文件夹，请创建" + PDefine.PBundlesFolder +"文件夹，并将prefab放置在此文件夹下");
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
                        if (uiPrefabInfo == null)
                        {
                            _taskList.RemoveAt(0);
                            if (_taskList.Count != 0)
                                EditorUtility.DisplayProgressBar("正在处理", _taskList[0], (_taskCount - _taskList.Count) / (float)_taskCount);
                            return;
                        }
                        if (string.IsNullOrEmpty(uiPrefabInfo.className))
                        {
                            Debug.LogWarning(_taskList[0] + " 需要填写导出View的类名,建议命名 XxxxView");
                            _taskList.RemoveAt(0);
                            if (_taskList.Count != 0)
                                EditorUtility.DisplayProgressBar("正在处理", _taskList[0], (_taskCount - _taskList.Count) / (float)_taskCount);
                            return;
                        }
                        if (_className2PrefabPath.ContainsKey(uiPrefabInfo.className))
                        {
                            Debug.LogError(_className2PrefabPath[uiPrefabInfo.className] + " 已经定义了 " + uiPrefabInfo.className);
                            Debug.LogError(_taskList[0] + " 定义 " + uiPrefabInfo.className + " 无效");
                            _taskList.RemoveAt(0);
                            if (_taskList.Count != 0)
                                EditorUtility.DisplayProgressBar("正在处理", _taskList[0], (_taskCount - _taskList.Count) / (float)_taskCount);
                            return;
                        }
                        else
                        {
                            _className2PrefabPath.Add(uiPrefabInfo.className, _taskList[0]);
                        }

                        Dictionary<string, UnityEngine.Object> oldName2ComDict = new Dictionary<string, UnityEngine.Object>();
                        for (int i = 0; i < uiPrefabInfo.fieldNameList.Count; i++)
                            oldName2ComDict.Add(uiPrefabInfo.fieldNameList[i], uiPrefabInfo.fieldValueList[i]);
                        uiPrefabInfo.fieldNameList.Clear();
                        uiPrefabInfo.fieldValueList.Clear();
                        bool isDirty = false;
                        string fieldDefineCodes = "";
                        int fieldStart;
                        int fieldEnd;
                        string fieldNamePrefix = "";
                        PViewMarker tempUIPrefabInfo;
                        foreach (Transform tf in prefab.GetComponentsInChildren<Transform>(true))
                        {
                            tempUIPrefabInfo = tf.GetComponent<PViewMarker>();
                            if (tempUIPrefabInfo != null)
                            {
                                if (tempUIPrefabInfo != uiPrefabInfo)
                                    continue;
                            }
                            else
                            {
                                Transform tfParent = tf.parent;
                                tempUIPrefabInfo = null;
                                while (tempUIPrefabInfo == null && tfParent != null)
                                {
                                    tempUIPrefabInfo = tfParent.GetComponent<PViewMarker>();
                                    tfParent = tfParent.parent;
                                    if (tfParent == prefab.transform)
                                        break;
                                }
                                if (tempUIPrefabInfo != null && tempUIPrefabInfo != uiPrefabInfo)
                                    continue;
                            }
                            fieldStart = tf.name.IndexOf("[");
                            fieldEnd = tf.name.IndexOf("]");
                            //检查变量命名是否规范//
                            if (!(fieldStart == 0 && fieldEnd == tf.name.Length - 1))
                            {
                                Debug.LogError(_taskList[0] + " 的节点名称 " + tf.name + " 不规范，不符合[]包围的规则……");
                                continue;
                            }
                            string[] marks = GetStringInSymbol(tf.name);
                            //至少需要一个变量一个组件
                            if (marks.Length <= 1)
                                continue;
                            fieldNamePrefix = marks[0];
                            List<Type> defineTypes = GetTypesFromString(tf, marks);
                            if (defineTypes.Count == 0)
                            {
                                Debug.LogError(_taskList[0] + ",节点" + tf.name + "需要指定哪些组件是需要定义成变量的……", tf);
                                continue;
                            }
                            string fieldName;
                            Component targetCom;
                            foreach (Type defineType in defineTypes)
                            {
                                string typeName = defineType.Name.ToLower();
                                if (typeStringDic.ContainsKey(defineType.Name))
                                {
                                    typeName = typeStringDic[defineType.Name];
                                }
                                fieldName = typeName + fieldNamePrefix.Substring(0, 1).ToUpper() + fieldNamePrefix.Substring(1); ;
                                if (!uiPrefabInfo.fieldNameList.Contains(fieldName))
                                {
                                    targetCom = tf.GetComponent(defineType);
                                    if (targetCom != null)
                                    {
                                        fieldDefineCodes += "\tpublic " + defineType.FullName + " " + fieldName + ";\n";
                                        uiPrefabInfo.fieldNameList.Add(fieldName);
                                        uiPrefabInfo.fieldValueList.Add(targetCom);
                                    }
                                    else
                                    {
                                        Debug.LogError(_taskList[0] + " 子节点 " + tf.name + " 缺少 " + defineType + " 组件,无法定义……");
                                    }
                                }
                                else
                                {
                                    Debug.LogError(_taskList[0] + " 定义了重名变量,节点 " + fieldName);
                                }
                            }
                        }
                        Dictionary<string, UnityEngine.Object> newName2ComDict = new Dictionary<string, UnityEngine.Object>();
                        for (int i = 0; i < uiPrefabInfo.fieldNameList.Count; i++)
                            newName2ComDict.Add(uiPrefabInfo.fieldNameList[i], uiPrefabInfo.fieldValueList[i]);
                        if (newName2ComDict.Count != oldName2ComDict.Count)
                            isDirty = true;
                        if (!isDirty)
                        {
                            foreach (var kv in newName2ComDict)
                            {
                                if (!oldName2ComDict.ContainsKey(kv.Key) || oldName2ComDict[kv.Key] != kv.Value)
                                {
                                    isDirty = true;
                                    break;
                                }
                            }
                        }
                        if (!isDirty)
                        {
                            foreach (var kv in oldName2ComDict)
                            {
                                if (!newName2ComDict.ContainsKey(kv.Key) || newName2ComDict[kv.Key] != kv.Value)
                                {
                                    isDirty = true;
                                    break;
                                }
                            }
                        }
                        if (uiPrefabInfo.className.Contains(" "))
                        {
                            isDirty = true;
                            uiPrefabInfo.className = uiPrefabInfo.className.Replace(" ", "");
                            Debug.LogWarning(uiPrefabInfo.className + " 不应该包含空格,已被自动清理!");
                        }
                        if (isDirty)
                            EditorUtility.SetDirty(prefab);
                        string dirName = "";
                        if (uiPrefabInfo.className.IndexOf('/') > 0)
                            dirName = uiPrefabInfo.className.Substring(0, uiPrefabInfo.className.LastIndexOf('/'));
                        string csFileName = uiPrefabInfo.className.Substring(uiPrefabInfo.className.LastIndexOf('/') + 1);
                        string csViewType = uiPrefabInfo.viewType.ToString();
                        string classCode = view_calss_templet.Replace(class_tag, csFileName).Replace(field_define_tag, fieldDefineCodes).Replace(class_type_tag, csViewType).Replace("\r\n", "\n");
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(classCode);
                        PTools.CreateDirectory(PDefine.ViewCsFolder + "/" + dirName);
                        FileStream fs = File.Open(PDefine.ViewCsFolder + "/" + uiPrefabInfo.className + ".cs", FileMode.OpenOrCreate, FileAccess.Write);
                        fs.SetLength(0);
                        fs.Write(utf8Bytes, 0, utf8Bytes.Length);
                        fs.Close();
                        fs = null;
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
        //获取中括号内的内容//
        public static string[] GetStringInSymbol(string input)
        {
            input = input.Replace("[", "");
            input = input.Replace("]", "");
            return input.Split('_');
        }

        public static List<Type> GetTypesFromString(Transform tf, string[] input)
        {
            List<Type> result = new List<Type>();
            Assembly currentAssembly = Assembly.Load("Assembly-CSharp");
            Type t;
            for (int i = 1; i < input.Length; i++)
            {
                string className = input[i];
                t = currentAssembly.GetType(className, false, true);
                if (t != null)
                {
                    if (!(t.IsSubclassOf(typeof(MonoBehaviour))))
                        t = null;
                }
                else
                {
                    foreach (Assembly oneAss in _allAssemblies)
                    {
                        t = oneAss.GetType("UnityEngine." + className, false, true);
                        if (t != null)
                        {
                            if (t.IsSubclassOf(typeof(Component)))
                                break;
                            else
                                t = null;
                        }
                        t = oneAss.GetType("UnityEngine.UI." + className, false, true);
                        if (t != null)
                        {
                            if (t.IsSubclassOf(typeof(Component)))
                                break;
                            else
                                t = null;
                        }
                        t = oneAss.GetType("TMPro." + className, false, true);
                        if (t != null)
                        {
                            if (t.IsSubclassOf(typeof(Component)))
                                break;
                            else
                                t = null;
                        }
                        t = oneAss.GetType(className, false, true);
                        if (t != null)
                        {
                            if (t.IsSubclassOf(typeof(Component)))
                                break;
                            else
                                t = null;
                        }
                    }
                }
                if (t != null)
                    result.Add(t);
                else
                    Debug.LogError("不兼容的组件类型字符:" + className, tf.gameObject);
            }
            return result;
        }
    }
}
