using System.Collections;
using System.Threading;
using UnityEngine;

namespace JFramework
{
    internal static class AsyncExtension
    {
        public static SynchronizationContext synchronize;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install() => synchronize = SynchronizationContext.Current;

        public static IEnumerator ReturnVoid(CoroutineAwaiter awaiter, object instruction)
        {
            yield return instruction;
            awaiter.Complete(null);
        }

        public static IEnumerator ReturnSelf<T>(CoroutineAwaiter<T> awaiter, T instruction)
        {
            yield return instruction;
            awaiter.Complete(instruction, null);
        }

        public static IEnumerator ResourceRequest(CoroutineAwaiter<Object> awaiter, ResourceRequest instruction)
        {
            yield return instruction;
            awaiter.Complete(instruction.asset, null);
        }

        public static IEnumerator AssetBundleRequest(CoroutineAwaiter<Object> awaiter, AssetBundleRequest instruction)
        {
            yield return instruction;
            awaiter.Complete(instruction.asset, null);
        }

        public static IEnumerator AssetBundleCreateRequest(CoroutineAwaiter<AssetBundle> awaiter, AssetBundleCreateRequest instruction)
        {
            yield return instruction;
            awaiter.Complete(instruction.assetBundle, null);
        }
    }
}