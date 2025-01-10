// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using JFramework;
using UnityEngine;

internal partial class GlobalSetting : JFramework.GlobalSetting
{
    private static GlobalSetting instance;
    public string assetCachePath = "Assets/Template";
    public DebugMode debugWindow = DebugMode.Disable;
    
    public static GlobalSetting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GlobalSetting>(nameof(GlobalSetting));
            }

#if UNITY_EDITOR
            if (instance == null)
            {
                var assetPath = Service.Text.Format("Assets/{0}", nameof(Resources));
                instance = CreateInstance<GlobalSetting>();
                if (!Directory.Exists(assetPath))
                {
                    Directory.CreateDirectory(assetPath);
                }

                assetPath = Service.Text.Format("{0}/{1}.asset", assetPath, nameof(GlobalSetting));
                UnityEditor.AssetDatabase.CreateAsset(instance, assetPath);
                UnityEditor.AssetDatabase.SaveAssets();
            }
#endif
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RuntimeInitializeOnLoad()
    {
        var manager = new GameObject(nameof(GlobalManager)).AddComponent<GlobalManager>();
        var enabled = Instance.debugWindow == DebugMode.Enable;
        manager.gameObject.AddComponent<DebugManager>().enabled = enabled;
    }

    public enum DebugMode
    {
        Enable,
        Disable,
    }

    public enum BuildMode : byte
    {
        StreamingAssets,
        BuildPath,
    }
}