using System;
using System.Threading.Tasks;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework.Core
{
    public static class SceneManager
    {
        /// <summary>
        /// 场景加载事件(进度条)
        /// </summary>
        public static event Action<float> OnLoadProgress;

        /// <summary>
        /// 当前场景名称
        /// ReSharper disable once MemberCanBePrivate.Global
        /// </summary>
        public static string localScene => UnitySceneManager.GetActiveScene().name;

        /// <summary>
        /// 异步加载场景的进度条
        /// </summary>
        /// <param name="path">场景名称</param>
        /// <param name="action"></param>
        /// <returns>返回场景加载迭代器</returns>
        public static async void LoadSceneAsync(string path, Action action = null)
        {
            try
            {
                if (!GlobalManager.Runtime) return;
                var localTime = Time.time;
                Log.Info($"异步加载 {path.Green()} 场景", Option.Scene);
                var operation = await AssetManager.LoadSceneAsync(path);
                while (!operation.isDone)
                {
                    OnLoadProgress?.Invoke(operation.progress);
                    Log.Info($"加载进度 {operation.progress.ToString("P").Green()}", Option.Scene);
                    if (!GlobalManager.Runtime) return;
                    await Task.Yield();
                }

                var totalTime = (Time.time - localTime).ToString("F");
                Log.Info($"异步加载 {path.Green()} 场景完成, 耗时 {totalTime.Yellow()} 秒", Option.Scene);
                action?.Invoke();
            }
            catch (Exception e)
            {
                Log.Info($"异步加载 {path.Red()} 场景失败\n{e}", Option.Scene);
            }
        }

        /// <summary>
        /// 场景管理器在开始游戏前清空
        /// </summary>
        internal static void Clear()
        {
            OnLoadProgress = null;
        }
    }
}