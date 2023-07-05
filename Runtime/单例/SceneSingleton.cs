using System;
using JFramework.Core;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    /// <summary>
    /// 调用后会在场景中寻找相应的游戏对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
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
                if (!GlobalManager.Runtime) return null;
                if (instance == null)
                {
                    lock (locked)
                    {
                        instance ??= FindObjectOfType<T>();
                    }

                    instance.Awake();
                }

                return instance;
            }
        }

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        private void OnDestroy()
        {
            try
            {
                instance = null;
                Despawn();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        protected virtual void Despawn()
        {
        }
    }
}