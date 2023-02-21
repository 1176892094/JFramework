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
    internal class PoolData : Pool<GameObject>
    {
        /// <summary>
        /// 游戏物体组
        /// </summary>
        [ShowInInspector, LabelText("对象池组")] private GameObject poolGroup;

        /// <summary>
        /// 游戏物体栈
        /// </summary>
        [ShowInInspector, LabelText("对象池栈")] private Stack<GameObject> poolManager;

        /// <summary>
        /// 栈中对象数量
        /// </summary>
        public override int Count => poolManager.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="poolGroup">推入的游戏对象</param>
        /// <param name="poolManager">池中的游戏对象栈</param>
        public void Awake(GameObject poolGroup, GameObject poolManager)
        {
            this.poolManager = new Stack<GameObject>();
            this.poolGroup = new GameObject(poolGroup.name + "Group");
            this.poolGroup.transform.SetParent(poolManager.transform);
            Push(poolGroup);
        }

        /// <summary>
        /// 对象池拉取对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        protected override GameObject Pop()
        {
            GameObject obj = poolManager.Pop();
            if (obj == null) return null;
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="obj">推出的游戏物体</param>
        protected override void Push(GameObject obj)
        {
            poolManager.Push(obj);
            if (poolGroup != null) obj.transform.SetParent(poolGroup.transform);
            obj.SetActive(false);
        }
    }
}