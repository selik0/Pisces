using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Pisces
{
    public class PAssemblyManager
    {
        public static List<Assembly> LoadAssembly()
        {
            List<Assembly> result = new List<Assembly>();
            List<string> dllFileList = new List<string>();
            dllFileList.AddRange(Directory.GetFiles(Application.dataPath, "*.dll", SearchOption.AllDirectories));
            dllFileList.AddRange(Directory.GetFiles(Application.dataPath.Replace("/Assets", @"/Library/ScriptAssemblies"), "*.dll", SearchOption.AllDirectories));
            dllFileList.AddRange(Directory.GetFiles(EditorApplication.applicationContentsPath + "/Managed", "*.dll", SearchOption.AllDirectories));
#if !UNITY_2019
            dllFileList.AddRange(Directory.GetFiles(EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/GUISystem", "*.dll", SearchOption.TopDirectoryOnly));
            dllFileList.AddRange(Directory.GetFiles(EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/Timeline/Runtime", "*.dll", SearchOption.TopDirectoryOnly));
#endif
            Assembly tempAss = null;
            foreach (var file in dllFileList)
            {
                try
                {
                    tempAss = Assembly.Load(Path.GetFileNameWithoutExtension(file));
                }
                catch (System.Exception e)
                {
                    Debug.LogError("加载程序集出错 error " + e.Message);
                }
                finally
                {
                    if (tempAss != null)
                        result.Add(tempAss);
                }
            }
            return result;
        }
    }
}


