// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:44
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 调用 Instance 会寻找 Settings 文件夹下的 ScriptableObject 文件
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class AssetSingleton<T> : ScriptableObject where T : AssetSingleton<T>
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 实现安全的单例调用
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                if (GlobalManager.Runtime)
                {
                    instance ??= AssetManager.Load<T>($"Settings/{typeof(T).Name}");
                    if (instance != null) return instance;
                }
#if UNITY_EDITOR
                instance = Editor.EditorSetting.Register<T>();
#endif
                return instance;
            }
        }

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake() => instance ??= (T)this;

        /// <summary>
        /// 释放时将单例置空
        /// </summary>
        public void OnDestroy() => instance = null;
    }
}