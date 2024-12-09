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
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework
{
    public static partial class AssetManager
    {
        public static async Task LoadScene(string assetPath)
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadSceneAsset(GlobalSetting.GetScenePath(assetPath));
                    await SceneManager.LoadSceneAsync(assetData, LoadSceneMode.Single);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载场景 {assetPath.Red()} 失败\n" + e);
            }
        }

        public static async void LoadScene(string assetPath, Action<AsyncOperation> action)
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadSceneAsset(GlobalSetting.GetScenePath(assetPath));
                    var operation = SceneManager.LoadSceneAsync(assetData, LoadSceneMode.Single);
                    action?.Invoke(operation);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载场景 {assetPath.Red()} 失败\n" + e);
            }
        }

        private static async Task<string> LoadSceneAsset(string assetPath)
        {
            if (GlobalManager.mode == AssetMode.AssetBundle)
            {
                var dependency = await LoadDependency(assetPath);
                var assetBundle = await LoadAssetBundle(dependency.Key);
                var assetData = assetBundle.GetAllScenePaths();
                return assetData.FirstOrDefault(data => data == dependency.Value);
            }

            return assetPath.Substring(assetPath.LastIndexOf('/') + 1);
        }
    }
}