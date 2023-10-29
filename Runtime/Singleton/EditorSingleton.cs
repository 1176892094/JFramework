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
using JFramework.Editor;
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
            instance = EditorSetting.Register<T>(AssetSetting.Instance.editorPath);
            return instance;
        }
    }
}
#endif