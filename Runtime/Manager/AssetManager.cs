using System;
using System.Collections;
using System.Collections.Generic;
using JFramework.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static class AssetManager
    {
        private static readonly Dictionary<string, AssetBundle> assetDict = new Dictionary<string, AssetBundle>();
        private static AssetBundleManifest manifest;
        private static AssetBundle asset;
        private static string PathURL => Application.streamingAssetsPath + "/";

        private static string MainName
        {
            get
            {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
                return "PC";
#endif
            }
        }

        private static void LoadAsset(string packageName)
        {
            if (asset == null)
            {
                asset = AssetBundle.LoadFromFile(PathURL + MainName);
                manifest = asset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            AssetBundle assetBundle;
            string[] strArray = manifest.GetAllDependencies(packageName);
            foreach (var package in strArray)
            {
                if (!assetDict.ContainsKey(package))
                {
                    assetBundle = AssetBundle.LoadFromFile(PathURL + package);
                    assetDict.Add(package, assetBundle);
                }
            }

            if (!assetDict.ContainsKey(packageName))
            {
                assetBundle = AssetBundle.LoadFromFile(PathURL + packageName);
                assetDict.Add(packageName, assetBundle);
            }
        }

        public static Object Load(string packageName, string assetName, Type type)
        {
            LoadAsset(packageName);
            Object obj = assetDict[packageName].LoadAsset(assetName, type);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        public static T Load<T>(string packageName, string assetName) where T : Object
        {
            LoadAsset(packageName);
            T obj = assetDict[packageName].LoadAsset<T>(assetName);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        public static async void LoadAsync(string packageName, string assetName, Type type, Action<Object> callback)
        {
            await LoadCompleted(packageName, assetName, type, callback);
        }

        private static IEnumerator LoadCompleted(string packageName, string assetName, Type type,
            Action<Object> callback)
        {
            LoadAsset(packageName);
            AssetBundleRequest request = assetDict[packageName].LoadAssetAsync(assetName, type);
            yield return request;
            if (request == null) yield break;
            if (request.asset == null)
            {
                Logger.LogWarning(assetName + "未获取到！");
                yield break;
            }

            callback(request.asset is GameObject ? Object.Instantiate(request.asset) : request.asset);
        }

        public static async void LoadAsync<T>(string packageName, string assetName, Action<T> callback) where T : Object
        {
            await LoadCompleted(packageName, assetName, callback);
        }

        private static IEnumerator LoadCompleted<T>(string packageName, string assetName, Action<T> callback)
            where T : Object
        {
            LoadAsset(packageName);
            AssetBundleRequest request = assetDict[packageName].LoadAssetAsync<T>(assetName);
            yield return request;
            if (request == null) yield break;
            if (request.asset == null)
            {
                Logger.LogWarning(assetName + "未获取到！");
                yield break;
            }

            if (request.asset is GameObject)
            {
                callback((T)Object.Instantiate(request.asset));
            }
            else
            {
                callback((T)request.asset);
            }
        }

        public static void UnLoad(string name)
        {
            if (assetDict.ContainsKey(name))
            {
                assetDict[name].Unload(false);
                assetDict.Remove(name);
            }
        }

        public static void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            assetDict.Clear();
            manifest = null;
            asset = null;
        }
    }
}