using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework
{
    public static class SceneManager
    {
        /// <summary>
        /// 场景列表
        /// </summary>
        internal static Dictionary<int, SceneData> sceneDict;

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(SceneManager);

        /// <summary>
        /// 当前场景名称
        /// </summary>
        public static string Scene => UnitySceneManager.GetActiveScene().name;

        /// <summary>
        /// 场景管理器初始化
        /// </summary>
        internal static void Awake()
        {
            sceneDict = new Dictionary<int, SceneData>();
            for (var i = 0; i < UnitySceneManager.sceneCountInBuildSettings; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var data = path.Split('/');
                var name = data[^1];
                data = name.Split('.');
                name = data[0];
                sceneDict.Add(i, new SceneData(i, name));
            }
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        public static void LoadScene(string name)
        {
            if (sceneDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化!");
                return;
            }

            if (GlobalManager.Instance.IsDebugScene)
            {
                Debug.Log($"{Name.Sky()} 加载 => {name.Green()}场景");
            }

            UnitySceneManager.LoadScene(name);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="action">场景加载完成的回调</param>
        public static void LoadSceneAsync(string name, Action action)
        {
            if (sceneDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化!");
                return;
            }

            if (GlobalManager.Instance.IsDebugScene)
            {
                Debug.Log($"{Name.Sky()} 异步加载 => {name.Green()}场景");
            }

            GlobalManager.Instance.StartCoroutine(LoadSceneCompleted(name, action));
        }

        /// <summary>
        /// 通过携程异步加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="action">场景加载完成的回调</param>
        /// <returns>返回场景加载迭代器</returns>
        private static IEnumerator LoadSceneCompleted(string name, Action action)
        {
            var asyncOperation = UnitySceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                var progress = 0f;
                while (progress < 0.999f)
                {
                    progress = Mathf.Lerp(progress, asyncOperation.progress / 9f * 10f, Time.deltaTime);
                    EventManager.Send(999, progress);
                    if (GlobalManager.Instance.IsDebugScene)
                    {
                        Debug.Log($"{Name.Sky()} 加载进度条 => {progress}%");
                    }

                    yield return new WaitForEndOfFrame();
                }

                asyncOperation.allowSceneActivation = true;
                yield return null;
            }

            action?.Invoke();

            if (GlobalManager.Instance.IsDebugScene)
            {
                Debug.Log($"{Name.Sky()} 加载 => {name.Green()}场景完成");
            }
        }

        /// <summary>
        /// 清空场景管理器
        /// </summary>
        internal static void Destroy() => sceneDict = null;
    }
}