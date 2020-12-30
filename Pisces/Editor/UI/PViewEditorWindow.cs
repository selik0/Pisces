

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.IMGUI;
// using System.IO;
// using UnityEditor.IMGUI.Controls;

// namespace Pisces
// {
//     public class PViewEditorWindow : EditorWindow
//     {
//         Vector2 winSize = new Vector2(400, 600);
//         [MenuItem("Pisces/View", false, 10)]
//         static void Create()
//         {
//             PViewEditorWindow win = EditorWindow.GetWindow(typeof(PViewEditorWindow), false, "生成View类", true) as PViewEditorWindow;
//             win.position = new Rect((Screen.currentResolution.width - 800f) / 2f, (Screen.currentResolution.height - 600f) / 2f, win.winSize.x, win.winSize.y);
//             // win.minSize = win.winSize;
//             win.Show();
//         }
//         List<string> _taskList = new List<string>();
//         List<TreeViewItem> allTreeItems = new List<TreeViewItem>();
//         TreeViewItem root;
//         void OnEnable()
//         {
//             _taskList.AddRange(Directory.GetFiles(PDefine.UIPanelPrefabFolder, "*.prefab", SearchOption.AllDirectories));
//             int index = 0;
//             root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
//             foreach (var item in _taskList)
//             {
//                 string str = item.Substring(item.IndexOf("\\") + 1);
//                 string[] temp = str.Split('\\');

//                 for (int i = 0; i < temp.Length; i++)
//                 {
//                     index++;
//                     allTreeItems.Add(new TreeViewItem {id = index, depth = i, displayName = temp[i] });
//                 }
//                 Debug.Log(temp.Length);
//             }
//             TreeViewState state = new TreeViewState();
//             //treeView = new TreeView(state);
//         }
//         TreeView treeView;
//         private void OnGUI()
//         {
//             EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Width(winSize.x - 10f));
//             if (GUILayout.Button("属性生成"))
//             {

//             }
//             if (GUILayout.Button("属性赋值"))
//             {

//             }
//             EditorGUILayout.EndHorizontal();

//             GUILayout.Space(1);

//             EditorGUILayout.BeginScrollView(new Vector2(0, 0), GUI.skin.box, GUILayout.Width(400), GUILayout.Height(600));

//             if (GUILayout.Toggle(true, "全选"))
//             {

//             }

//             EditorGUILayout.EndScrollView();
//             // EditorGUILayout.BeginVertical(GUILayout.Width(300), GUILayout.Height(600f));
//             // EditorGUILayout.EndVertical();
//         }
//     }
// }


