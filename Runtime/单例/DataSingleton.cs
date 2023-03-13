using System.IO;
using JFramework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 单例模式对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class DataSingleton<T> : ScriptableObject where T : DataSingleton<T>
    {
        /// <summary>
        /// 所属单例对象
        /// </summary>
        private static T instance;

        /// <summary>
        /// 返回单例对象
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
#if UNITY_EDITOR
                instance = FindObjectOfType<T>();
                if (instance != null) return instance;
                var name = typeof(T).Name;
                var path = "Assets/Editor";
                instance = AssetDatabase.LoadAssetAtPath<T>(path + $"/{name}.asset");
                if (instance != null) return instance;
                instance = CreateInstance<T>();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                AssetDatabase.CreateAsset(instance, path + $"/{name}.asset");
                AssetDatabase.Refresh();
#else
                instance = AssetManager.Instance.Load<T>("Setttngs/"+typeof(T).Name);
#endif
                return instance;
            }
        }
    }
}