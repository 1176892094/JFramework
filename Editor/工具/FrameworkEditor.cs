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
        private static OdinMenuTree editorWindow;
        
        //[MenuItem("Tools/JFramework/JFrameworkEditor _F1")]
        private static void JFrameworkEditor() => ShowEditorWindow<FrameworkEditor>();

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
            editorWindow = new OdinMenuTree(supportsMultiSelect: false);
            editorWindow.Add("资源", FrameworkAssetEditor.Instance, EditorIcons.Folder);
            editorWindow.Add("主页", FrameworkHouseEditor.Instance, EditorIcons.House);
            editorWindow.Add("设置", Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault(), EditorIcons.SettingsCog);
            JFrameworkWindow(editorWindow);
            return editorWindow;
        }

        /// <summary>
        /// 重新绘制框架窗口面板
        /// </summary>
        /// <param name="window"></param>
        protected abstract void JFrameworkWindow(OdinMenuTree window);
    }
}