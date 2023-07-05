using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class PoolManager
    {
        internal static readonly Dictionary<string, IPool<GameObject>> poolDict = new Dictionary<string, IPool<GameObject>>();
        
        internal static readonly Dictionary<Type, IPool> streamDict = new Dictionary<Type, IPool>();

        internal static Transform poolManager;

        internal static void Awake()
        {
            var transform = GlobalManager.Instance.transform;
            poolManager = transform.Find("PoolManager");
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="path">弹出对象的路径</param>
        public static async Task<T> Pop<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            var key = path.Substring(path.LastIndexOf('/') + 1);
            if (poolDict.ContainsKey(key) && poolDict[key].Count > 0)
            {
                var poolObj = poolDict[key].Pop();
                if (poolObj != null)
                {
                    Log.Info(DebugOption.Pool, $"取出 => {key.Pink()} 对象成功");
                    return poolObj.GetComponent<T>();
                }

                Log.Info(DebugOption.Pool, $"移除已销毁对象 : {key.Red()}");
            }

            var obj = await AssetManager.LoadAsync<GameObject>(path);
            obj.name = key;
            Log.Info(DebugOption.Pool, $"创建 => {key.Green()} 对象成功");
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public static void Push(GameObject obj)
        {
            if (!GlobalManager.Runtime) return;
            if (obj == null) return;
            var key = obj.name;

            if (poolDict.ContainsKey(key))
            {
                if (obj == null)
                {
                    Debug.LogWarning($"{nameof(PoolManager).Sky()} 移除已销毁对象 : {key.Red()}");
                    poolDict[key].Pop();
                    return;
                }

                Log.Info(DebugOption.Pool, $"存入 => {key.Pink()} 对象成功");
                poolDict[key].Push(obj);
            }
            else
            {
                Log.Info(DebugOption.Pool, $"创建 => 对象池 : {key.Green()}");
                poolDict.Add(key, new PoolData(obj, poolManager));
            }
        }

        /// <summary>
        /// 弹出对象池
        /// </summary>
        /// <typeparam name="T">任何可以被new的对象</typeparam>
        /// <returns>返回弹出对象</returns>
        public static T Pop<T>() where T : new()
        {
            var key = typeof(T);
            if (streamDict.ContainsKey(key) && streamDict[key].Count > 0)
            {
                return ((IPool<T>)streamDict[key]).Pop();
            }

            return new T();
        }

        /// <summary>
        /// 推入对象池
        /// </summary>
        /// <param name="obj">传入对象</param>
        /// <typeparam name="T">任何可以被new的对象</typeparam>
        public static void Push<T>(T obj) where T : new()
        {
            var key = typeof(T);
            if (streamDict.ContainsKey(key))
            {
                ((IPool<T>)streamDict[key]).Push(obj);
                return;
            }
            
            streamDict.Add(key, new Pool<T>(obj));
        }

        /// <summary>
        /// 管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            foreach (var pool in poolDict.Values)
            {
                pool.Clear();
            }

            foreach (var stack in streamDict.Values)
            {
                stack.Clear();
            }

            poolDict.Clear();
            streamDict.Clear();
            poolManager = null;
        }
    }
}