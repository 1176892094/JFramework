// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class AssetManager
    {
        private static class AssetBundleLoader
        {
            public static Object Load(AssetBundle assetBundle, string assetName, Type assetType)
            {
                if (assetBundle != null)
                {
                    var request = assetBundle.LoadAssetAsync(assetName, assetType);
                    return request.asset is GameObject ? Object.Instantiate(request.asset) : request.asset;
                }

                return null;
            }
        }

        private static class ResourcesLoader
        {
            public static Object Load(string assetPath, Type assetType)
            {
                var request = Resources.Load(assetPath, assetType);
                return request is GameObject ? Object.Instantiate(request) : request;
            }
        }

#if UNITY_EDITOR
        private static class SimulateLoader
        {
            public static Object Load(string assetPath, Type assetType)
            {
                if (EditorSetting.objects.TryGetValue(char.ToUpper(assetPath[0]) + assetPath.Substring(1), out assetPath))
                {
                    var request = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, assetType);
                    return request is GameObject ? Object.Instantiate(request) : request;
                }

                return null;
            }
        }
#endif
    }
}