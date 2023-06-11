using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using LoadManager = UnityEngine.SceneManagement.SceneManager;


namespace JFramework.Core
{
    public static class SceneManager
    {
        /// <summary>
        /// 场景列表
        /// </summary>
        internal static Dictionary<string, string> sceneDict;

        /// <summary>
        /// 场景加载事件(进度条)
        /// </summary>
        public static event Action<float> OnLoadScene;

        /// <summary>
        /// 场景加载进度条
        /// </summary>
        private static float progress;

        /// <summary>
        /// 场景加载进度条
        /// </summary>
        private static float Progress
        {
            get => progress;
            set
            {
                progress = value;
                if (progress >= 0.99f)
                {
                    progress = 1f;
                }
            }
        }

        /// <summary>
        /// 当前场景名称
        /// </summary>
        public static string scene => LoadManager.GetActiveScene().name;

        /// <summary>
        /// 场景管理器初始化
        /// </summary>
        internal static async void Awake()
        {
            var handle = Addressables.LoadResourceLocationsAsync("Scene", typeof(SceneInstance));
            await handle.Task;
            sceneDict = new Dictionary<string, string>();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var location in handle.Result)
                {
                    sceneDict.Add(location.PrimaryKey.Split('/')[1], location.PrimaryKey);
                }
            }

            Addressables.Release(handle);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="key">场景名称</param>
        public static async Task LoadSceneAsync(string key)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Scene, $"异步加载 => {sceneDict[key].Green()} 场景");
            await OnLoadProgress(sceneDict[key], Time.time);
        }

        /// <summary>
        /// 异步加载场景的进度条
        /// </summary>
        /// <param name="key">场景名称</param>
        /// <param name="time">开始加载场景的时间</param>
        /// <returns>返回场景加载迭代器</returns>
        private static async Task OnLoadProgress(string key, float time)
        {
            var handle = Addressables.LoadSceneAsync(key);
            while (!handle.IsDone)
            {
                Progress = 0;
                while (Progress < 0.99f)
                {
                    Progress = Mathf.Lerp(Progress, handle.PercentComplete / 9f * 10f, Time.fixedDeltaTime);
                    OnLoadScene?.Invoke(Progress);
                    Log.Info(DebugOption.Scene, $"加载进度 => {Progress.ToString("P").Green()}");
                    await Task.Yield();
                }

                if (!GlobalManager.Runtime) return;
                await Task.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var totalTime = (Time.time - time).ToString("F");
                Log.Info(DebugOption.Scene, $"异步加载 => {key.Green()} 场景完成, 耗时 {totalTime.Yellow()} 秒");
            }
            else
            {
                Log.Info(DebugOption.Scene, $"异步加载 => {key.Red()} 场景失败");
            }
        }

        /// <summary>
        /// 清空场景管理器
        /// </summary>
        internal static void Destroy()
        {
            sceneDict = null;
            OnLoadScene = null;
        }
    }
}