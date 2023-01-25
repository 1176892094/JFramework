using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JFramework.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static class AwaitExtension
    {
        public static CoroutineAwaiter GetAwaiter(this WaitForSeconds instruction)
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
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(AsyncExtension.ResourceRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest instruction)
        {
            var awaiter = new CoroutineAwaiter<AssetBundle>();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(AsyncExtension.AssetBundleCreateRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<Object> GetAwaiter(this AssetBundleRequest instruction)
        {
            var awaiter = new CoroutineAwaiter<Object>();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(AsyncExtension.AssetBundleRequest(awaiter, instruction)));
            return awaiter;
        }

        public static CoroutineAwaiter<T> GetAwaiter<T>(this IEnumerator<T> coroutine)
        {
            var awaiter = new CoroutineAwaiter<T>();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(new CoroutineWrapper<T>(coroutine, awaiter).Run()));
            return awaiter;
        }

        public static CoroutineAwaiter<object> GetAwaiter(this IEnumerator coroutine)
        {
            var awaiter = new CoroutineAwaiter<object>();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(new CoroutineWrapper<object>(coroutine, awaiter).Run()));
            return awaiter;
        }

        private static CoroutineAwaiter GetAwaiterReturnVoid(object instruction)
        {
            var awaiter = new CoroutineAwaiter();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(AsyncExtension.ReturnVoid(awaiter, instruction)));
            return awaiter;
        }

        private static CoroutineAwaiter<T> GetAwaiterReturnSelf<T>(T instruction)
        {
            var awaiter = new CoroutineAwaiter<T>();
            RunOnUnityScheduler(() =>
                GlobalManager.Instance.StartCoroutine(AsyncExtension.ReturnSelf(awaiter, instruction)));
            return awaiter;
        }

        public static void RunOnUnityScheduler(Action callback)
        {
            if (SynchronizationContext.Current == AsyncExtension.synchronize)
            {
                callback();
            }
            else
            {
                AsyncExtension.synchronize.Post(_ => callback(), null);
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