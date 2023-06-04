using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    internal class PoolStack : IPool
    {
        /// <summary>
        /// 栈池
        /// </summary>
        [ShowInInspector] private Stack<object> stack;

        /// <summary>
        /// 栈池的对象数
        /// </summary>
        public int Count => stack.Count;

        /// <summary>
        /// 构造函数初始化栈
        /// </summary>
        public PoolStack() => stack = new Stack<object>();

        /// <summary>
        /// 栈弹出
        /// </summary>
        /// <returns></returns>
        public object Pop() => stack.Count > 0 ? stack.Pop() : null;

        /// <summary>
        /// 栈推入
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        public void Push(object obj)
        {
            if (!stack.Contains(obj))
            {
                stack.Push(obj);
            }
        }
    }
}