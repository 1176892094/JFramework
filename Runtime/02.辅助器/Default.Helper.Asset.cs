// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:42
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IAssetHelper
    {
        async void IAssetHelper.LoadScene(string assetPath)
        {
            Service.Event.Invoke(new SceneAwakeEvent(assetPath));
            var request = SceneManager.LoadSceneAsync(assetPath, LoadSceneMode.Single);
            if (request != null)
            {
                while (!request.isDone && GlobalManager.Instance)
                {
                    Service.Event.Invoke(new SceneUpdateEvent(request.progress));
                    await Task.Yield();
                }
            }

            Service.Event.Invoke(new SceneCompleteEvent());
        }

        string[] IAssetHelper.GetAllDependency(object assetPack, string assetPath)
        {
            if (manifest != null) return manifest.GetAllDependencies(assetPath);
            manifest = ((AssetBundle)assetPack).LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            Service.Event.Invoke(new AssetAwakeEvent(manifest.GetAllAssetBundles()));
            return manifest.GetAllDependencies(assetPath);
        }

        string[] IAssetHelper.GetAllAssetPacks(object assetPack)
        {
            if (manifest != null) return manifest.GetAllAssetBundles();
            manifest = ((AssetBundle)assetPack).LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            Service.Event.Invoke(new AssetAwakeEvent(manifest.GetAllAssetBundles()));
            return manifest.GetAllAssetBundles();
        }

        string[] IAssetHelper.GetAllScenePaths(object assetPack)
        {
            return ((AssetBundle)assetPack).GetAllScenePaths();
        }

        object IAssetHelper.LoadByAssetPack(string assetPath, Type assetType, object assetPack)
        {
            if (assetPack == null) return null;
            var request = ((AssetBundle)assetPack).LoadAssetAsync(assetPath, assetType);
            return request.asset is GameObject ? Object.Instantiate(request.asset) : request.asset;
        }

        object IAssetHelper.LoadByResources(string assetPath, Type assetType)
        {
            var request = Resources.Load(assetPath, assetType);
            return request is GameObject ? Object.Instantiate(request) : request;
        }

        object IAssetHelper.LoadBySimulates(string assetPath, Type assetType)
        {
#if UNITY_EDITOR
            if (!EditorSetting.objects.TryGetValue(assetPath, out var assetData)) return null;
            var request = UnityEditor.AssetDatabase.LoadAssetAtPath(assetData, assetType);
            return request is GameObject ? Object.Instantiate(request) : request;
#else
            return null;
#endif
        }
    }
}