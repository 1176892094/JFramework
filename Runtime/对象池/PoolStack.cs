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
        [ShowInInspector] private Stack<object> stack;
        public int Count => stack.Count;
        public PoolStack() => stack = new Stack<object>();
        public object Pop() => stack.Count > 0 ? stack.Pop() : null;

        public void Push(object obj)
        {
            if (!stack.Contains(obj))
            {
                stack.Push(obj);
            }
        }
    }
}