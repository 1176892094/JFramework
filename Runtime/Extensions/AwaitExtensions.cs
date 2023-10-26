// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  20:57
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections;
using JFramework.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    /// <summary>
    /// 异步等待拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 等待 WaitForSeconds
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitForSeconds result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 WaitForEndOfFrame
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitForEndOfFrame result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 WaitForFixedUpdate
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitForFixedUpdate result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 WaitForSecondsRealtime
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitForSecondsRealtime result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 WaitUntil
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitUntil result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 WaitWhile
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async GetAwaiter(this WaitWhile result)
        {
            var awaiter = new Async();
            GlobalManager.Instance.StartCoroutine(Return(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 AsyncOperation
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async<AsyncOperation> GetAwaiter(this AsyncOperation result)
        {
            var awaiter = new Async<AsyncOperation>();
            GlobalManager.Instance.StartCoroutine(ReturnGeneric(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 ResourceRequest
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async<Object> GetAwaiter(this ResourceRequest result)
        {
            var awaiter = new Async<Object>();
            GlobalManager.Instance.StartCoroutine(ResourceRequest(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 AssetBundleRequest
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async<Object> GetAwaiter(this AssetBundleRequest result)
        {
            var awaiter = new Async<Object>();
            GlobalManager.Instance.StartCoroutine(AssetBundleRequest(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 等待 AssetBundleCreateRequest
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Async<AssetBundle> GetAwaiter(this AssetBundleCreateRequest result)
        {
            var awaiter = new Async<AssetBundle>();
            GlobalManager.Instance.StartCoroutine(AssetBundleCreateRequest(awaiter, result));
            return awaiter;
        }

        /// <summary>
        /// 返回 void
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static IEnumerator Return(Async awaiter, object result)
        {
            yield return result;
            awaiter.Complete(null);
        }

        /// <summary>
        /// 返回 Generic
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerator ReturnGeneric<T>(Async<T> awaiter, T result)
        {
            yield return result;
            awaiter.Complete(result, null);
        }

        /// <summary>
        /// 返回 Resource
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static IEnumerator ResourceRequest(Async<Object> awaiter, ResourceRequest result)
        {
            yield return result;
            awaiter.Complete(result.asset, null);
        }

        /// <summary>
        /// 返回 AssetBundle
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static IEnumerator AssetBundleRequest(Async<Object> awaiter, AssetBundleRequest result)
        {
            yield return result;
            awaiter.Complete(result.asset, null);
        }

        /// <summary>
        /// 返回 AssetBundleCreate
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static IEnumerator AssetBundleCreateRequest(Async<AssetBundle> awaiter, AssetBundleCreateRequest result)
        {
            yield return result;
            awaiter.Complete(result.assetBundle, null);
        }
    }
}