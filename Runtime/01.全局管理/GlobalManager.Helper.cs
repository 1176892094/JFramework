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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed class GlobalHelper : Service.Helper
    {
        string Service.Helper.assetPlatform => GlobalSetting.Instance.assetPlatform.ToString();
        bool Service.Helper.assetPackMode => GlobalSetting.Instance.assetPackMode == GlobalSetting.AssetPackMode.Authentic;
        string Service.Helper.assetPackPath => GlobalSetting.Instance.assetPackPath;
        string Service.Helper.assetPackName => GlobalSetting.Instance.assetPackName;
        string Service.Helper.assetRemotePath => GlobalSetting.Instance.assetRemotePath;
        string Service.Helper.assemblyName => "HotUpdate.Data";

        string Service.Helper.scriptDataPath
        {
            get
            {
#if UNITY_EDITOR
                return GlobalSetting.ScriptPath;
#else
                return string.Empty;
#endif
            }
        }

        string Service.Helper.assetDataPath
        {
            get
            {
#if UNITY_EDITOR
                return GlobalSetting.DataTablePath;
#else
                return string.Empty;
#endif
            }
        }

        void Service.Helper.CreateAsset(Object assetData, string assetPath)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset((ScriptableObject)assetData, assetPath);
#endif
        }

        void Service.Helper.CreateProgress(string assetPath, float progress)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayProgressBar(assetPath, "", progress);
#endif
        }

        async Task<KeyValuePair<int, string>> Service.Helper.LoadRequest(string persistentData, string streamingAssets)
        {
            if (File.Exists(persistentData))
            {
                return new KeyValuePair<int, string>(1, persistentData);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Head(streamingAssets);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return new KeyValuePair<int, string>(2, streamingAssets);
            }
#else
            if (File.Exists(streamingAssets))
            {
                return new KeyValuePair<int, string>(1, streamingAssets);
            }

#endif
            await Task.CompletedTask;
            return new KeyValuePair<int, string>(0, string.Empty);
        }

        Object Service.Helper.LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack)
        {
            if (assetPack == null) return null;
            var request = assetPack.LoadAssetAsync(assetPath, assetType);
            return request.asset is GameObject ? Object.Instantiate(request.asset) : request.asset;
        }

        Object Service.Helper.LoadByResources(string assetPath, Type assetType)
        {
            var request = Resources.Load(assetPath, assetType);
            return request is GameObject ? Object.Instantiate(request) : request;
        }

        Object Service.Helper.LoadBySimulates(string assetPath, Type assetType)
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