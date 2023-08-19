using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class PoolManager
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        internal static readonly Dictionary<string, IPool> pools = new Dictionary<string, IPool>();

        /// <summary>
        /// 对象池管理器对象
        /// </summary>
        internal static Transform poolManager;

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
                return ((Pool<T>)pool).Pop();
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
                ((Pool<T>)pool).Push(obj);
                return;
            }

            pools.Add(typeof(T).Name, new Pool<T>(obj));
        }

        /// <summary>
        /// 对象池管理器异步获取对象 (生成并返回结果)
        /// </summary>
        /// <param name="path">弹出对象的路径</param>
        public static async Task<T> Pop<T>(string path) where T : Component
        {
            if (!GlobalManager.Runtime) return null;
            if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
            {
                var @object = ((Pool)pool).Pop();
                if (@object != null)
                {
                    Log.Info($"取出 {path.Pink()} 对象成功", Option.Pool);
                    return @object.GetComponent<T>();
                }

                Log.Info($"移除已销毁对象 : {path.Red()}", Option.Pool);
            }

            var obj = await AssetManager.LoadAsync<GameObject>(path);
            Object.DontDestroyOnLoad(obj);
            Log.Info($"创建 {path.Green()} 对象成功", Option.Pool);
            obj.GetComponent<IPop>()?.OnPop();
            obj.name = path;
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public static void Push(GameObject obj)
        {
            if (!GlobalManager.Runtime) return;
            if (obj == null)
            {
                Debug.LogWarning($"{nameof(PoolManager).Sky()} 存入对象已被销毁");
                return;
            }

            if (pools.TryGetValue(obj.name, out var pool))
            {
                Log.Info($"存入 {obj.name.Pink()} 对象成功", Option.Pool);
                ((Pool)pool).Push(obj);
                return;
            }

            Log.Info($"创建对象池 : {obj.name.Green()}", Option.Pool);
            pools.Add(obj.name, new Pool(obj));
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
        }
    }
}