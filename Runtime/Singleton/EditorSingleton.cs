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

/// <summary>
/// 编辑器单例
/// </summary>
/// <typeparam name="T">所属的单例对象</typeparam>
public abstract class EditorSingleton<T> : ScriptableObject where T : EditorSingleton<T>
{
    /// <summary>
    /// 单例自身
    /// </summary>
    private static T instance;

    /// <summary>
    /// 安全的单例调用
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Resources.Load<T>(typeof(T).Name);
            if (instance != null) return instance;
            var asset = $"{AssetSetting.Instance.editorPath}/{typeof(T).Name}.asset";
            instance = AssetDatabase.LoadAssetAtPath<T>(asset);
            if (instance != null) return instance;
            if (!Directory.Exists(AssetSetting.Instance.editorPath))
            {
                Directory.CreateDirectory(AssetSetting.Instance.editorPath);
            }

            instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, asset);
            AssetDatabase.Refresh();
            return instance;
        }
    }
}
#endif