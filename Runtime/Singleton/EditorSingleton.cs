// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-26  12:47
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System.IO;
using JFramework.Editor;
using UnityEditor;
using UnityEngine;

// ReSharper disable All

public abstract class EditorSingleton<T> : ScriptableObject where T : EditorSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Resources.Load<T>(typeof(T).Name);
            if (instance != null) return instance;
            if (!Directory.Exists(AssetSetting.Instance.editorPath))
            {
                Directory.CreateDirectory(AssetSetting.Instance.editorPath);
            }

            instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, $"{AssetSetting.Instance.editorPath}/{typeof(T).Name}.asset");
            AssetDatabase.Refresh();
            return instance;
        }
    }
}
#endif