// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 23:04:54
// // # Recently: 2025-04-09 23:04:54
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.IO;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    public abstract class EditorSingleton<T> : ScriptableObject where T : EditorSingleton<T>
    {
        private static T instance;

        private static string fileName => typeof(T).Name;

        private static string filePath => Service.Text.Format("{0}/{1}.asset", GlobalSetting.EditorPath, fileName);

        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = AssetDatabase.LoadAssetAtPath<T>(filePath);
                if (instance != null)
                {
                    return instance;
                }

                if (!Directory.Exists(GlobalSetting.EditorPath))
                {
                    Directory.CreateDirectory(GlobalSetting.EditorPath);
                }

                instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, filePath);
                AssetDatabase.Refresh();
                return instance;
            }
        }
    }
}