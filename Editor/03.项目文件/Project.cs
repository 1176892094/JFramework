// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-08 00:12:53
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed class Project
    {
        private static readonly Dictionary<string, Object> assetsCaches = new Dictionary<string, Object>();
        private static Project instance;

        private Project()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUI;
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        [InitializeOnLoadMethod]
        private static void Enable() => instance ??= new Project();

        private static void OnGUI(string guid, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                if (!assetsCaches.TryGetValue(assetPath, out var assetData))
                {
                    assetData = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (assetData != null)
                    {
                        assetsCaches[assetPath] = assetData;
                    }
                }

                if (assetData == null)
                {
                    if (assetsCaches.ContainsKey(assetPath))
                    {
                        assetsCaches.Remove(assetPath);
                    }
                }

                if (rect.height > 16)
                {
                    return;
                }

                DrawTree(assetData == null ? 0 : assetData.GetInstanceID(), rect);
                DrawIcon(assetPath, rect);
            }
        }

        private static void DrawTree(int guid, Rect rect)
        {
            var x = Mathf.Max(0, rect.x - 128f - 16f);
            var width = Mathf.Min(128f, rect.x - 16f);
            var treeRect = new Rect(x, rect.y, width, rect.height);
            var texCoords = new Rect(1 - width / 128f, 0, width / 128f, 1);
            GUI.DrawTextureWithTexCoords(treeRect, ProjectIcon.GetIcon(Tree.Normal), texCoords);

            if (!Reflection.HasChild(guid))
            {
                treeRect.width = 16;
                treeRect.x = rect.x - 16;
                GUI.DrawTexture(treeRect, ProjectIcon.GetIcon(Tree.Middle));
            }

            if (Mathf.FloorToInt((rect.y - 4) / 16 % 2) != 0)
            {
                var itemRect = new Rect(0, rect.y, rect.width + rect.x, rect.height);
                EditorGUI.DrawRect(itemRect, Color.black * 0.05f);
            }

            rect.width += rect.x + 16f;
            rect.height = 1;
            rect.x = 0;
            rect.y += 15.5f;
            EditorGUI.DrawRect(rect, Color.black * 0.1f);
        }

        private static void DrawIcon(string path, Rect rect)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                var folder = Path.GetFileName(path);

                if (ProjectIcon.icons.TryGetValue(folder, out var icon))
                {
                    rect.width = rect.height;
                    GUI.DrawTexture(rect, ProjectIcon.GetIcon(icon));
                }
            }
        }
    }
}