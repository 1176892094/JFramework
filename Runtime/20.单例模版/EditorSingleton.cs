// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  22:46
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System.IO;
using JFramework;
using UnityEditor;
using UnityEngine;

public abstract class EditorSingleton<T> : ScriptableObject where T : EditorSingleton<T>
{
    private static T instance;

    private static string fileName => typeof(T).Name;

    private static string filePath => GlobalSetting.EditorPath + "/" + fileName + ".asset";

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
#endif