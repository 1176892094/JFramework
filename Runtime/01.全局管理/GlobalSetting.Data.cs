// *********************************************************************************
// # Project: Forest
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 01:01:38
// # Recently: 2025-01-11 01:01:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Common
{
    internal partial class GlobalSetting
    {
        protected override string scriptDataPath
        {
            get
            {
#if UNITY_EDITOR
                return ScriptPath;
#else
                return string.Empty;
#endif
            }
        }

        protected override string assetDataPath
        {
            get
            {
#if UNITY_EDITOR
                return DataTablePath;
#else
                return string.Empty;
#endif
            }
        }

#if UNITY_ANDROID
        private bool multicast;
        private AndroidJavaObject multicastLock;
#endif

        public override void MulticastLock(bool enabled)
        {
#if UNITY_ANDROID
            if (enabled)
            {
                if (multicast) return;
                using var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                using var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
                multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "lock");
                multicastLock.Call("acquire");
                multicast = true;
            }
            else
            {
                if (!multicast) return;
                multicastLock?.Call("release");
                multicast = false;
            }

#endif
        }

        public override void CreateAsset(Object assetData, string assetPath)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset((ScriptableObject)assetData, assetPath);
#endif
        }

        public override void CreateProgress(string assetPath, float progress)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayProgressBar(assetPath, "", progress);
#endif
        }

        public override Object LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack)
        {
            if (assetPack == null) return null;
            var request = assetPack.LoadAssetAsync(assetPath, assetType);
            return request.asset is GameObject ? Instantiate(request.asset) : request.asset;
        }

        public override Object LoadByResources(string assetPath, Type assetType)
        {
            var request = Resources.Load(assetPath, assetType);
            return request is GameObject ? Instantiate(request) : request;
        }

        public override Object LoadBySimulates(string assetPath, Type assetType)
        {
#if UNITY_EDITOR
            if (!EditorSetting.objects.TryGetValue(assetPath, out var assetData)) return null;
            var request = UnityEditor.AssetDatabase.LoadAssetAtPath(assetData, assetType);
            return request is GameObject ? Instantiate(request) : request;
#else
            return null;
#endif
        }

        public override async Task<KeyValuePair<int, string>> LoadRequest(string persistentData, string streamingAssets)
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
    }
}