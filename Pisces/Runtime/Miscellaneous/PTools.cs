/* ----------------------------------------------
    * @Author          :    cb
    * @DateTime        :    2020年12月29日 16:17:20 星期二
    * @Description     :    PTools
-----------------------------------------------*/
#if UNITY_EDITOR
// using TMPro;
// using Excel;
// using System.Data;
// using UnityEditor;
#endif
using System;
using System.IO;
// using UnityEngine;
// using System.Text;
// using System.Reflection;
// using System.Collections;
// using System.Collections.Generic;
// using System.Security.Cryptography;

namespace Pisces
{
    public static class PTools
    {
        public static void CreateDirectory(string path)
        {
            if (!string.IsNullOrEmpty(Path.GetExtension(path)))
                path = path.Replace(Path.GetFileName(path), "");
            path = path.Replace(@"\\", "/");
            path = path.Replace(@"\", "/");
            string prefix = "";
            string[] floderNames;
            if (path.Contains(":"))
            {
                prefix = path.Split(':')[0] + ":";
                floderNames = path.Split(':')[1].Split('/');
            }
            else if (path.Contains("./"))
            {
                prefix = path.Split('/')[0] + "/";
                floderNames = path.Replace(prefix, "").Split('/');
            }
            else if (path.StartsWith("/"))
            {
                prefix = "/";
                floderNames = path.Substring(1).Split('/');
            }
            else
            {
                floderNames = path.Split('/');
            }
            for (int i = 0, lenI = floderNames.Length; i < lenI; i++)
            {
                if (string.IsNullOrEmpty(floderNames[i]))
                    continue;
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = floderNames[i];
                }
                else
                {
                    if (prefix.EndsWith("/"))
                        prefix += floderNames[i];
                    else
                        prefix += "/" + floderNames[i];
                }
                if (!Directory.Exists(prefix))
                    Directory.CreateDirectory(prefix);
            }
        }
    }
}