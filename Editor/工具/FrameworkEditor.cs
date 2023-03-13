using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    public abstract class FrameworkEditor : OdinMenuEditorWindow
    {
        private static OdinMenuTree EditorWindow;

        [MenuItem("Tools/JFramework/CurrentProjectPath")]
        private static void CurrentProjectPath()
        {
            Process.Start(Environment.CurrentDirectory);
        }

        [MenuItem("Tools/JFramework/PersistentDataPath")]
        private static void PersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Tools/JFramework/StreamingAssetsPath")]
        private static void StreamingAssetsPath()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Process.Start(Application.streamingAssetsPath);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                Process.Start(Application.streamingAssetsPath);
            }
        }
        
        protected static void ShowEditorWindow<T>() where T : EditorWindow
        {
            var window = GetWindow<T>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        /// <summary>
        /// Odin窗口面板
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            EditorWindow = new OdinMenuTree(supportsMultiSelect: false);
            EditorWindow.Add("资源", FrameworkEditorAsset.Instance, EditorIcons.Folder);
            EditorWindow.Add("主页", FrameworkEditorHouse.Instance, EditorIcons.House);
            EditorWindow.Add("设置", Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault(), EditorIcons.SettingsCog);
            JFrameworkWindow(EditorWindow);
            return EditorWindow;
        }

        /// <summary>
        /// 重新绘制框架窗口面板
        /// </summary>
        /// <param name="window"></param>
        protected abstract void JFrameworkWindow(OdinMenuTree window);
    }
}