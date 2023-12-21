// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:43
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

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
                        instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                        instance.Register();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 初始化单例
        /// </summary>
        private void Register()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else if (instance != this)
            {
                Debug.LogWarning(typeof(T) + "单例重复！");
                Destroy(this);
            }
        }

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake() => Register();

        /// <summary>
        /// 销毁单例
        /// </summary>
        protected virtual void OnDestroy() => instance = null;
    }
}