// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 20:12:42
// # Recently: 2024-12-22 20:12:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IPanelHelper
    {
        async Task<object> IPanelHelper.Instantiate(string assetPath, Type assetType)
        {
            var assetData = await Service.Asset.Load<GameObject>(assetPath);
            assetData.name = Service.Text.Format("{0}", assetPath);
            var component = assetData.GetComponent(assetType);
            if (component == null)
            {
                component = assetData.AddComponent(assetType);
            }

            return component;
        }

        void IPanelHelper.Destroy(IPanel assetData)
        {
            assetData.Hide();
            var transform = assetData.GetComponent<Transform>();
            transform.gameObject.SetActive(false);
            Object.Destroy(transform.gameObject);
        }

        void IPanelHelper.Surface(IPanel assetData, int layer)
        {
            if (canvas == null)
            {
                canvas = new GameObject("UIManager").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                Object.DontDestroyOnLoad(canvas.gameObject);

                var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0.5f;
            }

            if (!panelLayers.TryGetValue(layer, out var parent))
            {
                var name = Service.Text.Format("Layer-{0}", layer);
                var child = new GameObject(name);
                child.transform.SetParent(canvas.transform);
                var renderer = child.AddComponent<Canvas>();
                renderer.overrideSorting = true;
                renderer.sortingOrder = layer;
                parent = child.GetComponent<RectTransform>();
                parent.anchorMin = Vector2.zero;
                parent.anchorMax = Vector2.one;
                parent.offsetMin = Vector2.zero;
                parent.offsetMax = Vector2.zero;
                parent.localScale = Vector3.one;
                panelLayers.Add(layer, parent);
                parent.SetSiblingIndex(layer);
            }

            var transform = assetData.GetComponent<RectTransform>();
            transform.SetParent(parent);
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;
            transform.localScale = Vector3.one;
        }
    }
}