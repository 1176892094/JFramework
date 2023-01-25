using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        /// <summary>
        /// 对象池控制器
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("对象池控制器"), FoldoutGroup("对象池管理视图")]
        private GameObject controller;

        /// <summary>
        /// 存储所有池的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("对象池数据"), FoldoutGroup("对象池管理视图")]
        private Dictionary<string, PoolData> poolDict;

        /// <summary>
        /// 对象池初始化
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            poolDict = new Dictionary<string, PoolData>();
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="key">拉取对象的名称</param>
        /// <param name="action">拉取对象的回调</param>
        public void Pop(string key, Action<GameObject> action)
        {
            if (poolDict.ContainsKey(key) && poolDict[key].Count > 0)
            {
                GameObject obj = poolDict[key].Pop();
                if (obj != null)
                {
                    action?.Invoke(obj);
                }
                else
                {
                    poolDict.Remove(key);
                    AssetManager.Instance.LoadAsync<GameObject>(key, o =>
                    {
                        o.name = key;
                        action?.Invoke(o);
                    });
                }
            }
            else
            {
                AssetManager.Instance.LoadAsync<GameObject>(key, o =>
                {
                    o.name = key;
                    action?.Invoke(o);
                });
            }
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public void Push(GameObject obj)
        {
            if (obj == null) return;
            string key = obj.name;
            if (controller == null)
            {
                controller = new GameObject(Global.PoolManager);
            }

            if (poolDict.ContainsKey(key))
            {
                if (obj == null)
                {
                    Debug.LogWarning(key + "已被销毁,无法推入对象！");
                    poolDict[key].Pop();
                    return;
                }

                poolDict[key].Push(obj);
            }
            else
            {
                poolDict.Add(key, new PoolData(obj, controller));
            }
        }
    }
}