using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace JFramework.Core
{
    /// <summary>
    /// 场景加载管理器
    /// </summary>
    public class LoadManager : Singleton<LoadManager>
    {

        /// <summary>
        /// 通过接口当前场景Id
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("当前场景标识"), FoldoutGroup("场景管理视图")]
        public int Id => SceneManager.GetActiveScene().buildIndex;

       
        /// <summary>
        /// 通过接口当前场景名称
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("当前场景名称"), FoldoutGroup("场景管理视图")]
        public string Name => SceneManager.GetActiveScene().name;
        
        /// <summary>
        /// 存储所有场景的名称
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("游戏场景列表"), FoldoutGroup("场景管理视图")]
        private List<SceneData> sceneList;
        
        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            sceneList = new List<SceneData>();
            var count = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < count; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var strArray = path.Split('/');
                var str = strArray[^1];
                strArray = str.Split('.');
                str = strArray[0];
                SceneData data = new SceneData()
                {
                    Name = str,
                    ID = i
                };
                sceneList.Add(data);
            }
        }

        /// <summary>
        /// 通过接口加载指定场景(同步)
        /// </summary>
        /// <param name="name">传入加载的场景名称</param>
        public void Load(string name) => SceneManager.LoadScene(name);

        /// <summary>
        /// 通过接口异步加载场景(异步)
        /// </summary>
        /// <param name="name">传入加载的场景名称</param>
        /// <param name="action">传入场景加载完成的回调</param>
        public void LoadAsync(string name, Action action)
        {
            GlobalManager.Instance.StartCoroutine(LoadSceneCompleted(name, action));
        }

        /// <summary>
        /// 通过携程异步等待加载场景完成
        /// </summary>
        /// <param name="name">传入加载的场景名称</param>
        /// <param name="action">传入场景加载完成的回调</param>
        /// <returns>返回场景加载的迭代器</returns>
        private IEnumerator LoadSceneCompleted(string name, Action action)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                float progress = 0;
                while (progress < 0.999f)
                {
                    progress = Mathf.Lerp(progress, asyncOperation.progress / 9f * 10f, Time.deltaTime);
                    EventManager.Instance.Send(Global.LoadSceneAsync, progress);
                    yield return new WaitForEndOfFrame();
                }

                asyncOperation.allowSceneActivation = true;
                yield return null;
            }

            action?.Invoke();
            Debug.Log($"SceneManager异步加载 {name} 场景完成!");
        }

        /// <summary>
        /// 场景数据
        /// </summary>
        private struct SceneData
        {
            /// <summary>
            /// 场景ID
            /// </summary>
            public int ID;

            /// <summary>
            /// 场景名称
            /// </summary>
            public string Name;
        }
    }
}