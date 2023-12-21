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

namespace JFramework
{
    public sealed partial class GlobalManager
    {
        /// <summary>
        /// 场景管理器
        /// </summary>
        public class SceneManager : Controller
        {
            /// <summary>
            /// 异步加载场景的进度条
            /// </summary>
            /// <param name="name">场景名称</param>
            /// <returns>返回场景加载迭代器</returns>
            public async Task LoadSceneAsync(string name)
            {
                try
                {
                    if (!Runtime) return;
                    var localTime = UnityEngine.Time.time;
                    var operation = await Asset.LoadSceneAsync(GlobalSetting.GetScenePath(name));
                    await operation;
                    var totalTime = (UnityEngine.Time.time - localTime).ToString("F");
                    Debug.Log($"异步加载 {name.Green()} 场景完成, 耗时 {totalTime.Yellow()} 秒");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
                }
            }

            /// <summary>
            /// 异步加载场景
            /// </summary>
            /// <param name="name">场景名称</param>
            /// <param name="action"></param>
            /// <returns>返回场景加载迭代器</returns>
            public async void LoadSceneAsync(string name, Action<AsyncOperation> action)
            {
                try
                {
                    if (!Runtime) return;
                    var operation = await Asset.LoadSceneAsync(GlobalSetting.GetScenePath(name));
                    action?.Invoke(operation);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"异步加载 {name.Red()} 场景失败\n{e}");
                }
            }

            /// <summary>
            /// 转化成当前场景名称
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return UnitySceneManager.GetActiveScene().name;
            }
        }
    }
}