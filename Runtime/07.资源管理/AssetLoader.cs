// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class AssetManager
    {
        private static class AssetBundleLoader
        {
            public static T LoadAsync<T>(AssetBundle assetBundle, string assetName) where T : Object
            {
                if (assetBundle != null)
                {
                    var request = assetBundle.LoadAssetAsync<T>(assetName);
                    return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
                }

                return null;
            }
        }

        private static class ResourcesLoader
        {
            public static async Task<T> LoadAsync<T>(string assetPath) where T : Object
            {
                var request = await Resources.LoadAsync<T>(assetPath);
                return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
            }
        }

#if UNITY_EDITOR
        private static class SimulateLoader
        {
            public static T LoadAsync<T>(string assetPath) where T : Object
            {
                if (EditorSetting.objects.TryGetValue(char.ToUpper(assetPath[0]) + assetPath.Substring(1), out var editorPath))
                {
                    var request = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(editorPath);
                    return request is GameObject ? Object.Instantiate(request) : request;
                }

                return null;
            }
        }
#endif
    }
}