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
        /// 存储所有池的字典
        /// </summary>
        internal static Dictionary<string, IPool> poolDict;

        /// <summary>
        /// 对象池管理器
        /// </summary>
        private static string Name => nameof(PoolManager);

        /// <summary>
        /// 对象池控制器
        /// </summary>
        internal static Transform poolManager;

        /// <summary>
        /// 对象池管理器初始化
        /// </summary>
        internal static void Awake()
        {
            poolDict = new Dictionary<string, IPool>();
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="key">拉取对象的名称</param>
        public static async Task<T> Pop<T>(string key) where T: Object
        {
            if (!GlobalManager.Runtime) return null;
            if (poolDict.ContainsKey(key) && poolDict[key].Count > 0)
            {
                var poolObj = (T)poolDict[key].Pop();

                if (poolObj != null)
                {
                    GlobalManager.Logger(DebugOption.Pool,$"取出 => {key.Pink()} 对象成功");
                    return poolObj;
                }

                GlobalManager.Logger(DebugOption.Pool,$"移除已销毁对象 : {key.Red()}");
            }

            var obj = await AssetManager.LoadAsync<GameObject>(key);
            obj.name = key;
            GlobalManager.Logger(DebugOption.Pool, $"创建 => {key.Green()} 对象成功");
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
                    Debug.Log($"{Name.Sky()} 移除已销毁对象 : {key.Red()}");
                    poolDict[key].Pop();
                    return;
                }
                
                GlobalManager.Logger(DebugOption.Pool,$"存入 => {key.Pink()} 对象成功");
                poolDict[key].Push(obj);
            }
            else
            {
                GlobalManager.Logger(DebugOption.Pool,$"创建 => 对象池 : {key.Green()}");
                poolDict.Add(key, new PoolData(obj, poolManager));
            }
        }

        internal static void Destroy()
        {
            poolManager = null;
            poolDict = null;
        }
    }
}