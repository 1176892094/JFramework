// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:42
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class AssetManager
    {
        public static async void LoadAssetData()
        {
            var platform = await LoadAssetPack(GlobalSetting.platformPath);
            GlobalManager.manifest ??= platform.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            Service.Event.Invoke(new AssetAwake(GlobalManager.manifest.GetAllAssetBundles()));

            var assetPacks = GlobalManager.manifest.GetAllAssetBundles();
            foreach (var assetPack in assetPacks)
            {
                _ = LoadAssetPack(assetPack);
            }

            await Task.WhenAll(GlobalManager.assetTask.Values);
            Service.Event.Invoke(new AssetComplete());
        }

        public static async Task<T> Load<T>(string assetPath) where T : Object
        {
            try
            {
                if (!GlobalManager.Instance) return default;
                var assetData = await LoadAsset(assetPath, typeof(T));
                if (assetData != null)
                {
                    return (T)assetData;
                }

                Debug.LogWarning(Service.Text.Format("加载资源 {0} 为空!", assetPath));
            }
            catch (Exception e)
            {
                Debug.LogWarning(Service.Text.Format("加载资源 {0} 失败!\n{1}", assetPath, e));
            }

            return default;
        }

        public static async void Load<T>(string assetPath, Action<T> assetAction) where T : Object
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var assetData = await LoadAsset(assetPath, typeof(T));
                if (assetData != null)
                {
                    assetAction.Invoke((T)assetData);
                    return;
                }

                Debug.LogWarning(Service.Text.Format("加载资源 {0} 为空!", assetPath));
            }
            catch (Exception e)
            {
                Debug.LogWarning(Service.Text.Format("加载资源 {0} 失败!\n{1}", assetPath, e));
            }
        }

        private static async Task<Object> LoadAsset(string assetPath, Type assetType)
        {
            if (GlobalSetting.assetLoadMode)
            {
                var assetPair = await LoadAssetPair(assetPath);
                var assetPack = await LoadAssetPack(assetPair.Key);
                var assetData = GlobalSetting.Instance.LoadByAssetPack(assetPair.Value, assetType, assetPack);
                assetData ??= GlobalSetting.Instance.LoadByResources(assetPath, assetType);
                return assetData;
            }
            else
            {
                var assetData = GlobalSetting.Instance.LoadBySimulates(assetPath, assetType);
                assetData ??= GlobalSetting.Instance.LoadByResources(assetPath, assetType);
                return assetData;
            }
        }

        private static async Task<KeyValuePair<string, string>> LoadAssetPair(string assetPath)
        {
            if (!GlobalManager.assetData.TryGetValue(assetPath, out var assetData))
            {
                var index = assetPath.LastIndexOf('/');
                if (index < 0)
                {
                    assetData = new KeyValuePair<string, string>(string.Empty, assetPath);
                }
                else
                {
                    var assetPack = assetPath.Substring(0, index).ToLower();
                    assetData = new KeyValuePair<string, string>(assetPack, assetPath.Substring(index + 1));
                }

                GlobalManager.assetData.Add(assetPath, assetData);
            }

            var platform = await LoadAssetPack(GlobalSetting.platformPath);
            GlobalManager.manifest ??= platform.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));

            var assetPacks = GlobalManager.manifest.GetAllDependencies(assetData.Key);
            foreach (var assetPack in assetPacks)
            {
                _ = LoadAssetPack(assetPack);
            }

            return assetData;
        }

        private static async Task<AssetBundle> LoadAssetPack(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            if (GlobalManager.assetPack.TryGetValue(assetPath, out var assetPack))
            {
                return assetPack;
            }

            if (GlobalManager.assetTask.TryGetValue(assetPath, out var assetTask))
            {
                return await assetTask;
            }

            var persistentData = GlobalSetting.GetPacketPath(assetPath);
            var streamingAssets = GlobalSetting.GetClientPath(assetPath);
            assetTask = PackManager.LoadAssetRequest(persistentData, streamingAssets);
            GlobalManager.assetTask.Add(assetPath, assetTask);
            try
            {
                assetPack = await assetTask;
                GlobalManager.assetPack.Add(assetPath, assetPack);
                Service.Event.Invoke(new AssetUpdate(assetPath));
                return assetPack;
            }
            finally
            {
                GlobalManager.assetTask.Remove(assetPath);
            }
        }

        internal static void Dispose()
        {
            GlobalManager.assetData.Clear();
            GlobalManager.assetTask.Clear();
            GlobalManager.assetPack.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
        }
    }
}