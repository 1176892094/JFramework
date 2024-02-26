// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  17:54
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework.Core
{
    public sealed class SceneManager : ScriptableObject
    {
        [ShowInInspector, LabelText("场景对象")] private readonly Dictionary<Type, IEntity> objects = new Dictionary<Type, IEntity>();

        public void Register<T>(T entity) where T : IEntity
        {
            if (!GlobalManager.Instance) return;
            if (!objects.ContainsKey(typeof(T)))
            {
                objects.Add(typeof(T), entity);
            }
        }

        public T Get<T>() where T : IEntity
        {
            if (!GlobalManager.Instance) return default;
            return objects.TryGetValue(typeof(T), out var entity) ? (T)entity : default;
        }

        public void UnRegister<T>() where T : IEntity
        {
            if (!GlobalManager.Instance) return;
            if (objects.ContainsKey(typeof(T)))
            {
                objects.Remove(typeof(T));
            }
        }

        public async Task LoadScene(string name)
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var operation = await GlobalManager.Asset.LoadSceneAsync(SettingManager.GetScenePath(name));
                await operation;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public async void LoadSceneAsync(string name, Action<AsyncOperation> action = null)
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var operation = await GlobalManager.Asset.LoadSceneAsync(SettingManager.GetScenePath(name));
                action?.Invoke(operation);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public override string ToString()
        {
            return UnitySceneManager.GetActiveScene().name;
        }

        internal void OnDisable()
        {
            objects.Clear();
        }
    }
}