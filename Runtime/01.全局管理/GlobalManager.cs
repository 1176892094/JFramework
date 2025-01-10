// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 22:12:25
// # Recently: 2024-12-22 20:12:41
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    internal class GlobalManager : MonoBehaviour, IEntity
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Service.Entry.Register(new DefaultHelper());
        }

        private void Update()
        {
            Service.Entry.Update();
        }

        private void OnDestroy()
        {
            Service.Entry.UnRegister();
            AssetBundle.UnloadAllAssetBundles(true);
            GC.Collect();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            var manager = new GameObject(nameof(GlobalManager)).AddComponent<GlobalManager>();
            var enabled = GlobalSetting.Instance.debugWindow == GlobalSetting.DebugMode.Enable;
            manager.gameObject.AddComponent<DebugManager>().enabled = enabled;
        }
    }
}