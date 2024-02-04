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

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Resources.Load<T>(typeof(T).Name);
            if (instance != null) return instance;
            var asset = $"{GlobalSetting.Instance.editorPath}/{typeof(T).Name}.asset";
            instance = AssetDatabase.LoadAssetAtPath<T>(asset);
            if (instance != null) return instance;
            if (!Directory.Exists(GlobalSetting.Instance.editorPath))
            {
                Directory.CreateDirectory(GlobalSetting.Instance.editorPath);
            }

            instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, asset);
            AssetDatabase.Refresh();
            return instance;
        }
    }
}
#endif