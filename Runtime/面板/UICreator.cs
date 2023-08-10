// #if UNITY_EDITOR
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using JFramework.Core;
// using TMPro;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// namespace JFramework.Editor
// {
//     using VisualElement = Dictionary<string, Component>;
//
//     public static class UICreator
//     {
//         /// <summary>
//         /// 忽略名称
//         /// </summary>
//         private static readonly HashSet<string> ignore = new HashSet<string>()
//         {
//             "Image", "Panel", "Button", "Toggle", "Slider", "RawImage",
//             "Text (Legacy)", "Button (Legacy)", "InputField (Legacy)",
//             "Text (TMP)", "InputField (TMP)",
//         };
//
//         [MenuItem("Tools/JFramework/UICreator")]
//         private static void FindAssets()
//         {
//             string[] guids = AssetDatabase.FindAssets("t:prefab", new[] { AssetSetting.FILE_PATH });
//             foreach (var guid in guids)
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(guid);
//                 var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//                 if (prefab != null && prefab.GetComponent<UIPanel>() != null)
//                 {
//                     var components = new List<(string, string)>();
//                     components.AddRange(FindComponent<Button>(prefab));
//                     components.AddRange(FindComponent<Toggle>(prefab));
//                     components.AddRange(FindComponent<TMP_Text>(prefab));
//                     components.AddRange(FindComponent<TMP_InputField>(prefab));
//                     var builder = PoolManager.Pop<StringBuilder>();
//                     foreach (var (type, name) in components)
//                     {
//                         var fix = name[..1].ToLower() + name[1..];
//                         builder.AppendFormat("\t\tpublic {0} {1};\n", type, fix);
//                     }
//
//                     var text = Resources.Load<TextAsset>("Panel").text;
//                     text = text.Replace("TemplatePanel", prefab.name);
//                     text = text.Replace("//TODO:1", builder.ToString());
//                     if (builder.ToString().Length > 0)
//                     {
//                         File.WriteAllText($"Assets/Scripts/UI/Auto/{prefab.name}.cs", text);
//                     }
//                 }
//             }
//
//             AssetDatabase.Refresh();
//         }
//
//         private static IEnumerable<(string, string)> FindComponent<T>(GameObject prefab) where T : Component
//         {
//             var components = prefab.GetComponentsInChildren<T>();
//             var findList = new List<(string, string)>();
//             foreach (var component in components)
//             {
//                 var name = component.gameObject.name;
//                 if (ignore.Contains(name)) continue;
//                 findList.Add((typeof(T).Name, name));
//             }
//
//             return findList;
//         }
//     }
// }
//
// #endif