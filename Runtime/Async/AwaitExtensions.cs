using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Async
{
    public static class AwaitExtensions
    {
        public static SimpleCoroutineAwaiter GetAwaiter(this WaitForSeconds instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitForUpdate instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitForEndOfFrame instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitForFixedUpdate instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitForSecondsRealtime instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitUntil instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter GetAwaiter(this WaitWhile instruction)
        {
            return GetAwaiterReturnVoid(instruction);
        }

        public static SimpleCoroutineAwaiter<AsyncOperation> GetAwaiter(this AsyncOperation instruction)
        {
            return GetAwaiterReturnSelf(instruction);
        }

        public static SimpleCoroutineAwaiter<Object> GetAwaiter(this ResourceRequest instruction)
        {
            var awaiter = new SimpleCoroutineAwaiter<Object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(InstructionWrappers.ResourceRequest(awaiter, instruction)));
            return awaiter;
        }

        public static SimpleCoroutineAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest instruction)
        {
            var awaiter = new SimpleCoroutineAwaiter<AssetBundle>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(InstructionWrappers.AssetBundleCreateRequest(awaiter, instruction)));
            return awaiter;
        }

        public static SimpleCoroutineAwaiter<Object> GetAwaiter(this AssetBundleRequest instruction)
        {
            var awaiter = new SimpleCoroutineAwaiter<Object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(InstructionWrappers.AssetBundleRequest(awaiter, instruction)));
            return awaiter;
        }

        public static SimpleCoroutineAwaiter<T> GetAwaiter<T>(this IEnumerator<T> coroutine)
        {
            var awaiter = new SimpleCoroutineAwaiter<T>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(new CoroutineWrapper<T>(coroutine, awaiter).Run()));
            return awaiter;
        }

        public static SimpleCoroutineAwaiter<object> GetAwaiter(this IEnumerator coroutine)
        {
            var awaiter = new SimpleCoroutineAwaiter<object>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(new CoroutineWrapper<object>(coroutine, awaiter).Run()));
            return awaiter;
        }

        private static SimpleCoroutineAwaiter GetAwaiterReturnVoid(object instruction)
        {
            var awaiter = new SimpleCoroutineAwaiter();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(InstructionWrappers.ReturnVoid(awaiter, instruction)));
            return awaiter;
        }

        private static SimpleCoroutineAwaiter<T> GetAwaiterReturnSelf<T>(T instruction)
        {
            var awaiter = new SimpleCoroutineAwaiter<T>();
            RunOnUnityScheduler(() => MonoManager.Instance.StartCoroutine(InstructionWrappers.ReturnSelf(awaiter, instruction)));
            return awaiter;
        }

        private static void RunOnUnityScheduler(Action action)
        {
            if (SynchronizationContext.Current == UnitySynchronize.synchronizationContext)
            {
                action();
            }
            else
            {
                UnitySynchronize.synchronizationContext.Post( _ => action(), null);
            }
        }

        private static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("Assert hit in UnityAsyncUtil package!");
            }
        }

        public class SimpleCoroutineAwaiter<T> : INotifyCompletion
        {
            private T result;
            private bool isCompleted;
            private Action continuation;
            private Exception exception;
            public bool IsCompleted => isCompleted;

            public T GetResult()
            {
                Assert(isCompleted);
                if (exception != null)
                {
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }

                return result;
            }

            public void Complete(T result, Exception e)
            {
                Assert(!isCompleted);
                isCompleted = true;
                exception = e;
                this.result = result;
                if (continuation != null)
                {
                    RunOnUnityScheduler(continuation);
                }
            }

            void INotifyCompletion.OnCompleted(Action continuation)
            {
                Assert(this.continuation == null);
                Assert(!isCompleted);
                this.continuation = continuation;
            }
        }

        public class SimpleCoroutineAwaiter : INotifyCompletion
        {
            private bool isCompleted;
            private Action continuation;
            private Exception exception;
            public bool IsCompleted => isCompleted;

            public void GetResult()
            {
                Assert(isCompleted);
                if (exception != null)
                {
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
            }

            public void Complete(Exception e)
            {
                Assert(!isCompleted);
                isCompleted = true;
                exception = e;
                if (continuation != null)
                {
                    RunOnUnityScheduler(continuation);
                }
            }

            void INotifyCompletion.OnCompleted(Action continuation)
            {
                Assert(this.continuation == null);
                Assert(!isCompleted);
                this.continuation = continuation;
            }
        }

        private class CoroutineWrapper<T>
        {
            readonly SimpleCoroutineAwaiter<T> awaiter;
            readonly Stack<IEnumerator> processStack;

            public CoroutineWrapper(IEnumerator coroutine, SimpleCoroutineAwaiter<T> awaiter)
            {
                processStack = new Stack<IEnumerator>();
                processStack.Push(coroutine);
                this.awaiter = awaiter;
            }

            public IEnumerator Run()
            {
                while (true)
                {
                    var topWorker = processStack.Peek();
                    bool isDone;
                    try
                    {
                        isDone = !topWorker.MoveNext();
                    }
                    catch (Exception e)
                    {
                        var objectTrace = GenerateObjectTrace(processStack);
                        awaiter.Complete(default(T), objectTrace.Any() ? new Exception(GenerateObjectTraceMessage(objectTrace), e) : e);
                        yield break;
                    }

                    if (isDone)
                    {
                        processStack.Pop();

                        if (processStack.Count == 0)
                        {
                            awaiter.Complete((T)topWorker.Current, null);
                            yield break;
                        }
                    }
                    
                    if (topWorker.Current is IEnumerator)
                    {
                        processStack.Push((IEnumerator)topWorker.Current);
                    }
                    else
                    {
                        yield return topWorker.Current;
                    }
                }
            }

            private string GenerateObjectTraceMessage(List<Type> objTrace)
            {
                var result = new StringBuilder();

                foreach (var objType in objTrace)
                {
                    if (result.Length != 0)
                    {
                        result.Append(" -> ");
                    }

                    result.Append(objType);
                }

                result.AppendLine();
                return "Unity Coroutine Object Trace: " + result;
            }

            private static List<Type> GenerateObjectTrace(IEnumerable<IEnumerator> enumerators)
            {
                var objTrace = new List<Type>();

                foreach (var enumerator in enumerators)
                {
                    var field = enumerator.GetType().GetField("$this", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                    if (field == null)
                    {
                        continue;
                    }

                    var obj = field.GetValue(enumerator);

                    if (obj == null)
                    {
                        continue;
                    }

                    var objType = obj.GetType();

                    if (!objTrace.Any() || objType != objTrace.Last())
                    {
                        objTrace.Add(objType);
                    }
                }

                objTrace.Reverse();
                return objTrace;
            }
        }

        private static class InstructionWrappers
        {
            public static IEnumerator ReturnVoid(SimpleCoroutineAwaiter awaiter, object instruction)
            {
                yield return instruction;
                awaiter.Complete(null);
            }

            public static IEnumerator AssetBundleCreateRequest(SimpleCoroutineAwaiter<AssetBundle> awaiter, AssetBundleCreateRequest instruction)
            {
                yield return instruction;
                awaiter.Complete(instruction.assetBundle, null);
            }

            public static IEnumerator ReturnSelf<T>(SimpleCoroutineAwaiter<T> awaiter, T instruction)
            {
                yield return instruction;
                awaiter.Complete(instruction, null);
            }

            public static IEnumerator AssetBundleRequest(SimpleCoroutineAwaiter<Object> awaiter, AssetBundleRequest instruction)
            {
                yield return instruction;
                awaiter.Complete(instruction.asset, null);
            }

            public static IEnumerator ResourceRequest(SimpleCoroutineAwaiter<Object> awaiter, ResourceRequest instruction)
            {
                yield return instruction;
                awaiter.Complete(instruction.asset, null);
            }
        }
    }
}