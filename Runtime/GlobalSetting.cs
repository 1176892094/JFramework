// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework.Common
{
    internal partial class GlobalSetting : JFramework.GlobalSetting
    {
        public static GlobalSetting singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GlobalSetting>(nameof(GlobalSetting));
                }

#if UNITY_EDITOR
                if (instance == null)
                {
                    var assetPath = Service.Text.Format("Assets/{0}", nameof(Resources));
                    instance = CreateInstance<GlobalSetting>();
                    if (!Directory.Exists(assetPath))
                    {
                        Directory.CreateDirectory(assetPath);
                    }

                    assetPath = Service.Text.Format("{0}/{1}.asset", assetPath, nameof(GlobalSetting));
                    UnityEditor.AssetDatabase.CreateAsset(instance, assetPath);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
#endif
                return (GlobalSetting)instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            var canvas = new GameObject(nameof(UIManager)).AddComponent<Canvas>();
            canvas.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.referencePixelsPerUnit = 64;
            DontDestroyOnLoad(canvas);
            var manager = new GameObject(nameof(PoolManager)).AddComponent<GlobalManager>();
            manager.canvas = canvas;
            manager.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}