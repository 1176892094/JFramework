using System.IO;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 基于ScriptableObject的单例对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public class AssetSingleton<T> : ScriptableObject where T : AssetSingleton<T>
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
                var name = $"Assets/Editor/{typeof(T).Name}.asset";
                instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(name);
                if (instance != null) return instance;
                instance = CreateInstance<T>();
                if (!Directory.Exists("Assets/Editor"))
                {
                    Directory.CreateDirectory("Assets/Editor");
                }

                UnityEditor.AssetDatabase.CreateAsset(instance, name);
                UnityEditor.AssetDatabase.Refresh();
#else
                instance = AssetManager.Load<T>("Settings/"+typeof(T).Name);
                if (instance == null)
                {
                    Debug.LogWarning($"请将{typeof(T).Name}放入AddressableResources/Settings/文件夹中！");
                }
#endif
                return instance;
            }
        }
    }
}