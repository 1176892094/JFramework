// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-14  15:32
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************


using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 异步请求的拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 异步操作拓展 await 关键字
        /// </summary>
        /// <param name="request"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TaskAwaiter<T> GetAwaiter<T>(this T request) where T : AsyncOperation
        {
            var completion = new TaskCompletionSource<T>();
            request.completed += operation => completion.SetResult(operation as T);
            return completion.Task.GetAwaiter();
        }
    }
}