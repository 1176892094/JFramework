using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    public static class PoolManager
    {
        internal static Dictionary<string, IPool> poolDict;
        internal static Transform poolManager;

        internal static void Awake()
        {
            poolDict = new Dictionary<string, IPool>();
            var transform = GlobalManager.Instance.transform;
            poolManager = transform.Find("PoolManager");
        }

        internal static void Destroy()
        {
            poolManager = null;
            poolDict = null;
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="path">弹出对象的路径</param>
        public static async Task<T> Pop<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            if (poolDict.ContainsKey(path) && poolDict[path].Count > 0)
            {
                var poolObj = (GameObject)poolDict[path].Pop();
                if (poolObj != null)
                {
                    Log.Info(DebugOption.Pool, $"取出 => {path.Pink()} 对象成功");
                    return poolObj.GetComponent<T>();
                }

                Log.Info(DebugOption.Pool, $"移除已销毁对象 : {path.Red()}");
            }

            var obj = await AssetManager.LoadAsync<GameObject>(path);
            obj.name = path;
            Log.Info(DebugOption.Pool, $"创建 => {path.Green()} 对象成功");
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
                    Log.Warn($"{nameof(PoolManager).Sky()} 移除已销毁对象 : {key.Red()}");
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
    }
}