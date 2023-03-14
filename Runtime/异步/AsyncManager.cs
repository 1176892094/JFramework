using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static class AsyncManager
    {
        private static SynchronizationContext synchronize;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Awake() => synchronize = SynchronizationContext.Current;

        public static CoroutineAwaiter GetAwaiter(this WaitForSeconds result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter GetAwaiter(this WaitForEndOfFrame result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter GetAwaiter(this WaitForFixedUpdate result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter GetAwaiter(this WaitForSecondsRealtime result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter GetAwaiter(this WaitUntil result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter GetAwaiter(this WaitWhile result) => GetAwaiterNone(result);
        
        public static CoroutineAwaiter<AsyncOperation> GetAwaiter(this AsyncOperation result) => GetAwaiterSelf(result);

        public static CoroutineAwaiter<Object> GetAwaiter(this ResourceRequest result)
        {
            var awaiter = new CoroutineAwaiter<Object>();
            Run(() => GlobalManager.Instance.StartCoroutine(ResourceRequest(awaiter, result)));
            return awaiter;
        }

        public static CoroutineAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest result)
        {
            var awaiter = new CoroutineAwaiter<AssetBundle>();
            Run(() => GlobalManager.Instance.StartCoroutine(AssetBundleCreateRequest(awaiter, result)));
            return awaiter;
        }

        public static CoroutineAwaiter<Object> GetAwaiter(this AssetBundleRequest result)
        {
            var awaiter = new CoroutineAwaiter<Object>();
            Run(() => GlobalManager.Instance.StartCoroutine(AssetBundleRequest(awaiter, result)));
            return awaiter;
        }

        public static CoroutineAwaiter<T> GetAwaiter<T>(this IEnumerator<T> coroutine)
        {
            var awaiter = new CoroutineAwaiter<T>();
            Run(() => GlobalManager.Instance.StartCoroutine(new CoroutineWrapper<T>(coroutine, awaiter).Run()));
            return awaiter;
        }

        public static CoroutineAwaiter<object> GetAwaiter(this IEnumerator coroutine)
        {
            var awaiter = new CoroutineAwaiter<object>();
            Run(() => GlobalManager.Instance.StartCoroutine(new CoroutineWrapper<object>(coroutine, awaiter).Run()));
            return awaiter;
        }

        private static CoroutineAwaiter GetAwaiterNone(object result)
        {
            var awaiter = new CoroutineAwaiter();
            Run(() => GlobalManager.Instance.StartCoroutine(ReturnNone(awaiter, result)));
            return awaiter;
        }

        private static CoroutineAwaiter<T> GetAwaiterSelf<T>(T result)
        {
            var awaiter = new CoroutineAwaiter<T>();
            Run(() => GlobalManager.Instance.StartCoroutine(ReturnSelf(awaiter, result)));
            return awaiter;
        }

        private static IEnumerator ReturnNone(CoroutineAwaiter awaiter, object result)
        {
            yield return result;
            awaiter.Complete(null);
        }

        private static IEnumerator ReturnSelf<T>(CoroutineAwaiter<T> awaiter, T result)
        {
            yield return result;
            awaiter.Complete(result, null);
        }

        private static IEnumerator ResourceRequest(CoroutineAwaiter<Object> awaiter, ResourceRequest result)
        {
            yield return result;
            awaiter.Complete(result.asset, null);
        }

        private static IEnumerator AssetBundleRequest(CoroutineAwaiter<Object> awaiter, AssetBundleRequest result)
        {
            yield return result;
            awaiter.Complete(result.asset, null);
        }

        private static IEnumerator AssetBundleCreateRequest(CoroutineAwaiter<AssetBundle> awaiter, AssetBundleCreateRequest result)
        {
            yield return result;
            awaiter.Complete(result.assetBundle, null);
        }

        public static void Run(Action action)
        {
            if (SynchronizationContext.Current != synchronize)
            {
                synchronize.Post(_ => action?.Invoke(), null);
                return;
            }

            action?.Invoke();
        }

        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("异步执行异常！".Red());
            }
        }
    }
}