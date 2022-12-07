using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = JFramework.Basic.Logger;
using Object = UnityEngine.Object;

namespace JFramework
{
    public class AssetManager: Singleton<AssetManager>
    {
        private readonly Dictionary<string, AssetBundle> assetDict = new Dictionary<string, AssetBundle>();
        private AssetBundleManifest manifest;
        private AssetBundle asset;
        private string PathURL => Application.streamingAssetsPath + "/";

        private string MainName
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

        private void LoadAsset(string packageName)
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

        public Object Load(string packageName, string assetName, Type type)
        {
            LoadAsset(packageName);
            Object obj = assetDict[packageName].LoadAsset(assetName, type);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        public T Load<T>(string packageName, string assetName) where T : Object
        {
            LoadAsset(packageName);
            T obj = assetDict[packageName].LoadAsset<T>(assetName);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        public void LoadAsync(string packageName, string assetName, Type type, Action<Object> callback)
        {
            MonoManager.Instance.StartCoroutine(LoadCompleted(packageName, assetName, type, callback));
        }

        private IEnumerator LoadCompleted(string packageName, string assetName, Type type, Action<Object> callback)
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

        public void LoadAsync<T>(string packageName, string assetName, Action<T> callback) where T : Object
        {
            MonoManager.Instance.StartCoroutine(LoadCompleted(packageName, assetName, callback));
        }

        private IEnumerator LoadCompleted<T>(string packageName, string assetName, Action<T> callback) where T : Object
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

        public void UnLoad(string name)
        {
            if (assetDict.ContainsKey(name))
            {
                assetDict[name].Unload(false);
                assetDict.Remove(name);
            }
        }

        public void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            assetDict.Clear();
            manifest = null;
            asset = null;
        }
    }
}