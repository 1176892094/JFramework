// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 19:12:47
// # Recently: 2024-12-22 20:12:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IPoolHelper
    {
        bool IPoolHelper.IsEntity(IEntity entity)
        {
            if (entity is Component component)
            {
                return component != null;
            }

            return false;
        }

        bool IPoolHelper.IsActive(IEntity entity)
        {
            if (entity is Behaviour component)
            {
                return component.isActiveAndEnabled;
            }

            return false;
        }
        
        async Task<object> IPoolHelper.Instantiate(string assetPath, Type assetType)
        {
            var assetData = await Service.Asset.Load<GameObject>(assetPath);
            assetData.name = Service.Text.Format("{0}", assetPath);
            var component = assetData.GetComponent(assetType);
            if (component == null)
            {
                component = assetData.AddComponent(assetType);
            }

            Object.DontDestroyOnLoad(assetData);
            return component;
        }

        void IPoolHelper.OnDequeue(IEntity assetData)
        {
            var transform = assetData.GetComponent<Transform>();
            transform.SetParent(null);
            transform.gameObject.SetActive(true);
        }

        string IPoolHelper.OnEnqueue(IEntity assetData)
        {
            var transform = assetData.GetComponent<Transform>();
            return LoadParent(transform);
        }

        private string LoadParent(Transform transform)
        {
            var assetPath = transform.name;
            if (manager == null)
            {
                manager = new GameObject("PoolManager");
                Object.DontDestroyOnLoad(manager);
            }

            if (!assetPools.TryGetValue(assetPath, out var parent))
            {
                parent = new GameObject(Service.Text.Format("Pool - {0}", assetPath));
                parent.transform.SetParent(manager.transform);
                assetPools.Add(assetPath, parent);
            }

            transform.SetParent(parent.transform);
            transform.gameObject.SetActive(false);
            return assetPath;
        }
    }
}