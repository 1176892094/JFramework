using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 调用后会寻找Settings文件夹下的ScriptableObject文件
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class DataSingleton<T> : ScriptableObject where T : DataSingleton<T>
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 实现安全的单例调用
        /// </summary>
        public static T Instance => instance ??= AssetManager.Load<T>("Settings/" + typeof(T).Name);

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