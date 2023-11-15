// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:57
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class PoolManager
    {
        /// <summary>
        /// 对象池组
        /// </summary>
        internal static readonly Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();

        /// <summary>
        /// 对象池容器
        /// </summary>
        internal static readonly Dictionary<string, IPool> pools = new Dictionary<string, IPool>();

        /// <summary>
        /// 对象池管理器
        /// </summary>
        private static Transform poolManager;

        /// <summary>
        /// 获取 PoolManager 对象
        /// </summary>
        internal static void Register()
        {
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
        }

        /// <summary>
        /// 弹出对象池
        /// </summary>
        /// <typeparam name="T">任何可以被new的对象</typeparam>
        /// <returns>返回弹出对象</returns>
        public static T Pop<T>() where T : new()
        {
            if (pools.TryGetValue(typeof(T).Name, out var pool) && pool.Count > 0)
            {
                return ((IPool<T>)pool).Pop();
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
            if (pools.TryGetValue(typeof(T).Name, out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            pools.Add(typeof(T).Name, new Pool<T>(obj));
        }

        /// <summary>
        /// 对象池管理器异步获取对象
        /// </summary>
        /// <param name="path">弹出对象的路径</param>
        /// <param name="action"></param>
        public static void PopAsync<T>(string path, Action<T> action) where T : Component
        {
            if (!GlobalManager.Runtime) return;
            if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
            {
                var obj = ((IPool<GameObject>)pool).Pop();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                    obj.GetComponent<IPop>()?.OnPop();
                    action?.Invoke(obj.GetComponent<T>());
                    return;
                }
            }

            AssetManager.LoadAsync<GameObject>(path, obj =>
            {
                obj.name = path;
                Object.DontDestroyOnLoad(obj);
                obj.GetComponent<IPop>()?.OnPop();
                action?.Invoke(obj.GetComponent<T>());
            });
        }

        /// <summary>
        /// 对象池管理器异步获取对象
        /// </summary>
        /// <param name="path">弹出对象的路径</param>
        /// <param name="action"></param>
        public static void PopAsync(string path, Action<GameObject> action)
        {
            if (!GlobalManager.Runtime) return;
            if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
            {
                var obj = ((IPool<GameObject>)pool).Pop();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                    obj.GetComponent<IPop>()?.OnPop();
                    action?.Invoke(obj);
                    return;
                }
            }


            AssetManager.LoadAsync<GameObject>(path, obj =>
            {
                obj.name = path;
                Object.DontDestroyOnLoad(obj);
                obj.GetComponent<IPop>()?.OnPop();
                action?.Invoke(obj);
            });
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public static bool Push(GameObject obj)
        {
            if (!GlobalManager.Runtime) return false;
            if (obj == null) return false;
            if (pools.TryGetValue(obj.name, out var pool))
            {
                if (!((IPool<GameObject>)pool).Push(obj)) return false;
                obj.SetActive(false);
                obj.transform.SetParent(parents[obj.name].transform);
                obj.GetComponent<IPush>()?.OnPush();
                return true;
            }

            parents[obj.name] = new GameObject(obj.name + "-Pool");
            parents[obj.name].transform.SetParent(poolManager);
            obj.SetActive(false);
            obj.transform.SetParent(parents[obj.name].transform);
            obj.GetComponent<IPush>()?.OnPush();
            pools.Add(obj.name, new Pool<GameObject>(obj));
            return true;
        }

        /// <summary>
        /// 管理器卸载
        /// </summary>
        internal static void UnRegister()
        {
            foreach (var pool in pools.Values)
            {
                pool.Clear();
            }

            pools.Clear();
            parents.Clear();
        }
    }
}