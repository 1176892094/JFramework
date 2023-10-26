// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  22:53
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
   
    /// <summary>
    /// 泛型协程等待器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Async<T> : INotifyCompletion
    {
        /// <summary>
        /// 泛型结果
        /// </summary>
        private T result;

        /// <summary>
        /// 协程方法
        /// </summary>
        private Action continuation;

        /// <summary>
        /// 异常捕获
        /// </summary>
        private Exception exception;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 获取结果
        /// </summary>
        public T GetResult()
        {
            Debug.Assert(IsCompleted);
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return result;
        }
        
        /// <summary>
        /// 协程完成
        /// </summary>
        /// <param name="result"></param>
        /// <param name="exception"></param>
        public void Complete(T result, Exception exception)
        {
            Debug.Assert(!IsCompleted);
            IsCompleted = true;
            this.result = result;
            this.exception = exception;
            continuation?.Invoke();
        }

        /// <summary>
        /// 通知完成方法
        /// </summary>
        /// <param name="continuation"></param>
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Debug.Assert(this.continuation == null);
            Debug.Assert(!IsCompleted);
            this.continuation = continuation;
        }
    }
}