using System;

namespace JFramework
{
    /// <summary>
    /// 调用单例后自动创建一个单例对象
    /// </summary>
    /// <typeparam name="T">传入单例的所有者</typeparam>
    public abstract class Singleton<T> : IDisposable where T : Singleton<T>, new()
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object locked = typeof(T);

        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locked)
                    {
                        instance ??= new T();
                    }

                    instance.Awake();
                }

                return instance;
            }
        }

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake() => instance ??= (T)this;

        /// <summary>
        /// 释放单例
        /// </summary>
        public void Dispose() => instance = default;
    }
}