using System.IO;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 单例模式对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class EditorSingleton<T> : ScriptableObject where T : EditorSingleton<T>
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
                string name = typeof(T).Name;
                instance = AssetDatabase.LoadAssetAtPath<T>(EditorConst.EditorPath + $"/{name}.asset");
                if (instance != null) return instance;
                instance = CreateInstance<T>();
                if (!Directory.Exists(EditorConst.EditorPath))
                {
                    Directory.CreateDirectory(EditorConst.EditorPath);
                }

                AssetDatabase.CreateAsset(instance, EditorConst.EditorPath + $"/{name}.asset");
                AssetDatabase.Refresh();
                return instance;
            }
        }
    }
}