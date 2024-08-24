// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace JFramework
{
    public static partial class Extensions
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this T request) where T : AsyncOperation
        {
            var completion = new TaskCompletionSource<T>();
            request.completed += operation => completion.SetResult(operation as T);
            return completion.Task.GetAwaiter();
        }
    }
}