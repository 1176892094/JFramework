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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework
{
    public static class SceneManager
    {
        public static string name => UnitySceneManager.GetActiveScene().name;

        public static async void Load(string name)
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var newScene = await AssetManager.LoadScene(GlobalSetting.GetScenePath(name));
                await UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public static async void Load(string name, Action action)
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var newScene = await AssetManager.LoadScene(GlobalSetting.GetScenePath(name));
                await UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }

        public static async void Load(string name, Action<AsyncOperation> action)
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var newScene = await AssetManager.LoadScene(GlobalSetting.GetScenePath(name));
                var operation = UnitySceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                action?.Invoke(operation);
                await operation;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
            }
        }
    }
}