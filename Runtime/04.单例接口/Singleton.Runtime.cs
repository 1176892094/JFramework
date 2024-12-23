// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-25 01:08:18
// # Recently: 2024-12-22 20:12:38
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
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
                        if ((instance.status & Status.Automatic) != 0)
                        {
                            instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                        }

                        instance?.Awake();
                    }
                }

                return instance;
            }
        }

        protected virtual Status status => Status.None;

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

            if ((instance.status & Status.Perpetual) != 0)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }

        [Flags]
        protected enum Status
        {
            None,
            Automatic = 1 << 0,
            Perpetual = 1 << 1,
        }
    }
}