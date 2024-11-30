// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  22:47
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static readonly object locked = typeof(T);

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locked)
                    {
                        instance ??= FindFirstObjectByType<T>();
                        if (instance is IAutoRegister)
                        {
                            instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                        }

                        instance?.Awake();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
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

            if (this is IDontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}