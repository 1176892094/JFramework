// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:02
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
        /// </summary>
        public static string localScene => UnitySceneManager.GetActiveScene().name;

        /// <summary>
        /// 异步加载场景的进度条
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="action"></param>
        /// <returns>返回场景加载迭代器</returns>
        public static void LoadSceneAsync(string name, Action action = null)
        {
            try
            {
                if (!GlobalManager.Runtime) return;
                AssetManager.LoadSceneAsync(GlobalSetting.Instance.sceneBundle + "/" + name, LoadProgress);

                async void LoadProgress(AsyncOperation operation)
                {
                    var localTime = Time.time;
                    while (!operation.isDone && GlobalManager.Runtime)
                    {
                        OnLoadProgress?.Invoke(operation.progress);
                        await Task.Yield();
                    }

                    var totalTime = (Time.time - localTime).ToString("F");
                    action?.Invoke();
                    Debug.Log($"异步加载 {name.Green()} 场景完成, 耗时 {totalTime.Yellow()} 秒");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
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