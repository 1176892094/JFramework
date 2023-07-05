using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework.Core
{
    public static class SceneManager
    {
        /// <summary>
        /// 场景加载事件(进度条)
        /// </summary>
        public static event Action<float> OnLoadScene;

        /// <summary>
        /// 当前场景名称
        /// </summary>
        public static string scene => UnitySceneManager.GetActiveScene().name;

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="path">场景名称</param>
        public static async Task LoadSceneAsync(string path)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Scene, $"异步加载 => {path.Green()} 场景");
            await OnLoadProgress(path, Time.time);
        }

        /// <summary>
        /// 异步加载场景的进度条
        /// </summary>
        /// <param name="path">场景名称</param>
        /// <param name="time">开始加载场景的时间</param>
        /// <returns>返回场景加载迭代器</returns>
        private static async Task OnLoadProgress(string path, float time)
        {
            var handle = Addressables.LoadSceneAsync(path);
            while (!handle.IsDone)
            {
                OnLoadScene?.Invoke(handle.PercentComplete);
                Log.Info(DebugOption.Scene, $"加载进度 => {handle.PercentComplete.ToString("P").Green()}");
                if (!GlobalManager.Runtime) return;
                await Task.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var totalTime = (Time.time - time).ToString("F");
                Log.Info(DebugOption.Scene, $"异步加载 => {path.Green()} 场景完成, 耗时 {totalTime.Yellow()} 秒");
            }
            else
            {
                Log.Info(DebugOption.Scene, $"异步加载 => {path.Red()} 场景失败");
            }
        }

        /// <summary>
        /// 管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            OnLoadScene = null;
        }
    }
}