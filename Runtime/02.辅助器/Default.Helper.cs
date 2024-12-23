// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 02:12:55
// # Recently: 2024-12-22 20:12:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal sealed partial class DefaultHelper
    {
        private static readonly Dictionary<int, RectTransform> panelLayers = new Dictionary<int, RectTransform>();
        private static readonly Dictionary<string, GameObject> assetPools = new Dictionary<string, GameObject>();
        private static readonly HashSet<AudioSource> audioSources = new HashSet<AudioSource>();
        private Canvas canvas;
        private GameObject manager;
        private AssetBundleManifest manifest;
        private AudioSource musicSource;

        void IBaseHelper.Log(string message)
        {
            Debug.Log(message);
        }

        void IBaseHelper.Warn(string message)
        {
            Debug.LogWarning(message);
        }

        void IBaseHelper.Error(string message)
        {
            Debug.LogError(message);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            assetPools.Clear();
            panelLayers.Clear();
            audioSources.Clear();
            var manager = new GameObject(nameof(GlobalManager)).AddComponent<GlobalManager>();
            var enabled = GlobalSetting.Instance.debugWindow == GlobalSetting.DebugWindow.Enable;
            manager.gameObject.AddComponent<DebugManager>().enabled = enabled;
        }
    }
}