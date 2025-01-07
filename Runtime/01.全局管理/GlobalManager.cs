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
        public static GlobalManager Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Service.Entry.Register(new DefaultHelper());
        }

        private void Update()
        {
            Service.Entry.Update(Time.time, Time.unscaledTime);
        }

        private void OnDestroy()
        {
            Service.Entry.UnRegister();
            AssetBundle.UnloadAllAssetBundles(true);
            GC.Collect();
        }
    }
}