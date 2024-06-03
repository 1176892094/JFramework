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
using JFramework.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework.Core
{
    public static partial class SceneManager
    {
        internal static readonly Dictionary<Type, IEntity> objects = new();
        public static bool isLoading { get; private set; }
        public static Scene current => UnitySceneManager.GetActiveScene();
        public static string name => current.name;

        public static void Add<T>(T entity) where T : IEntity
        {
            if (!GlobalManager.Instance) return;
            objects.TryAdd(typeof(T), entity);
        }

        public static T Get<T>() where T : IEntity
        {
            if (!GlobalManager.Instance) return default;
            return (T)objects.GetValueOrDefault(typeof(T));
        }

        public static void Remove<T>() where T : IEntity
        {
            if (!GlobalManager.Instance) return;
            objects.Remove(typeof(T));
        }

        internal static void UnRegister()
        {
            objects.Clear();
            isLoading = false;
        }
    }

    public static partial class SceneManager
    {
        public static async void Load(string name)
        {
            if (!GlobalManager.Instance) return;
            try
            {
                isLoading = true;
                var newScene = await AssetManager.LoadScene(SettingManager.GetScenePath(name));
                await UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                isLoading = false;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public static async void Load(string name, Action action)
        {
            if (!GlobalManager.Instance) return;
            try
            {
                isLoading = true;
                var newScene = await AssetManager.LoadScene(SettingManager.GetScenePath(name));
                await UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                isLoading = false;
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public static async void Load(string name, Action<AsyncOperation> action)
        {
            if (!GlobalManager.Instance) return;
            try
            {
                isLoading = true;
                var newScene = await AssetManager.LoadScene(SettingManager.GetScenePath(name));
                var operation = UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                action?.Invoke(operation);
                await operation;
                isLoading = false;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }
    }
}