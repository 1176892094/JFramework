using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Async
{
    public static class AwaitExtension
    {
        public static CoroutineAwaiter GetAwaiter(this WaitForSeconds instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitForUpdate instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitForEndOfFrame instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitForFixedUpdate instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitForSecondsRealtime instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitUntil instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter GetAwaiter(this WaitWhile instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static CoroutineAwaiter<AsyncOperation> GetAwaiter(this AsyncOperation instruction)
        {
            return GetAwaiterReturnSelf(instruction);
        }

        public static CoroutineAwaiter<Object> GetAwaiter(this ResourceRequest instruction)
        {
            var awaiter = new CoroutineAwaiter<Object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(AsyncManager.ResourceRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest instruction)
        {
            var awaiter = new CoroutineAwaiter<AssetBundle>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(AsyncManager.AssetBundleCreateRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<Object> GetAwaiter(this AssetBundleRequest instruction)
        {
            var awaiter = new CoroutineAwaiter<Object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(AsyncManager.AssetBundleRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<T> GetAwaiter<T>(this IEnumerator<T> coroutine)
        {
            var awaiter = new CoroutineAwaiter<T>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(new CoroutineWrapper<T>(coroutine, awaiter).Run()));
            return awaiter;
        }

        public static CoroutineAwaiter<object> GetAwaiter(this IEnumerator coroutine)
        {
            var awaiter = new CoroutineAwaiter<object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(new CoroutineWrapper<object>(coroutine, awaiter).Run()));
            return awaiter;
        }

        private static CoroutineAwaiter GetAwaiterReturnVoid(object instruction)
        {
            var awaiter = new CoroutineAwaiter();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(AsyncManager.ReturnVoid(awaiter, instruction)));
            return awaiter;
        }

        private static CoroutineAwaiter<T> GetAwaiterReturnSelf<T>(T instruction)
        {
            var awaiter = new CoroutineAwaiter<T>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(AsyncManager.ReturnSelf(awaiter, instruction)));
            return awaiter;
        }

        public static void RunOnUnityScheduler(Action callback)
        {
            if (SynchronizationContext.Current == AsyncManager.synchronize)
            {
                callback();
            }
            else
            {
                AsyncManager.synchronize.Post( _ => callback(), null);
            }
        }

        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("Assert hit in AsyncAwait of JFramewrok!");
            }
        }
    }
}