// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 20:04:37
// // # Recently: 2025-04-09 20:04:37
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed class Importer
    {
        private static readonly Dictionary<string, Object> assetsCaches = new Dictionary<string, Object>();
        private static Importer instance;

        private Importer()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUI;
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        [InitializeOnLoadMethod]
        private static void Enable() => instance ??= new Importer();

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

                DrawTexture(assetData == null ? 0 : assetData.GetInstanceID(), rect);

                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    var folder = Path.GetFileName(assetPath);

                    if (EditorTool.icons.TryGetValue(folder, out var icon))
                    {
                        rect.width = rect.height;
                        GUI.DrawTexture(rect, EditorIcon.GetImage(icon));
                    }
                }
            }
        }

        private static void DrawTexture(int guid, Rect rect)
        {
            var x = Mathf.Max(0, rect.x - 128 - 16);
            var width = Mathf.Min(128, rect.x - 16);
            var position = new Rect(x, rect.y, width, rect.height);
            var texCoords = new Rect(1 - width / 128, 0, width / 128, 1);
            GUI.DrawTextureWithTexCoords(position, EditorItem.GetImage(Item.Normal), texCoords);

            if (!Reflection.HasChild(guid))
            {
                position.width = 16;
                position.x = rect.x - 16;
                GUI.DrawTexture(position, EditorItem.GetImage(Item.Middle));
            }

            if (Mathf.FloorToInt((rect.y - 4) / 16 % 2) != 0)
            {
                var itemRect = new Rect(0, rect.y, rect.width + rect.x, rect.height);
                EditorGUI.DrawRect(itemRect, Color.black * 0.05f);
            }

            rect.width += rect.x + 16;
            rect.height = 1;
            rect.x = 0;
            rect.y += 15.5f;
            EditorGUI.DrawRect(rect, Color.black * 0.1f);
        }
    }
}