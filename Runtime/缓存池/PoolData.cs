using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    [Serializable]
    internal class PoolData
    {
        /// <summary>
        /// 游戏物体组
        /// </summary>
        [ShowInInspector] private readonly GameObject prefab;
        
        /// <summary>
        /// 游戏物体栈
        /// </summary>
        [ShowInInspector] private readonly Stack<GameObject> poolStack;
        
        /// <summary>
        /// 栈中对象数量
        /// </summary>
        public int Count => poolStack.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="prefab">推入的游戏对象</param>
        /// <param name="poolStack">池中的游戏对象栈</param>
        public PoolData(GameObject prefab, GameObject poolStack)
        {
            this.prefab = new GameObject(prefab.name + "Group");
            this.prefab.transform.SetParent(poolStack.transform);
            this.poolStack = new Stack<GameObject>();
            Push(prefab);
        }

        /// <summary>
        /// 对象池拉取对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        public GameObject Pop()
        {
            GameObject obj = poolStack.Pop();
            if (obj == null) return null;
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="obj">推出的游戏物体</param>
        public void Push(GameObject obj)
        {
            poolStack.Push(obj);
            if (prefab != null) obj.transform.SetParent(prefab.transform);
            obj.SetActive(false);
        }
    }
}