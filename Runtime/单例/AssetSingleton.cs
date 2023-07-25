using System.IO;
using JFramework.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JFramework
{
    /// <summary>
    /// 调用后会寻找Settings文件夹下的ScriptableObject文件
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
                var name = typeof(T).Name;
                instance ??= AssetManager.Load<T>($"Settings/{name}");
                if (instance != null) return instance;
#if UNITY_EDITOR
                var path = "Assets/AddressableResources/Settings";
                var asset = $"{path}/{name}.asset";
                instance = AssetDatabase.LoadAssetAtPath<T>(asset);
                if (instance != null) return instance;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(Instance, asset);
                AssetDatabase.Refresh();
                Debug.Log($"创建 <color=#00FF00>{name}</color> 单例资源。路径: <color=#FFFF00>{path}</color>");
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