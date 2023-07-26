using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
     /// <summary>
    /// 泛型对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal sealed class Pool<T> : IPool<T> where T : new()
    {
        /// <summary>
        /// 静态对象池
        /// </summary>
        [ShowInInspector] private readonly HashSet<T> pool = new HashSet<T>();

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 创建时推入对象
        /// </summary>
        /// <param name="object">传入泛型对象</param>
        public Pool(T @object) => Push(@object);

        /// <summary>
        /// 对象弹出
        /// </summary>
        /// <returns>返回对象</returns>
        public T Pop()
        {
            if (pool.Count == 0) return new T();
            var @object = pool.First();
            pool.Remove(@object);
            return @object;
        }

        /// <summary>
        /// 对象推入
        /// </summary>
        /// <param name="object">推入对象</param>
        public bool Push(T @object) => pool.Add(@object);

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear() => pool.Clear();
    }
    
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    [Serializable]
    internal sealed class Pool : IPool<GameObject>
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        [ShowInInspector] private readonly HashSet<GameObject> pool = new HashSet<GameObject>();

        /// <summary>
        /// 游戏物体组
        /// </summary>
        private readonly Transform transform;

        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="object">推入的游戏对象</param>
        public Pool(GameObject @object)
        {
            transform = new GameObject(@object.name + "-Pool").transform;
            transform.SetParent(GlobalManager.poolManager);
            Push(@object);
        }

        /// <summary>
        /// 对象池弹出对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        public GameObject Pop()
        {
            if (pool.Count == 0) return null;
            var @object = pool.First();
            @object.transform.SetParent(null);
            @object.SetActive(true);
            pool.Remove(@object);
            return @object;
        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="object">推出的游戏物体</param>
        public bool Push(GameObject @object)
        {
            @object.SetActive(false);
            if (transform == null) return false;
            @object.transform.SetParent(transform);
            return pool.Add(@object);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            foreach (var @object in pool)
            {
                Push(@object);
            }

            pool.Clear();
        }
    }
}